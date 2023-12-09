using GoruntuIsleme.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoruntuIsleme.Services
{
    public class ImageService:IImageService
    {
        private Bitmap ContraharmonicMeanFilterImage(Bitmap image, int kernelSize, double Q)
        {
            int width = image.Width;
            int height = image.Height;

            Bitmap result = new Bitmap(width, height);

            int kernelRadius = kernelSize / 2;

            for (int y = kernelRadius; y < height - kernelRadius; y++)
            {
                for (int x = kernelRadius; x < width - kernelRadius; x++)
                {
                    double sumNumeratorRed = 0.0, sumNumeratorGreen = 0.0, sumNumeratorBlue = 0.0;
                    double sumDenominatorRed = 0.0, sumDenominatorGreen = 0.0, sumDenominatorBlue = 0.0;

                    for (int ky = -kernelRadius; ky <= kernelRadius; ky++)
                    {
                        for (int kx = -kernelRadius; kx <= kernelRadius; kx++)
                        {
                            int pixelX = x + kx;
                            int pixelY = y + ky;

                            Color pixelColor = image.GetPixel(pixelX, pixelY);
                            int pixelRed = pixelColor.R;
                            int pixelGreen = pixelColor.G;
                            int pixelBlue = pixelColor.B;

                            sumNumeratorRed += Math.Pow(pixelRed, Q + 1);
                            sumNumeratorGreen += Math.Pow(pixelGreen, Q + 1);
                            sumNumeratorBlue += Math.Pow(pixelBlue, Q + 1);

                            sumDenominatorRed += Math.Pow(pixelRed, Q);
                            sumDenominatorGreen += Math.Pow(pixelGreen, Q);
                            sumDenominatorBlue += Math.Pow(pixelBlue, Q);
                        }
                    }

                    int index = (y * width) + x;

                    int resultRed = (int)(sumNumeratorRed / sumDenominatorRed);
                    int resultGreen = (int)(sumNumeratorGreen / sumDenominatorGreen);
                    int resultBlue = (int)(sumNumeratorBlue / sumDenominatorBlue);

                    resultRed = Math.Min(255, Math.Max(0, resultRed));
                    resultGreen = Math.Min(255, Math.Max(0, resultGreen));
                    resultBlue = Math.Min(255, Math.Max(0, resultBlue));

                    result.SetPixel(x, y, Color.FromArgb(resultRed, resultGreen, resultBlue));
                }
            }

            return result;
        }
        private Bitmap MedianFilterImage(Bitmap image, int kernelSize)
        {
            int width = image.Width;
            int height = image.Height;

            Bitmap result = new Bitmap(width, height);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color[] neighborhood = KomsulariAl(image, x, y, kernelSize);
                    Color medianColor = MedianColorHesapla(neighborhood);

                    result.SetPixel(x, y, medianColor);
                }
            }

            return result;
        }
        private Color MedianColorHesapla(Color[] colors)
        {
            byte[] redValues = colors.Select(c => c.R).OrderBy(r => r).ToArray();
            byte[] greenValues = colors.Select(c => c.G).OrderBy(g => g).ToArray();
            byte[] blueValues = colors.Select(c => c.B).OrderBy(b => b).ToArray();

            int medianIndex = colors.Length / 2;

            byte medianRed = redValues[medianIndex];
            byte medianGreen = greenValues[medianIndex];
            byte medianBlue = blueValues[medianIndex];

            return Color.FromArgb(medianRed, medianGreen, medianBlue);
        }
        private Color[] KomsulariAl(Bitmap image, int x, int y, int kernelSize)
        {
            int kernelRadius = kernelSize / 2;
            int startX = Math.Max(0, x - kernelRadius);
            int startY = Math.Max(0, y - kernelRadius);
            int endX = Math.Min(image.Width - 1, x + kernelRadius);
            int endY = Math.Min(image.Height - 1, y + kernelRadius);

            Color[] komsular = new Color[(endX - startX + 1) * (endY - startY + 1)];

            int index = 0;

            for (int i = startY; i <= endY; i++)
            {
                for (int j = startX; j <= endX; j++)
                {
                    komsular[index] = image.GetPixel(j, i);
                    index++;
                }
            }

            return komsular;
        }
        private double[,] CreateMeanKernel(int size)
        {
            double[,] kernel = new double[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    kernel[i, j] = 1.0 / (size * size);
                }
            }

            return kernel;
        }
        private Bitmap ConvolutionFilter(Bitmap image, double[,] kernel)
        {
            int width = image.Width;
            int height = image.Height;

            BitmapData srcData = image.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            int bytes = srcData.Stride * srcData.Height;
            byte[] buffer = new byte[bytes];
            System.Runtime.InteropServices.Marshal.Copy(srcData.Scan0, buffer, 0, bytes);
            image.UnlockBits(srcData);

            Bitmap result = new Bitmap(width, height);
            BitmapData destData = result.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            int stride = destData.Stride;
            byte[] resultBuffer = new byte[bytes];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    double red = 0.0, green = 0.0, blue = 0.0;

                    for (int ky = -kernel.GetLength(0) / 2; ky <= kernel.GetLength(0) / 2; ky++)
                    {
                        for (int kx = -kernel.GetLength(1) / 2; kx <= kernel.GetLength(1) / 2; kx++)
                        {
                            int pixelX = x + kx;
                            int pixelY = y + ky;

                            if (pixelX >= 0 && pixelX < width && pixelY >= 0 && pixelY < height)
                            {
                                int offset = (pixelY * srcData.Stride) + (pixelX * 4);
                                double weight = kernel[ky + kernel.GetLength(0) / 2, kx + kernel.GetLength(1) / 2];

                                red += buffer[offset + 2] * weight;
                                green += buffer[offset + 1] * weight;
                                blue += buffer[offset] * weight;
                            }
                        }
                    }

                    int index = (y * stride) + (x * 4);
                    resultBuffer[index + 2] = (byte)Math.Min(255, Math.Max(0, red));
                    resultBuffer[index + 1] = (byte)Math.Min(255, Math.Max(0, green));
                    resultBuffer[index] = (byte)Math.Min(255, Math.Max(0, blue));
                    resultBuffer[index + 3] = 255; // Alpha değeri
                }
            }

            System.Runtime.InteropServices.Marshal.Copy(resultBuffer, 0, destData.Scan0, bytes);
            result.UnlockBits(destData);

            return result;
        }
        private Bitmap ConvolutionFilter(Bitmap image, int[,] horizontalKernel, int[,] verticalKernel)
        {
            int width = image.Width;
            int height = image.Height;

            BitmapData srcData = image.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            int bytes = srcData.Stride * srcData.Height;
            byte[] buffer = new byte[bytes];
            System.Runtime.InteropServices.Marshal.Copy(srcData.Scan0, buffer, 0, bytes);
            image.UnlockBits(srcData);

            Bitmap result = new Bitmap(width, height);
            BitmapData destData = result.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            int stride = destData.Stride;
            byte[] resultBuffer = new byte[bytes];

            for (int y = 1; y < height - 1; y++)
            {
                for (int x = 1; x < width - 1; x++)
                {
                    int horizontalGradient = 0;
                    int verticalGradient = 0;

                    for (int ky = -1; ky <= 1; ky++)
                    {
                        for (int kx = -1; kx <= 1; kx++)
                        {
                            int pixelX = x + kx;
                            int pixelY = y + ky;
                            int offset = (pixelY * srcData.Stride) + (pixelX * 4);
                            int grayValue = (buffer[offset] + buffer[offset + 1] + buffer[offset + 2]) / 3;

                            horizontalGradient += grayValue * horizontalKernel[ky + 1, kx + 1];
                            verticalGradient += grayValue * verticalKernel[ky + 1, kx + 1];
                        }
                    }

                    int index = (y * stride) + (x * 4);
                    int edgeGradient = (int)Math.Sqrt(horizontalGradient * horizontalGradient + verticalGradient * verticalGradient);

                    resultBuffer[index] = resultBuffer[index + 1] = resultBuffer[index + 2] = (byte)(edgeGradient > 128 ? 255 : 0);
                    resultBuffer[index + 3] = 255;
                }
            }

            System.Runtime.InteropServices.Marshal.Copy(resultBuffer, 0, destData.Scan0, bytes);
            result.UnlockBits(destData);

            return result;
        }
        public Bitmap ApplyGaussianBlur(Bitmap image, double sigma)
        {
            int radius = Convert.ToInt32(Math.Ceiling(3 * sigma));

            int size = 2 * radius + 1;
            double[,] kernel = new double[size, size];

            double normalizer = 0.0;

            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    double weight = Math.Exp(-(x * x + y * y) / (2 * sigma * sigma));
                    kernel[x + radius, y + radius] = weight;
                    normalizer += weight;
                }
            }

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    kernel[x, y] /= normalizer;
                }
            }

            Bitmap result = ConvolutionFilter(image, kernel);

            return result;
        }

        /// <summary>
        /// Eşik değer şimdilik 128 kullanıcıdan alınabilir.
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public Bitmap ConvertToBlackWhite(Bitmap bmp)
        {
            for (int i = 0; i < bmp.Height - 1; i++)
            {
                for (int j = 0; j < bmp.Width - 1; j++)
                {
                    int deger = (bmp.GetPixel(j, i).R + bmp.GetPixel(j, i).G + bmp.GetPixel(j, i).B) / 3;

                    Color renk = (deger > 128) ? Color.White : Color.Black;

                    bmp.SetPixel(j, i, renk);
                }
            }
            return bmp;
        }

        public Bitmap GriSeviyeDonustur(Bitmap bmp)
        {
            for (int i = 0; i < bmp.Height - 1; i++)
            {
                for (int j = 0; j < bmp.Width - 1; j++)
                {
                    int deger = (bmp.GetPixel(j, i).R + bmp.GetPixel(j, i).G + bmp.GetPixel(j, i).B) / 3;
                    Color renk;
                    renk = Color.FromArgb(deger, deger, deger);

                    bmp.SetPixel(j, i, renk);
                }
            }
            return bmp;
        }

        public Bitmap KesipAl(PictureBox pictureBox, int startX, int startY, int width, int height)
        {
            Bitmap originalImage = new Bitmap(pictureBox.ClientSize.Width, pictureBox.ClientSize.Height);
            pictureBox.DrawToBitmap(originalImage, pictureBox.ClientRectangle);

            if (startX < 0) startX = 0;
            if (startY < 0) startY = 0;
            if (startX + width > originalImage.Width) width = originalImage.Width - startX;
            if (startY + height > originalImage.Height) height = originalImage.Height - startY;

            Rectangle sourceRect = new Rectangle(startX, startY, width, height);
            Bitmap croppedImage = new Bitmap(width, height);

            using (Graphics g = Graphics.FromImage(croppedImage))
            {
                g.DrawImage(originalImage, 0, 0, sourceRect, GraphicsUnit.Pixel);
            }

            return croppedImage;
        }

        public Bitmap ZoomIn(Bitmap bmp, double scaleFactor)
        {
            int newWidth = (int)(bmp.Width * scaleFactor);
            int newHeight = (int)(bmp.Height * scaleFactor);
            Bitmap newBitmap = new Bitmap(newWidth, newHeight);

            using (Graphics g = Graphics.FromImage(newBitmap))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(bmp, 0, 0, newWidth, newHeight);
            }

            return newBitmap;
        }


        public Bitmap ZoomOut(Bitmap bmp, double scaleFactor)
        {
            int newWidth = (int)(bmp.Width / scaleFactor);
            int newHeight = (int)(bmp.Height / scaleFactor);
            Bitmap newBitmap = new Bitmap(newWidth, newHeight);

            using (Graphics g = Graphics.FromImage(newBitmap))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(bmp, 0, 0, newWidth, newHeight);
            }

            return newBitmap;
        }

        public Bitmap ApplySharpenFilter(Bitmap image, double strength)
        {
            double[,] kernel = {
            {-1, -1, -1},
            {-1,  9 + strength, -1},
            {-1, -1, -1}
            };
            Bitmap result = ConvolutionFilter(image, kernel);
            return result;
        }

        public Bitmap ApplyEdgeDetectionFilter(Bitmap image)
        {
            int[,] horizontalKernel = {
            {-1, 0, 1},
            {-2, 0, 2},
            {-1, 0, 1}
            };
            int[,] verticalKernel = {
            {-1, -2, -1},
            { 0,  0,  0},
            { 1,  2,  1}
            };
            Bitmap result = ConvolutionFilter(image, horizontalKernel, verticalKernel);
            return result;
        }

        public Bitmap ApplyMeanFilter(Bitmap image, int kernelSize)
        {
            if (kernelSize % 2 == 0)
            {
                throw new ArgumentException("kernel tek sayı olmalıdır.");
            }

            Bitmap result = ConvolutionFilter(image, CreateMeanKernel(kernelSize));
            return result;
        }

        public Bitmap ApplyMedianFilter(Bitmap image, int kernelSize)
        {
            if (kernelSize % 2 == 0)
            {
                throw new ArgumentException("kernel size tek sayi olmalı.!");
            }

            Bitmap result = MedianFilterImage(image, kernelSize);
            return result;
        }

        public Bitmap ApplyContraharmonicMeanFilter(Bitmap image, int kernelSize, double Q)
        {
            if (kernelSize % 2 == 0)
            {
                throw new ArgumentException("kernel size tek sayi olmalı.!");
            }

            Bitmap result = ContraharmonicMeanFilterImage(image, kernelSize, Q);
            return result;
        }

        public Bitmap HistogramEsitle(Bitmap bmp)
        {
            int[] histogram = new int[256];
            int totalPixels = bmp.Width * bmp.Height;

            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    Color pixelColor = bmp.GetPixel(x, y);
                    int grayValue = (int)(pixelColor.R * 0.3 + pixelColor.G * 0.59 + pixelColor.B * 0.11);
                    histogram[grayValue]++;
                }
            }


            int[] cumulativeHistogram = new int[256];
            cumulativeHistogram[0] = histogram[0];
            for (int i = 1; i < 256; i++)
            {
                cumulativeHistogram[i] = cumulativeHistogram[i - 1] + histogram[i];
            }


            Bitmap equalizedBitmap = new Bitmap(bmp.Width, bmp.Height);
            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    Color pixelColor = bmp.GetPixel(x, y);
                    int grayValue = (int)(pixelColor.R * 0.3 + pixelColor.G * 0.59 + pixelColor.B * 0.11);


                    int newGrayValue = (int)(((double)cumulativeHistogram[grayValue] - 1) / totalPixels * 255);


                    Color newColor = Color.FromArgb(newGrayValue, newGrayValue, newGrayValue);


                    equalizedBitmap.SetPixel(x, y, newColor);
                }
            }

            return equalizedBitmap;
        }
        private int[] GetHistogram(Bitmap image)
        {
            int[] histogram = new int[256];

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    Color pixel = image.GetPixel(x, y);
                    int pixelValue = pixel.R;
                    histogram[pixelValue]++;
                }
            }

            return histogram;
        }
        public Bitmap Nicemleme(Bitmap bmp, double contrast)
        {
            int[] histogram = GetHistogram(bmp);

            int width = bmp.Width;
            int height = bmp.Height;
            Bitmap enhancedImage = new Bitmap(width, height);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color pixel = bmp.GetPixel(x, y);
                    int newPixelValue = (int)(contrast * (pixel.R - 128) + 128);

                    newPixelValue = Math.Max(0, Math.Min(255, newPixelValue));

                    Color newPixel = Color.FromArgb(newPixelValue, newPixelValue, newPixelValue);
                    enhancedImage.SetPixel(x, y, newPixel);
                }
            }

            return enhancedImage;
        }

        public Bitmap GenisletmeUygula(Bitmap sourceImage)
        {
            if (sourceImage == null)
                throw new ArgumentNullException(nameof(sourceImage));

            int width = sourceImage.Width;
            int height = sourceImage.Height;

            Bitmap resultImage = new Bitmap(width, height);

            for (int y = 1; y < height - 1; y++)
            {
                for (int x = 1; x < width - 1; x++)
                {

                    Color pixelColor = sourceImage.GetPixel(x, y);

                    if (pixelColor.R == 0)
                    {
                        bool hasWhiteNeighbor = BeyazKomsuKontrol(sourceImage, x, y);


                        if (hasWhiteNeighbor)
                            resultImage.SetPixel(x, y, Color.White);
                        else
                            resultImage.SetPixel(x, y, Color.Black);
                    }
                    else
                    {

                        resultImage.SetPixel(x, y, Color.White);
                    }
                }
            }

            return resultImage;
        }
        private bool BeyazKomsuKontrol(Bitmap sourceImage, int x, int y)
        {

            for (int ky = -1; ky <= 1; ky++)
            {
                for (int kx = -1; kx <= 1; kx++)
                {
                    int offsetX = x + kx;
                    int offsetY = y + ky;

                    Color neighborColor = sourceImage.GetPixel(offsetX, offsetY);

                    if (neighborColor.R == 255)
                        return true;
                }
            }

            return false;
        }

        public Bitmap ErozyonUygula(Bitmap sourceImage)
        {
            if (sourceImage == null)
                throw new ArgumentNullException(nameof(sourceImage));

            int width = sourceImage.Width;
            int height = sourceImage.Height;

            Bitmap resultImage = new Bitmap(width, height);

            for (int y = 1; y < height - 1; y++)
            {
                for (int x = 1; x < width - 1; x++)
                {
                    Color pixelColor = sourceImage.GetPixel(x, y);

                    if (pixelColor.R == 255)
                    {
                        bool hasBlackNeighbor = SiyahKomsuKontrol(sourceImage, x, y);

                        if (hasBlackNeighbor)
                            resultImage.SetPixel(x, y, Color.Black);
                        else
                            resultImage.SetPixel(x, y, Color.White);
                    }
                    else
                    {
                        resultImage.SetPixel(x, y, Color.Black);
                    }
                }
            }

            return resultImage;
        }
        private bool SiyahKomsuKontrol(Bitmap sourceImage, int x, int y)
        {
            for (int ky = -1; ky <= 1; ky++)
            {
                for (int kx = -1; kx <= 1; kx++)
                {
                    int offsetX = x + kx;
                    int offsetY = y + ky;

                    Color neighborColor = sourceImage.GetPixel(offsetX, offsetY);

                    if (neighborColor.R == 0)
                        return true;
                }
            }

            return false;
        }

        public Bitmap ApplySkeletonization(Bitmap sourceImage)
        {
            if (sourceImage == null)
                throw new ArgumentNullException(nameof(sourceImage));

            int width = sourceImage.Width;
            int height = sourceImage.Height;

            Bitmap resultImage = new Bitmap(width, height);
            sourceImage.copyTo(resultImage);

            bool hasChanged;

            do
            {
                hasChanged = false;

                Bitmap tempImage = new Bitmap(width, height);
                resultImage.copyTo(tempImage);

                for (int y = 1; y < height - 1; y++)
                {
                    for (int x = 1; x < width - 1; x++)
                    {
                        Color pixelColor = resultImage.GetPixel(x, y);

                        if (pixelColor.R == 0)
                        {
                            int[] neighbors = {
                            tempImage.GetPixel(x - 1, y - 1).R, tempImage.GetPixel(x, y - 1).R, tempImage.GetPixel(x + 1, y - 1).R,
                            tempImage.GetPixel(x - 1, y).R, tempImage.GetPixel(x + 1, y).R,
                            tempImage.GetPixel(x - 1, y + 1).R, tempImage.GetPixel(x, y + 1).R, tempImage.GetPixel(x + 1, y + 1).R
                        };

                            int numberOfBlackNeighbors = CountBlackNeighbors(neighbors);

                            if (2 <= numberOfBlackNeighbors && numberOfBlackNeighbors <= 6)
                            {
                                int a = (tempImage.GetPixel(x, y - 1).R == 0 && tempImage.GetPixel(x + 1, y).R == 255
                                    && tempImage.GetPixel(x + 1, y + 1).R == 0) ? 1 : 0;
                                int b = (tempImage.GetPixel(x + 1, y).R == 0 && tempImage.GetPixel(x, y + 1).R == 255
                                    && tempImage.GetPixel(x - 1, y + 1).R == 0) ? 1 : 0;

                                if (a + b == 1)
                                {
                                    resultImage.SetPixel(x, y, Color.White);
                                    hasChanged = true;
                                }
                            }
                        }
                    }
                }
            } while (hasChanged);

            return resultImage;
        }
        private int CountBlackNeighbors(int[] neighbors)
        {
            int count = 0;
            foreach (int neighbor in neighbors)
            {
                if (neighbor == 0)
                    count++;
            }
            return count;
        }
    }
}
