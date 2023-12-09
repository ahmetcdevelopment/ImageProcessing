using GoruntuIsleme.Enums;
using GoruntuIsleme.Services;
using System.Drawing.Imaging;
using static System.Windows.Forms.AxHost;

namespace GoruntuIsleme
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void cbx1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {

                Bitmap image = new Bitmap(pictureBox1.Image);
                IImageService _IS = new ImageService();
                if (cbx1.SelectedIndex == (int)IslemTypes.BirinciIslemMenu.GriSeviye)
                {
                    var gri = _IS.GriSeviyeDonustur(image);
                    pictureBox1.Image = gri;
                }
                if (cbx1.SelectedIndex == (int)IslemTypes.BirinciIslemMenu.SiyahBeyaz)
                {
                    var sb = _IS.ConvertToBlackWhite(image);
                    pictureBox1.Image = sb;
                }
                if (cbx1.SelectedIndex == (int)IslemTypes.BirinciIslemMenu.ZoomIn)
                {
                    pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;

                    var zoomedPicture = _IS.ZoomIn(image, 2.0);
                    pictureBox1.Image = zoomedPicture;
                }
                if (cbx1.SelectedIndex == (int)IslemTypes.BirinciIslemMenu.ZoomOut)
                {
                    pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;

                    var zoomOutPicture = _IS.ZoomOut(image, 2.0);
                    pictureBox1.Image = zoomOutPicture;
                }
                if (cbx1.SelectedIndex == (int)IslemTypes.BirinciIslemMenu.KesipAl)
                {
                    pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;

                    int startX = 381;
                    int startY = 24;
                    int width = 100;
                    int height = 100;
                    var croppedImage = _IS.KesipAl(pictureBox1, startX, startY, width, height);
                    pictureBox1.Image = croppedImage;
                }
            }
        }

        private void btnResimYukle_Click(object sender, EventArgs e)
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            openFileDialog1.ShowDialog();
            pictureBox1.ImageLocation = openFileDialog1.FileName;
            //if (pictureBox1.Image != null)
            //{
            //    _OrjImage = new Bitmap(pictureBox1.Image);

            //}
        }

        private void cbx2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {

                Bitmap image = new Bitmap(pictureBox1.Image);
                IImageService _IS = new ImageService();
                if (cbx1.SelectedIndex == (int)IslemTypes.IkinciIslemMenu.HistogramEsitle)
                {
                    var newImage = _IS.HistogramEsitle(image);
                    pictureBox1.Image = newImage;
                }
                if (cbx1.SelectedIndex == (int)IslemTypes.IkinciIslemMenu.Nicemleme)
                {
                    var newImage = _IS.Nicemleme(image, 1.5);
                    pictureBox1.Image = newImage;
                }
            }

        }

        private void cbx3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {

                Bitmap image = new Bitmap(pictureBox1.Image);
                IImageService _IS = new ImageService();
                if (cbx1.SelectedIndex == (int)IslemTypes.FiltreIslemMenu.Gauss)
                {
                    //sigma deðeri standart sapma deðeri
                    var Gaussian = _IS.ApplyGaussianBlur(image, 1.5);
                    pictureBox1.Image = Gaussian;
                }
                if (cbx1.SelectedIndex == (int)IslemTypes.FiltreIslemMenu.Keskinlestirme)
                {
                    double strength = 0.5;
                    var sharpedImage = _IS.ApplySharpenFilter(image, strength);
                    pictureBox1.Image = sharpedImage;
                }
                if (cbx1.SelectedIndex == (int)IslemTypes.FiltreIslemMenu.KenarBulma)
                {
                    var edgeDetectedImage = _IS.ApplyEdgeDetectionFilter(image);
                    pictureBox1.Image = edgeDetectedImage;
                }
                if (cbx1.SelectedIndex == (int)IslemTypes.FiltreIslemMenu.OrtalamaFiltresi)
                {
                    int kernelSize = 3;
                    var meanFilteredImage = _IS.ApplyMeanFilter(image, kernelSize);
                    pictureBox1.Image = meanFilteredImage;
                }
                if (cbx1.SelectedIndex == (int)IslemTypes.FiltreIslemMenu.OrtancaFiltresi)
                {
                    int kernelSize = 3;
                    var medianFilteredImage = _IS.ApplyMedianFilter(image, kernelSize);
                    pictureBox1.Image = medianFilteredImage;
                }
                if (cbx1.SelectedIndex == (int)IslemTypes.FiltreIslemMenu.KontraharmonikFiltre)
                {
                    int kernelSize = 3;
                    double Q = 1.5;
                    var contraharmonicFilteredImage = _IS.ApplyContraharmonicMeanFilter(image, kernelSize, Q);
                    pictureBox1.Image = contraharmonicFilteredImage;
                }
            }
        }

        private void cbx4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {

                Bitmap image = new Bitmap(pictureBox1.Image);
                IImageService _IS = new ImageService();
                if (cbx1.SelectedIndex == (int)IslemTypes.MorfolojikIslemMenu.Genisletme)
                {
                    var newImage = _IS.GenisletmeUygula(image);
                    pictureBox1.Image = newImage;
                }
                if (cbx1.SelectedIndex == (int)IslemTypes.MorfolojikIslemMenu.Erozyon)
                {
                    var newImage = _IS.ErozyonUygula(image);
                    pictureBox1.Image = newImage;
                }
                if (cbx1.SelectedIndex == (int)IslemTypes.MorfolojikIslemMenu.IskeletCikart)
                {
                    var newImage = _IS.ApplySkeletonization(image);
                    pictureBox1.Image = newImage;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "PNG Dosyasý|*.png|JPEG Dosyasý|*.jpg;*.jpeg|BMP Dosyasý|*.bmp";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string fileName = saveFileDialog.FileName;
                    ImageFormat format = ImageFormat.Png;

                    if (fileName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                        fileName.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase))
                    {
                        format = ImageFormat.Jpeg;
                    }
                    else if (fileName.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase))
                    {
                        format = ImageFormat.Bmp;
                    }
                    else if (fileName.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
                    {
                        format = ImageFormat.Gif;
                    }

                    pictureBox1.Image.Save(fileName, format);
                    MessageBox.Show("Resim baþarýyla kaydedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Lütfen önce bir resim seçin.", "Uyarý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
