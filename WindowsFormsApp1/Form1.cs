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
        private Button buttonBlur;
        private Button buttonGaussianSmoothing;
        private Button buttonSharpen;
        private Button buttonEdgeDetect;
        private Button buttonEmboss;

        private Button buttonKernelReady;
        private TextBox kernelOneDimensionTextbox;
        private TextBox kernelSecondDimensionTextbox;
        private TextBox pixelRowTextbox;
        private TextBox pixelColumnTextbox;
        private DataGridView kernelTable = new DataGridView();
        private TextBox divisorTextbox;
        private TextBox offsetTextbox;
        private Label divisorLabel;
        private Label kernelSizeLabel;
        private Label offsetLabel;
        private Label pixelLabel;
        
        private int[,] kernel;
        private int kernelRows = 1;
        private int kernelColumns = 1;
        private int pixelRow = 0;
        private int pixelColumn = 0;
        private int divisor = 1;
        private int offset = 0;

        public Form1()
        {
            buttonLoad = new Button();
            buttonLoad.Text = "Load";
            buttonLoad.Left = 3;
            buttonLoad.Top = 3;
            buttonLoad.Width = 150;
            buttonLoad.Click += new EventHandler(this.LoadOnClick);

            buttonInverse = new Button();
            buttonInverse.Text = "Inverse";
            buttonInverse.Left = 3;
            buttonInverse.Top = 3 + buttonLoad.Height;
            buttonInverse.Width = 150;
            buttonInverse.Click += new EventHandler(this.InvertOnClick);

            buttonBrightness = new Button();
            buttonBrightness.Text = "Brightness";
            buttonBrightness.Left = 3;
            buttonBrightness.Top = 3 + 2 * buttonLoad.Height;
            buttonBrightness.Width = 150;
            buttonBrightness.Click += new EventHandler(this.BrightnessCorrectionOnClick);

            buttonContrast = new Button();
            buttonContrast.Text = "Contrast";
            buttonContrast.Left = 3;
            buttonContrast.Top = 3 + 3 * buttonLoad.Height;
            buttonContrast.Width = 150;
            buttonContrast.Click += new EventHandler(this.ContrastEnhancementOnClick);

            buttonBlur = new Button();
            buttonBlur.Text = "Blur";
            buttonBlur.Left = 3;
            buttonBlur.Top = 3 + 4*buttonLoad.Height;
            buttonBlur.Width = 150;
            buttonBlur.Click += new EventHandler(this.BlurOnClick);

            buttonGaussianSmoothing = new Button();
            buttonGaussianSmoothing.Text = "Gaussian Smoothing";
            buttonGaussianSmoothing.Left = 3;
            buttonGaussianSmoothing.Top = 3 + 5*buttonLoad.Height;
            buttonGaussianSmoothing.Width = 150;
            buttonGaussianSmoothing.Click += new EventHandler(this.GaussianSmoothingOnClick);

            buttonSharpen = new Button();
            buttonSharpen.Text = "Sharpen";
            buttonSharpen.Left = 3;
            buttonSharpen.Top = 3 + 6 * buttonLoad.Height;
            buttonSharpen.Width = 150;
            buttonSharpen.Click += new EventHandler(this.SharpenOnClick);

            buttonEdgeDetect = new Button();
            buttonEdgeDetect.Text = "Edge detection";
            buttonEdgeDetect.Left = 3;
            buttonEdgeDetect.Top = 3 + 7 * buttonLoad.Height;
            buttonEdgeDetect.Width = 150;
            buttonEdgeDetect.Click += new EventHandler(this.EdgeDetectOnClick);

            buttonEmboss = new Button();
            buttonEmboss.Text = "Emboss";
            buttonEmboss.Left = 3;
            buttonEmboss.Top = 3 + 8 * buttonLoad.Height;
            buttonEmboss.Width = 150;
            buttonEmboss.Click += new EventHandler(this.EmbossOnClick);

            buttonKernelReady = new Button();
            buttonKernelReady.Text = "Ready";
            buttonKernelReady.Left = 3 + 3 * buttonLoad.Width;
            buttonKernelReady.Top = 3 + 5 * buttonLoad.Height;
            buttonKernelReady.Width = 150;
            buttonKernelReady.Click += new EventHandler(this.KernelReady);

            originalImage = new PictureBox();
            originalImage.BorderStyle = BorderStyle.Fixed3D;
            originalImage.Width = this.Width * 2;
            originalImage.Height = this.Height * 2;
            originalImage.Left = 3;
            originalImage.Top = 10 * buttonLoad.Height;
            originalImage.SizeMode = PictureBoxSizeMode.StretchImage;

            filterImage = new PictureBox();
            filterImage.BorderStyle = BorderStyle.Fixed3D;
            filterImage.Width = this.Width * 2;
            filterImage.Height = this.Height * 2;
            filterImage.Left = 3 + originalImage.Width;
            filterImage.Top = 10 * buttonLoad.Height;
            filterImage.SizeMode = PictureBoxSizeMode.StretchImage;

            kernelOneDimensionTextbox = new TextBox();
            kernelOneDimensionTextbox.Text = "1";
            kernelOneDimensionTextbox.Left = 3 + 3 * buttonLoad.Width;
            kernelOneDimensionTextbox.Top = 3;
            kernelOneDimensionTextbox.Width = 50;
            kernelOneDimensionTextbox.TextChanged += new EventHandler(this.KernelSizeChanged);

            kernelSecondDimensionTextbox = new TextBox();
            kernelSecondDimensionTextbox.Text = "1";
            kernelSecondDimensionTextbox.Left = 3 + 3 * buttonLoad.Width + 50;
            kernelSecondDimensionTextbox.Top = 3;
            kernelSecondDimensionTextbox.Width = 50;
            kernelSecondDimensionTextbox.TextChanged += new EventHandler(this.KernelSizeChanged);

            pixelRowTextbox = new TextBox();
            pixelRowTextbox.Text = "0";
            pixelRowTextbox.Left = 3 + 3 * buttonLoad.Width;
            pixelRowTextbox.Top = 3 + 3 * buttonLoad.Height;
            pixelRowTextbox.Width = 50;
            pixelRowTextbox.TextChanged += new EventHandler(this.PixelChanged);

            pixelColumnTextbox = new TextBox();
            pixelColumnTextbox.Text = "0";
            pixelColumnTextbox.Left = 3 + 3 * buttonLoad.Width + 50;
            pixelColumnTextbox.Top = 3 + 3 * buttonLoad.Height;
            pixelColumnTextbox.Width = 50;
            pixelColumnTextbox.TextChanged += new EventHandler(this.PixelChanged);

            divisorTextbox = new TextBox();
            divisorTextbox.Left = 3 + 3 * buttonLoad.Width;
            divisorTextbox.Top = 3 + buttonLoad.Height;

            offsetTextbox = new TextBox();
            offsetTextbox.Left = 3 + 3 * buttonLoad.Width;
            offsetTextbox.Top = 3 + 2 * buttonLoad.Height;

            divisorLabel = new Label();
            divisorLabel.Text = "Divisor:";
            divisorLabel.Left = 98 + 2 * buttonLoad.Width;
            divisorLabel.Top = 3 + buttonLoad.Height;

            offsetLabel = new Label();
            offsetLabel.Text = "Offset:";
            offsetLabel.Left = 103 + 2 * buttonLoad.Width;
            offsetLabel.Top = 3 + 2 * buttonLoad.Height;

            pixelLabel = new Label();
            pixelLabel.Text = "Center pixel:";
            pixelLabel.Left = 50 + 2 * buttonLoad.Width;
            pixelLabel.Top = 3 + 3 * buttonLoad.Height;

            kernelSizeLabel = new Label();
            kernelSizeLabel.Text = "Kernel size:";
            kernelSizeLabel.Left = 53 + 2 * buttonLoad.Width;
            kernelSizeLabel.Top = 3;

            this.Controls.Add(buttonLoad);
            this.Controls.Add(originalImage);
            this.Controls.Add(buttonInverse);
            this.Controls.Add(filterImage);
            this.Controls.Add(buttonBrightness);
            this.Controls.Add(buttonContrast);
            this.Controls.Add(buttonBlur);
            this.Controls.Add(buttonGaussianSmoothing);
            this.Controls.Add(buttonSharpen);
            this.Controls.Add(buttonEdgeDetect);
            this.Controls.Add(buttonEmboss);
            this.Controls.Add(buttonKernelReady);
            this.Controls.Add(divisorTextbox);
            this.Controls.Add(offsetTextbox);
            this.Controls.Add(divisorLabel);
            this.Controls.Add(offsetLabel);
            this.Controls.Add(kernelSizeLabel);
            this.Controls.Add(kernelOneDimensionTextbox);
            this.Controls.Add(kernelSecondDimensionTextbox);
            this.Controls.Add(pixelRowTextbox);
            this.Controls.Add(pixelColumnTextbox);
            this.Controls.Add(pixelLabel);

            InitializeComponent();
        }

        private void PixelChanged(object sender, EventArgs e)
        {
            for(int i = 0; i < kernelRows; i++)
            {
                for(int j = 0; j < kernelColumns; j++)
                {
                    kernelTable.Rows[i].Cells[j].Style.BackColor = Color.White;
                }
            }
            try
            {
                pixelRow = Int32.Parse(pixelColumnTextbox.Text);
                pixelColumn = Int32.Parse(pixelRowTextbox.Text);
            }
            catch { }
            kernelTable[pixelColumn, pixelRow].Style.BackColor = Color.Green;
        }

        private void KernelReady(object sender, EventArgs e)
        {
            foreach (DataGridViewRow rw in kernelTable.Rows)
            {
                for (int i = 0; i < rw.Cells.Count; i++)
                {
                    if (rw.Cells[i].Value == null || rw.Cells[i].Value == DBNull.Value || String.IsNullOrWhiteSpace(rw.Cells[i].Value.ToString()))
                    {
                        MessageBox.Show("Fill all the cells in the kernel", "Kernel", MessageBoxButtons.OK);
                        return;
                    }
                }
            }
            kernel = new int[kernelRows, kernelColumns];

            for (int i = 0; i < kernelRows; i++)
            {
                for ( int j = 0; j < kernelColumns; j++)
                {
                    string k = kernelTable.Rows[i].Cells[j].Value.ToString();
                    kernel[i, j] = Int32.Parse(k);
                }
            }
            try
            {
                divisor = Int32.Parse(divisorTextbox.Text);
                offset = Int32.Parse(offsetTextbox.Text);
            }
            catch { }
            filterBitmap = Convolution(kernelRows, kernelColumns, pixelRow, pixelColumn, kernel, divisor, offset, originalBitmap);
            filterImage.Image = filterBitmap;
        }

        private void KernelSizeChanged(object sender, EventArgs e)
        {
            try
            {
                kernelRows = Int32.Parse(kernelOneDimensionTextbox.Text);
                kernelColumns = Int32.Parse(kernelSecondDimensionTextbox.Text);
            }
            catch { }
            kernelTable.RowCount = kernelRows;
            kernelTable.ColumnCount = kernelColumns;
            
            kernelTable.Top = 3;
            kernelTable.Left = 3 + 5 * buttonLoad.Width;
            kernelTable.RowHeadersVisible = false;
            kernelTable.ColumnHeadersVisible = false;
            kernelTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            kernelTable.AllowUserToAddRows = false;
            kernelTable[0, 0].Style.BackColor = Color.Green;
            
            this.Controls.Add(kernelTable);
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
            int[,] matrix = new int[3, 3] { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };
            filterBitmap = Convolution(3, 3, 1, 1, matrix, 9, 0, originalBitmap);
            filterImage.Image = filterBitmap;
        }

        //public Bitmap SetBlur(Bitmap orgBitmap)
        //{
        //    Bitmap tempBitmap = (Bitmap)orgBitmap;
        //    Bitmap bitmap = (Bitmap)tempBitmap.Clone();
        //    int sumR = 0, sumG = 0, sumB = 0;
        //    int avgR = 0, avgG = 0, avgB = 0;
        //    Color c;

        //    for (int i = 0; i < bitmap.Width; i++)
        //    {
        //        for (int j = 0; j < bitmap.Height; j++)
        //        {
        //            for (int k = i - 1; k <= i + 1; k++)
        //            {
        //                for (int n = j - 1; n <= j + 1; n++)
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
        //    return (Bitmap)bitmap.Clone();
        //}

        protected void GaussianSmoothingOnClick(object sender, EventArgs e)
        {
            int[,] matrix = new int[3, 3] { { 0, 1, 0 }, { 1, 4, 1 }, { 0, 1, 0 } };
            filterBitmap = Convolution(3, 3, 1, 1, matrix, 8, 0, originalBitmap);
            filterImage.Image = filterBitmap;
        }

        //public Bitmap SetGaussianSmoothing(Bitmap orgBitmap)
        //{
        //    Bitmap tempBitmap = (Bitmap)orgBitmap;
        //    Bitmap bitmap = (Bitmap)tempBitmap.Clone();
        //    int sumR = 0, sumG = 0, sumB = 0;
        //    int avgR = 0, avgG = 0, avgB = 0;
        //    Color c;

        //    for (int i = 0; i < bitmap.Width; i++)
        //    {
        //        for (int j = 0; j < bitmap.Height; j++)
        //        {
        //            c = bitmap.GetPixel(i, j);

        //            sumR = (int)c.R * 4;
        //            sumG = (int)c.G * 4;
        //            sumB = (int)c.B * 4;

        //            if((i - 1) >= 0)
        //            {
        //                sumR += (int)bitmap.GetPixel(i - 1, j).R;
        //                sumG += (int)bitmap.GetPixel(i - 1, j).G;
        //                sumB += (int)bitmap.GetPixel(i - 1, j).B;
        //            }
        //            if ((i + 1) < bitmap.Width)
        //            {
        //                sumR += (int)bitmap.GetPixel(i + 1, j).R;
        //                sumG += (int)bitmap.GetPixel(i + 1, j).G;
        //                sumB += (int)bitmap.GetPixel(i + 1, j).B;
        //            }
        //            if ((j - 1) >= 0)
        //            {
        //                sumR += (int)bitmap.GetPixel(i, j - 1).R;
        //                sumG += (int)bitmap.GetPixel(i, j - 1).G;
        //                sumB += (int)bitmap.GetPixel(i, j - 1).B;
        //            }
        //            if ((j + 1) < bitmap.Height)
        //            {
        //                sumR += (int)bitmap.GetPixel(i, j + 1).R;
        //                sumG += (int)bitmap.GetPixel(i, j + 1).G;
        //                sumB += (int)bitmap.GetPixel(i, j + 1).B;
        //            }

        //            avgR = (int)(sumR) / 8;
        //            avgG = (int)(sumG) / 8;
        //            avgB = (int)(sumB) / 8;

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
        //    return (Bitmap)bitmap.Clone();
        //}

        protected void SharpenOnClick(object sender, EventArgs e)
        {
            int[,] matrix = new int[3, 3] { { 0, -1, 0 }, { -1, 5, -1 }, { 0, -1, 0 } };
            filterBitmap = Convolution(3, 3, 1, 1, matrix, 1, 0, originalBitmap); 
            filterImage.Image = filterBitmap;
        }

        //public Bitmap SetSharpen(Bitmap orgBitmap)
        //{
        //    Bitmap bitmap = (Bitmap)orgBitmap.Clone();
        //    int sumR = 0, sumG = 0, sumB = 0;
        //    int avgR = 0, avgG = 0, avgB = 0;
        //    Color c;

        //    for (int i = 0; i < bitmap.Width; i++)
        //    {
        //        for (int j = 0; j < bitmap.Height; j++)
        //        {
        //            c = bitmap.GetPixel(i, j);

        //            sumR = (int)c.R * 5;
        //            sumG = (int)c.G * 5;
        //            sumB = (int)c.B * 5;

        //            if ((i - 1) >= 0)
        //            {
        //                sumR -= (int)(bitmap.GetPixel(i - 1, j).R);
        //                sumG -= (int)(bitmap.GetPixel(i - 1, j).G);
        //                sumB -= (int)(bitmap.GetPixel(i - 1, j).B);
        //            }
        //            if ((i + 1) < bitmap.Width)
        //            {
        //                sumR -= (int)(bitmap.GetPixel(i + 1, j).R);
        //                sumG -= (int)(bitmap.GetPixel(i + 1, j).G);
        //                sumB -= (int)(bitmap.GetPixel(i + 1, j).B);
        //            }
        //            if ((j - 1) >= 0)
        //            {
        //                sumR -= (int)(bitmap.GetPixel(i, j - 1).R);
        //                sumG -= (int)(bitmap.GetPixel(i, j - 1).G);
        //                sumB -= (int)(bitmap.GetPixel(i, j - 1).B);
        //            }
        //            if ((j + 1) < bitmap.Height)
        //            {
        //                sumR -= (int)(bitmap.GetPixel(i, j + 1).R);
        //                sumG -= (int)(bitmap.GetPixel(i, j + 1).G);
        //                sumB -= (int)(bitmap.GetPixel(i, j + 1).B);
        //            }

        //            avgR = (int)(sumR);
        //            avgG = (int)(sumG);
        //            avgB = (int)(sumB);

        //            if (avgR < 0) { avgR = 0; }
        //            if (avgR > 255) { avgR = 255; }

        //            if (avgG < 0) { avgG = 0; }
        //            if (avgG > 255) { avgG = 255;}

        //            if (avgB < 0) { avgB = 0; }
        //            if (avgB > 255) { avgB = 255; }

        //            bitmap.SetPixel(i, j, Color.FromArgb(avgR, avgG, avgB));
        //            sumR = sumG = sumB = avgR = avgG = avgB = 0;
        //        }
        //    }
        //    return (Bitmap)bitmap.Clone();
        //}

        protected void EdgeDetectOnClick(object sender, EventArgs e)
        {
            int[,] matrix = new int[3, 3] { { 0, -1, 0 }, { 0, 1, 0 }, { 0, 0, 0 } };
            filterBitmap = Convolution(3, 3, 1, 1, matrix, 1, 127, originalBitmap);
            filterImage.Image = filterBitmap;
        }

        //public Bitmap SetEdgeDetection(Bitmap orgBitmap)
        //{
        //    //int[,] matrix = new int[3, 3] { { 0, -1, 0 }, { 0, 1, 0 }, { 0, 0, 0 } };
        //    Bitmap bitmap = (Bitmap)orgBitmap.Clone();
        //    int sumR = 0, sumG = 0, sumB = 0;
        //    int avgR = 0, avgG = 0, avgB = 0;
        //    Color c;

        //    for (int i = 0; i < bitmap.Width; i++)
        //    {
        //        for (int j = 0; j < bitmap.Height; j++)
        //        {
        //            c = bitmap.GetPixel(i, j);

        //            sumR = (int)c.R;
        //            sumG = (int)c.G;
        //            sumB = (int)c.B;

        //            if ((i - 1) >= 0)
        //            {
        //                sumR -= (int)bitmap.GetPixel(i - 1, j).R;
        //                sumG -= (int)bitmap.GetPixel(i - 1, j).G;
        //                sumB -= (int)bitmap.GetPixel(i - 1, j).B;
        //            }

        //            avgR = sumR + 127;
        //            avgG = sumG + 127;
        //            avgB = sumB + 127;

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
        //    return (Bitmap)bitmap.Clone();
        //}

        //NIE DZIALA!!!!!!!!!!!!!

        protected void EmbossOnClick(object sender, EventArgs e)
        {
            int[,] matrix = new int[3, 3] { { -1, -1, -1 }, { 0, 1, 0 }, { 1, 1, 1 } };
            filterBitmap = Convolution(3, 3, 1, 1, matrix, 1, 0, originalBitmap);
            filterImage.Image = filterBitmap;
        }

        //public Bitmap SetEmboss(Bitmap orgBitmap)
        //{
        //    Bitmap bitmap = (Bitmap)orgBitmap.Clone();
        //    int sumR = 0, sumG = 0, sumB = 0;
        //    int avgR = 0, avgG = 0, avgB = 0;
        //    Color c;

        //    for (int i = 0; i < bitmap.Width; i++)
        //    {
        //        for (int j = 0; j < bitmap.Height; j++)
        //        {
        //            c = bitmap.GetPixel(i, j);

        //            sumR = (int)c.R;
        //            sumG = (int)c.G;
        //            sumB = (int)c.B;

        //            if ((i - 1) >= 0)
        //            {
        //                sumR -= (int)bitmap.GetPixel(i - 1, j).R;
        //                sumG -= (int)bitmap.GetPixel(i - 1, j).G;
        //                sumB -= (int)bitmap.GetPixel(i - 1, j).B;
        //                if ((j - 1) >= 0)
        //                {
        //                    sumR -= (int)bitmap.GetPixel(i - 1, j - 1).R;
        //                    sumG -= (int)bitmap.GetPixel(i - 1, j - 1).G;
        //                    sumB -= (int)bitmap.GetPixel(i - 1, j - 1).B;
        //                }
        //                if ((j + 1) < bitmap.Height)
        //                {
        //                    sumR -= (int)bitmap.GetPixel(i - 1, j + 1).R;
        //                    sumG -= (int)bitmap.GetPixel(i - 1, j + 1).G;
        //                    sumB -= (int)bitmap.GetPixel(i - 1, j + 1).B;
        //                }
        //            }
        //            if ((i + 1) < bitmap.Width)
        //            {
        //                sumR += (int)bitmap.GetPixel(i + 1, j).R;
        //                sumG += (int)bitmap.GetPixel(i + 1, j).G;
        //                sumB += (int)bitmap.GetPixel(i + 1, j).B;
        //                if ((j + 1) < bitmap.Height)
        //                {
        //                    sumR += (int)bitmap.GetPixel(i + 1, j + 1).R;
        //                    sumG += (int)bitmap.GetPixel(i + 1, j + 1).G;
        //                    sumB += (int)bitmap.GetPixel(i + 1, j + 1).B;
        //                }
        //                if ((j - 1) >= 0)
        //                {
        //                    sumR += (int)bitmap.GetPixel(i + 1, j - 1).R;
        //                    sumG += (int)bitmap.GetPixel(i + 1, j - 1).G;
        //                    sumB += (int)bitmap.GetPixel(i + 1, j - 1).B;
        //                }
        //            }

        //            avgR = (int)(sumR);
        //            avgG = (int)(sumG);
        //            avgB = (int)(sumB);

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
        //    return (Bitmap)bitmap.Clone();
        //}

        // DOBRZE BYLO !!!!

        public Bitmap Convolution(int kernelRows, int kernelColumns, int pixelRow, int pixelColumn, int[,] kernelMatrix, int divisor, int offset, Bitmap orgBitmap)
        {
            Bitmap bitmap = (Bitmap)orgBitmap.Clone();
            int sumR = 0, sumG = 0, sumB = 0;
            int avgR = 0, avgG = 0, avgB = 0;
            Color c;

            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    for (int k = i - pixelColumn; k <= i + kernelColumns - pixelColumn - 1; k++)
                    {
                        for (int n = j - pixelRow; n <= j + kernelRows - pixelRow - 1; n++)
                        {
                            if (k >= 0 && n >= 0 && k < bitmap.Width && n < bitmap.Height)
                            {
                                c = orgBitmap.GetPixel(k, n);
                                sumR += (int)c.R * kernelMatrix[n - j + pixelRow, k - i + pixelColumn];
                                sumG += (int)c.G * kernelMatrix[n - j + pixelRow, k - i + pixelColumn];
                                sumB += (int)c.B * kernelMatrix[n - j + pixelRow, k - i + pixelColumn];
                            }
                        }
                    }

                    avgR = (int)(sumR / divisor) + offset;
                    avgG = (int)(sumG / divisor) + offset;
                    avgB = (int)(sumB / divisor) + offset;

                    if (avgR < 0) avgR = 0;
                    if (avgR > 255) avgR = 255;

                    if (avgG < 0) avgG = 0;
                    if (avgG > 255) avgG = 255;

                    if (avgB < 0) avgB = 0;
                    if (avgB > 255) avgB = 255;

                    bitmap.SetPixel(i, j, Color.FromArgb(avgR, avgG, avgB));
                    sumR = sumG = sumB = avgR = avgG = avgB = 0;
                }
            }
            return (Bitmap)bitmap.Clone();
        }

        //public Bitmap Convolution(int kernelSize, int[,] kernelMatrix, int divisor, int offset, Bitmap orgBitmap)
        //{
        //    Bitmap tempBitmap = (Bitmap)orgBitmap;
        //    Bitmap bitmap = (Bitmap)tempBitmap.Clone();
        //    int kernel = kernelSize / 2;
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
        //                        sumR += c.R * kernelMatrix[k - i + kernel, n - j + kernel];
        //                        sumG += c.G * kernelMatrix[k - i + kernel, n - j + kernel];
        //                        sumB += c.B * kernelMatrix[k - i + kernel, n - j + kernel];
        //                    }
        //                }
        //            }

        //            avgR = (int)(sumR / divisor) + offset;
        //            avgG = (int)(sumG / divisor) + offset;
        //            avgB = (int)(sumB / divisor) + offset;

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
        //    return (Bitmap)bitmap.Clone();
        //}
    }
}
