using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoruntuIsleme.Services
{
    public interface IImageService
    {
        #region 1. İşlem Menüsü
        Bitmap GriSeviyeDonustur(Bitmap bmp);
        Bitmap ConvertToBlackWhite(Bitmap bmp);
        Bitmap ZoomIn(Bitmap bmp, double scaleFactor);
        Bitmap ZoomOut(Bitmap bmp, double scaleFactor);
        Bitmap KesipAl(PictureBox pictureBox, int startX, int startY, int width, int height);
        #endregion
        #region 2. İşlem Menüsü
        Bitmap HistogramEsitle(Bitmap bmp);
        Bitmap Nicemleme(Bitmap bmp, double contrast);
        #endregion
        #region Filtreleme Menüsü
        Bitmap ApplyGaussianBlur(Bitmap image, double sigma);
        Bitmap ApplySharpenFilter(Bitmap image, double strength);
        Bitmap ApplyEdgeDetectionFilter(Bitmap image);
        Bitmap ApplyMeanFilter(Bitmap image, int kernelSize);
        Bitmap ApplyMedianFilter(Bitmap image, int kernelSize);
        Bitmap ApplyContraharmonicMeanFilter(Bitmap image, int kernelSize, double Q);
        #endregion
        #region Morfolojik İşlemler Menüsü
        Bitmap GenisletmeUygula(Bitmap sourceImage);
        Bitmap ErozyonUygula(Bitmap sourceImage);
        Bitmap ApplySkeletonization(Bitmap sourceImage);
        #endregion
    }
}
