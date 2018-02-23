using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private Button buttonLoad;
        private PictureBox originalImage;
        private Bitmap originalBitmap;
        private PictureBox filterImage;
        private Bitmap filterBitmap;

        private Button buttonInverse;
        private Button buttonBrightness;
        private Button buttonContrast;
        private Button buttonConvolution;
        private Button buttonBlur;

        private ComboBox kernelSizeComboBox;

        public Form1()
        {
            buttonLoad = new Button();
            buttonLoad.Text = "Load";
            buttonLoad.Left = 3;
            buttonLoad.Top = 3;
            buttonLoad.Click += new EventHandler(this.LoadOnClick);

            buttonInverse = new Button();
            buttonInverse.Text = "Inverse";
            buttonInverse.Left = 3;
            buttonInverse.Top = 3 + buttonLoad.Height;
            buttonInverse.Click += new EventHandler(this.InvertOnClick);

            buttonBrightness = new Button();
            buttonBrightness.Text = "Brightness";
            buttonBrightness.Left = 3;
            buttonBrightness.Top = 3 + 2 * buttonLoad.Height;
            buttonBrightness.Click += new EventHandler(this.BrightnessCorrectionOnClick);

            buttonContrast = new Button();
            buttonContrast.Text = "Contrast";
            buttonContrast.Left = 3;
            buttonContrast.Top = 3 + 3 * buttonLoad.Height;
            buttonContrast.Click += new EventHandler(this.ContrastEnhancementOnClick);

            buttonConvolution = new Button();
            buttonConvolution.Text = "Convolution";
            buttonConvolution.Left = 3 + buttonLoad.Width;
            buttonConvolution.Top = 3;
            buttonConvolution.Click += new EventHandler(this.ConvolutionOnClick);

            buttonBlur = new Button();
            buttonBlur.Text = "Blur";
            buttonBlur.Left = 3 + buttonLoad.Width;
            buttonBlur.Top = 3 + buttonLoad.Height;
            buttonBlur.Click += new EventHandler(this.BlurOnClick);

            originalImage = new PictureBox();
            originalImage.BorderStyle = BorderStyle.Fixed3D;
            originalImage.Width = this.Width * 2;
            originalImage.Height = this.Height * 2;
            originalImage.Left = 3;
            originalImage.Top = 5 * buttonLoad.Height;
            originalImage.SizeMode = PictureBoxSizeMode.StretchImage;

            filterImage = new PictureBox();
            filterImage.BorderStyle = BorderStyle.Fixed3D;
            filterImage.Width = this.Width * 2;
            filterImage.Height = this.Height * 2;
            filterImage.Left = 3 + originalImage.Width;
            filterImage.Top = 5 * buttonLoad.Height;
            filterImage.SizeMode = PictureBoxSizeMode.StretchImage;

            kernelSizeComboBox = new ComboBox();
            kernelSizeComboBox.Text = "Kernel Size";
            kernelSizeComboBox.Left = 3 + 2*buttonLoad.Width;
            kernelSizeComboBox.Top = 3;
            kernelSizeComboBox.Items.AddRange(new object[] { "3x3", "5x5", "7x7", "9x9", "11x11", "13x13", "15x15" });

            this.Controls.Add(buttonLoad);
            this.Controls.Add(originalImage);
            this.Controls.Add(buttonInverse);
            this.Controls.Add(filterImage);
            this.Controls.Add(buttonBrightness);
            this.Controls.Add(buttonContrast);
            this.Controls.Add(buttonConvolution);
            this.Controls.Add(buttonBlur);
            this.Controls.Add(kernelSizeComboBox);

            InitializeComponent();
        }

        protected void LoadOnClick(object sender, EventArgs e)
        {
            OpenFileDialog imageDialog = new OpenFileDialog();

            imageDialog.Title = "Original Image";
            imageDialog.Filter = "jpg files (*.jpg)|*.jpg|All files (*.*)|*.*";

            if (imageDialog.ShowDialog() == DialogResult.OK)
            {
                originalBitmap = new Bitmap(imageDialog.OpenFile());
                originalImage.Image = originalBitmap;
            }

            imageDialog.Dispose();
        }

        protected void InvertOnClick(object sender, EventArgs e)
        {
            filterBitmap = SetInvert(originalBitmap);
            filterImage.Image = filterBitmap;
        }

        public Bitmap SetInvert(Bitmap orgBitmap)
        {
            Bitmap tempBitmap = (Bitmap)orgBitmap;
            Bitmap bitmap = (Bitmap)tempBitmap.Clone();
            Color c;
            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    c = bitmap.GetPixel(i, j);
                    bitmap.SetPixel(i, j, Color.FromArgb(255 - c.R, 255 - c.G, 255 - c.B));
                }
            }
            return (Bitmap)bitmap.Clone();
        }

        protected void BrightnessCorrectionOnClick(object sender, EventArgs e)
        {
            filterBitmap = SetBrightness(-100, originalBitmap);
            filterImage.Image = filterBitmap;
        }

        public Bitmap SetBrightness(int brightness, Bitmap orgBitmap)
        {
            Bitmap tempBitmap = (Bitmap)orgBitmap;
            Bitmap bitmap = (Bitmap)tempBitmap.Clone();
            Color color;

            if (brightness < -255) brightness = -255;
            if (brightness > 255) brightness = 255;

            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    color = bitmap.GetPixel(i, j);
                    int colorR = color.R + brightness;
                    int colorG = color.G + brightness;
                    int colorB = color.B + brightness;

                    if (colorR < 0) colorR = 1;
                    if (colorR > 255) colorR = 255;

                    if (colorG < 0) colorG = 1;
                    if (colorG > 255) colorG = 255;

                    if (colorB < 0) colorB = 1;
                    if (colorB > 255) colorB = 255;

                    bitmap.SetPixel(i, j, Color.FromArgb(colorR, colorG, colorB));
                }
            }

            return (Bitmap)bitmap.Clone();
        }

        protected void ContrastEnhancementOnClick(object sender, EventArgs e)
        {
            filterBitmap = SetContrast(100f, originalBitmap);
            filterImage.Image = filterBitmap;
        }

        public Bitmap SetContrast(double contrast, Bitmap orgBitmap)
        {
            Bitmap tempBitmap = (Bitmap)orgBitmap;
            Bitmap bitmap = (Bitmap)tempBitmap.Clone();
            Color color;

            if (contrast < -100) contrast = -100;
            if (contrast > 100) contrast = 100;

            contrast = (100 + contrast) / 100;
            contrast *= contrast;

            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    color = bitmap.GetPixel(i, j);
                    double colorR = color.R / 255.0f;
                    double colorG = color.G / 255.0f;
                    double colorB = color.B / 255.0f;

                    colorR = (((colorR - 0.5f) * contrast) + 0.5f) * 255.0f;
                    colorG = (((colorG - 0.5f) * contrast) + 0.5f) * 255.0f;
                    colorB = (((colorB - 0.5f) * contrast) + 0.5f) * 255.0f;

                    if (colorR < 0) colorR = 0;
                    if (colorR > 255) colorR = 255;

                    if (colorG < 0) colorG = 0;
                    if (colorG > 255) colorG = 255;

                    if (colorB < 0) colorB = 0;
                    if (colorB > 255) colorB = 255;

                    bitmap.SetPixel(i, j, Color.FromArgb((byte)colorR, (byte)colorG, (byte)colorB));
                }
            }
            return (Bitmap)bitmap.Clone();
        }

        public Bitmap SetGrayscale(Bitmap orgBitmap)
        {
            Bitmap tempBitmap = (Bitmap)orgBitmap;
            Bitmap bitmap = (Bitmap)tempBitmap.Clone();
            Color c;
            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    c = bitmap.GetPixel(i, j);
                    byte gray = (byte)(.299 * c.R + .587 * c.G + .114 * c.B);

                    bitmap.SetPixel(i, j, Color.FromArgb(gray, gray, gray));
                }
            }
            return (Bitmap)bitmap.Clone();
        }

        protected void BlurOnClick(object sender, EventArgs e)
        {
            int[,] kernel = new int[,] { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };
            int selectedKernelSize = (kernelSizeComboBox.SelectedIndex + 1) * 2 + 1;
            filterBitmap = SetBlur(selectedKernelSize, originalBitmap);
            filterImage.Image = filterBitmap;
        }

        public Bitmap SetBlur(int matrixSize, Bitmap orgBitmap)
        {
            Bitmap tempBitmap = (Bitmap)orgBitmap;
            Bitmap bitmap = (Bitmap)tempBitmap.Clone();
            int kernel = matrixSize / 2;
            int sumR = 0, sumG = 0, sumB = 0;
            int avgR = 0, avgG = 0, avgB = 0;
            Color c;

            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    for (int k = i - kernel; k <= i + kernel; k++)
                    {
                        for (int n = j - kernel; n <= j + kernel; n++)
                        {
                            if (k >= 0 && n >= 0 && k < bitmap.Width && n < bitmap.Height)
                            {
                                c = bitmap.GetPixel(k, n);
                                sumR += c.R;
                                sumG += c.G;
                                sumB += c.B;
                            }
                        }
                    }

                    avgR = (int)(sumR) / (matrixSize*matrixSize);
                    avgG = (int)(sumG) / (matrixSize * matrixSize);
                    avgB = (int)(sumB) / (matrixSize * matrixSize);

                    if (avgR < 0) avgR = 0;
                    if (avgR > 255) avgR = 255;

                    if (avgG < 0) avgG = 0;
                    if (avgG > 255) avgG = 255;

                    if (avgB < 0) avgB = 0;
                    if (avgB > 255) avgB = 255;

                    bitmap.SetPixel(i, j, Color.FromArgb((byte)avgR, (byte)avgG, (byte)avgB));
                    sumR = sumG = sumB = avgR = avgG = avgB = 0;
                }
            }
            return (Bitmap)bitmap.Clone();
        }

        protected void ConvolutionOnClick(object sender, EventArgs e)
        {
            //int[,] kernel = new int[,] { { 0, 1, 0 }, { 1, 4, 1 }, { 0, 1, 0 } };
            //int[,] kernel = new int[,] { { 0, -1, 0 }, { -1, 5, -1 }, { 0, -1, 0 } };
            int[,] kernel = new int[,] { { -1, -1, -1 }, { -1, 9, -1 }, { -1, -1, -1 } };
            int selectedKernelSize = (kernelSizeComboBox.SelectedIndex + 1) * 2 + 1;
            filterBitmap = Convolution(selectedKernelSize, kernel, selectedKernelSize*selectedKernelSize, 0, originalBitmap);
            filterImage.Image = filterBitmap;
        }

        public Bitmap Convolution(int kernelSize, int[,] kernelMatrix, int divisor, int offset, Bitmap orgBitmap)
        {
            Bitmap tempBitmap = (Bitmap)orgBitmap;
            Bitmap bitmap = (Bitmap)tempBitmap.Clone();
            int kernel = kernelSize / 2;
            int sumR = 0, sumG = 0, sumB = 0;
            int avgR = 0, avgG = 0, avgB = 0;
            Color c;

            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    for (int k = i - kernel; k <= i + kernel; k++)
                    {
                        for (int n = j - kernel; n <= j + kernel; n++)
                        {
                            if (k >= 0 && n >= 0 && k < bitmap.Width && n < bitmap.Height)
                            {
                                c = bitmap.GetPixel(k, n);
                                sumR += c.R * kernelMatrix[k - i + kernel, n - j + kernel];
                                sumG += c.G * kernelMatrix[k - i + kernel, n - j + kernel];
                                sumB += c.B * kernelMatrix[k - i + kernel, n - j + kernel];
                            }
                        }
                    }

                    avgR = (int)(sumR) / divisor + offset;
                    avgG = (int)(sumG) / divisor + offset;
                    avgB = (int)(sumB) / divisor + offset;

                    if (avgR < 0) avgR = 0;
                    if (avgR > 255) avgR = 255;

                    if (avgG < 0) avgG = 0;
                    if (avgG > 255) avgG = 255;

                    if (avgB < 0) avgB = 0;
                    if (avgB > 255) avgB = 255;

                    bitmap.SetPixel(i, j, Color.FromArgb((byte)avgR, (byte)avgG, (byte)avgB));
                    sumR = sumG = sumB = avgR = avgG = avgB = 0;
                }
            }
            return (Bitmap)bitmap.Clone();
        }

        //public Bitmap SetBlur(int matrixSize, Bitmap orgBitmap)
        //{
        //    Bitmap tempBitmap = (Bitmap)orgBitmap;
        //    Bitmap bitmap = (Bitmap)tempBitmap.Clone();
        //    Color c;
        //    int intensitySum = 0;

        //    for (int x = 0; x < bitmap.Width; x++)
        //    {
        //        for (int y = 0; y < bitmap.Height; y++)
        //        {                   
        //            for (int i = 0; i < matrixSize; i++)
        //            {
        //                for (int j = 0; j < matrixSize; j++)
        //                {
        //                    if (x + i >= 0 && y + j >= 0 && x + i < bitmap.Width && y + j < bitmap.Height)
        //                    {
        //                        c = bitmap.GetPixel(x + i, y + j);
        //                        int intensity = (c.R + c.G + c.B) / 3;
        //                        intensitySum += intensity;
        //                    }
        //                }
        //            }

        //            bitmap.SetPixel(x, y, Color.FromArgb(intensitySum / 9));
        //            intensitySum = 0;
        //        }
        //    }

        //    return (Bitmap)bitmap.Clone();
        //}

        //public Bitmap SetBlur(int kernelSize, int[,] kernelMatrix, int divisor, int offset, Bitmap orgBitmap)
        //{
        //    Bitmap tempBitmap = (Bitmap)orgBitmap;
        //    Bitmap bitmap = (Bitmap)tempBitmap.Clone();
        //    bitmap = SetGrayscale(bitmap);
        //    int intensitySum = 0;
        //    Color c;

        //    for (int x = 0; x < bitmap.Width; x++)
        //    {
        //        for (int y = 0; y < bitmap.Height; y++)
        //        {
        //            for (int i = 0; i < kernelSize; i++)
        //            {
        //                for (int j = 0; j < kernelSize; j++)
        //                {
        //                    if (x + i >= 0 && y + j >= 0 && x + i < bitmap.Width && y + j < bitmap.Height)
        //                    {
        //                        c = bitmap.GetPixel(x + i, y + j);
        //                        int intensity = (c.R + c.G + c.B) / 3;
        //                        intensitySum += intensity * kernelMatrix[i, j];
        //                    }
        //                }
        //            }

        //            bitmap.SetPixel(x, y, Color.FromArgb(offset + intensitySum / divisor));
        //            intensitySum = 0;
        //        }
        //    }

        //    return (Bitmap)bitmap.Clone();
        //}

        //DZIALAJACE
        //public Bitmap SetBlur(int matrixSize, Bitmap orgBitmap)
        //{
        //    Bitmap tempBitmap = (Bitmap)orgBitmap;
        //    Bitmap bitmap = (Bitmap)tempBitmap.Clone();
        //    int kernel = matrixSize / 2;
        //    int sumR = 0, sumG = 0, sumB = 0;
        //    int avgR = 0, avgG = 0, avgB = 0;
        //    Color c;

        //    for (int i = 0; i < bitmap.Width; i++)
        //    {
        //        for (int j = 0; j < bitmap.Height; j++)
        //        {
        //            for (int k = i - kernel; k <= i + kernel; k++)
        //            {
        //                for (int n = j - kernel; n <= j + kernel; n++)
        //                {
        //                    if (k >= 0 && n >= 0 && k < bitmap.Width && n < bitmap.Height)
        //                    {
        //                        c = bitmap.GetPixel(k, n);
        //                        sumR += c.R;
        //                        sumG += c.G;
        //                        sumB += c.B;
        //                    }
        //                }
        //            }

        //            avgR = (int)(sumR) / 9;
        //            avgG = (int)(sumG) / 9;
        //            avgB = (int)(sumB) / 9;

        //            if (avgR < 0) avgR = 0;
        //            if (avgR > 255) avgR = 255;

        //            if (avgG < 0) avgG = 0;
        //            if (avgG > 255) avgG = 255;

        //            if (avgB < 0) avgB = 0;
        //            if (avgB > 255) avgB = 255;

        //            bitmap.SetPixel(i, j, Color.FromArgb((byte)avgR, (byte)avgG, (byte)avgB));
        //            sumR = sumG = sumB = avgR = avgG = avgB = 0;
        //        }
        //    }

        //    //for (int i = 1; i < bitmap.Width - 1; i++)
        //    //{
        //    //    for (int j = 1; j < bitmap.Height - 1; j++)
        //    //    {
        //    //        Color c1 = bitmap.GetPixel(i - 1, j - 1);
        //    //        Color c2 = bitmap.GetPixel(i - 1, j);
        //    //        Color c3 = bitmap.GetPixel(i - 1, j + 1);
        //    //        Color c4 = bitmap.GetPixel(i, j - 1);
        //    //        Color c5 = bitmap.GetPixel(i, j);
        //    //        Color c6 = bitmap.GetPixel(i, j + 1);
        //    //        Color c7 = bitmap.GetPixel(i + 1, j - 1);
        //    //        Color c8 = bitmap.GetPixel(i + 1, j);
        //    //        Color c9 = bitmap.GetPixel(i + 1, j + 1);

        //    //        int avgR = (int)(c1.R + c2.R + c3.R + c4.R + c5.R + c6.R + c7.R + c8.R + c9.R) / 9;
        //    //        int avgG = (int)(c1.G + c2.G + c3.G + c4.G + c5.G + c6.G + c7.G + c8.G + c9.G) / 9;
        //    //        int avgB = (int)(c1.B + c2.B + c3.B + c4.B + c5.B + c6.B + c7.B + c8.B + c9.B) / 9;

        //    //        if (avgR < 0) avgR = 0;
        //    //        if (avgR > 255) avgR = 255;

        //    //        if (avgG < 0) avgG = 0;
        //    //        if (avgG > 255) avgG = 255;

        //    //        if (avgB < 0) avgB = 0;
        //    //        if (avgB > 255) avgB = 255;

        //    //        bitmap.SetPixel(i, j, Color.FromArgb((byte)avgR, (byte)avgG, (byte)avgB));
        //    //    }
        //    //}
        //    return (Bitmap)bitmap.Clone();
        //}
    }
}
