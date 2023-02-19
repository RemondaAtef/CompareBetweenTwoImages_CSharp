using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Compare2Images
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        OpenFileDialog op1 = new OpenFileDialog();   // to browse image
        private void Image1_Click(object sender, EventArgs e)
        {
            op1.ShowDialog();
            pictureBox1.Image = Image.FromFile(op1.FileName);
        }


        OpenFileDialog op2 = new OpenFileDialog();
        private void Image2_Click(object sender, EventArgs e)
        {
            op2.ShowDialog();
            pictureBox2.Image = Image.FromFile(op2.FileName);
        }

        public byte[,] ImageTo2dByte(Bitmap bmp)
        {
            try
            {
                int width = bmp.Width;
                int height = bmp.Height;
                BitmapData data = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                byte[] bytes = new byte[height * data.Stride];
                try
                {
                    System.Runtime.InteropServices.Marshal.Copy(data.Scan0, bytes, 0, bytes.Length);
                }

                finally
                {
                    bmp.UnlockBits(data);
                }
                byte[,] result = new byte[height, width];
                for (int y = 0; y < height; ++y)
                    for (int x = 0; x < width; ++x)
                    {
                        int offset = y * data.Stride + x * 3;
                        result[y, x] = (byte)((bytes[offset + 0] + bytes[offset + 1] + bytes[offset + 2]) / 3);
                    }
                return result;
            }
            catch (Exception ex)
            {
                return new byte[0, 0];
            }
        }


        // if two images are identical return true , else return false
        private void Compare_Click(object sender, EventArgs e)
        {
           

            Bitmap img1 = new Bitmap(pictureBox3.Image);
            Bitmap img2 = new Bitmap(pictureBox4.Image);

           
            var x = ImageTo2dByte(img1);
            var y = ImageTo2dByte(img2);


            double Similarity = 0, difference = 0;
            for (int i = 0; i < x.GetLength(0); i++)
            {
                for (int j = 0; j < x.GetLength(1); j++)
                {
                    if (x[i, j] == y[i, j])
                        Similarity++;
                    else difference++;
                }
            }
            Similarity = (Similarity / (x.GetLength(0) * x.GetLength(1))) * 100;
            difference = (difference / (x.GetLength(0) * x.GetLength(1))) * 100;

            MessageBox.Show(("Percentage difference between images: ", difference, " %").ToString());


        }

        private void SelectArea1_Click(object sender, EventArgs e)
        {
            //here we declare mouse event handlers

            pictureBox1.MouseDown += new MouseEventHandler(pictureBox1_MouseDown);

            pictureBox1.MouseMove += new MouseEventHandler(pictureBox1_MouseMove);

            pictureBox1.MouseEnter += new EventHandler(pictureBox1_MouseEnter);
            Controls.Add(pictureBox1);
        }

        //declare some variable for crop coordinates
        int crpX, crpY, rectW, rectH;
        // Declare crop pen for cropping image
        public Pen crpPen = new Pen(Color.White);
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                Cursor = Cursors.Cross;
                crpPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                // set initial x,y co ordinates for crop rectangle
                //this is where we firstly click on image
                crpX = e.X;
                crpY = e.Y;

            }
        }
        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            base.OnMouseEnter(e);
            Cursor = Cursors.Cross;
        }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                pictureBox1.Refresh();
                //set width and height for crop rectangle.
                rectW = e.X - crpX;
                rectH = e.Y - crpY;
                Graphics g = pictureBox1.CreateGraphics();
                g.DrawRectangle(crpPen, crpX, crpY, rectW, rectH);
                g.Dispose();
            }
        }
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            Cursor = Cursors.Default;
        }

        public void CropArea1_Click(object sender, EventArgs e)
        {
            label1.Text = "Dimensions :" + rectW + "," + rectH;
            Cursor = Cursors.Default;
            //Now we will draw the cropped image into pictureBox2
            Bitmap bmp2 = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.DrawToBitmap(bmp2, pictureBox1.ClientRectangle);

            Bitmap crpImg = new Bitmap(rectW, rectH);

            for (int i = 0; i < rectW; i++)
            {
                for (int y = 0; y < rectH; y++)
                {
                    Color pxlclr = bmp2.GetPixel(crpX + i, crpY + y);
                    crpImg.SetPixel(i, y, pxlclr);
                }
            }

            pictureBox3.Image = (Image)crpImg;
            pictureBox3.SizeMode = PictureBoxSizeMode.CenterImage;
        }

        int rectW2, rectH2;
       // public Pen crpPen2 = new Pen(Color.White);

        private void SelectArea2_Click(object sender, EventArgs e)
        {
            pictureBox2.MouseDown += new MouseEventHandler(pictureBox2_MouseDown);

            pictureBox2.MouseMove += new MouseEventHandler(pictureBox2_MouseMove);

            pictureBox2.MouseEnter += new EventHandler(pictureBox1_MouseEnter);
            Controls.Add(pictureBox2);
        }
        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                Cursor = Cursors.Cross;
                crpPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                // set initial x,y co ordinates for crop rectangle
                //this is where we firstly click on image

                //crpX2 = crpX;
                //crpY2 = crpX;

                crpX = e.X;
                crpY = e.Y;

            }
        }
        private void pictureBox2_MouseEnter(object sender, EventArgs e)
        {
            base.OnMouseEnter(e);
            Cursor = Cursors.Cross;
        }

        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                pictureBox2.Refresh();
                //set width and height for crop rectangle.

                rectW2 = rectW;
                rectH2 = rectH;

                Graphics g = pictureBox2.CreateGraphics();
                g.DrawRectangle(crpPen, crpX, crpY, rectW2, rectH2);
                g.Dispose();
            }
        }
        private void CropArea2_Click(object sender, EventArgs e)
        {
            label2.Text = "Dimensions :" + rectW2 + "," + rectH2;
           
                Cursor = Cursors.Default;
                //Now we will draw the cropped image into pictureBox4
                Bitmap bmp2 = new Bitmap(pictureBox2.Width, pictureBox2.Height);
                pictureBox2.DrawToBitmap(bmp2, pictureBox2.ClientRectangle);

                Bitmap crpImg = new Bitmap(rectW2, rectH2);

                for (int i = 0; i < rectW2; i++)
                {
                    for (int y = 0; y < rectH2; y++)
                    {
                        Color pxlclr2 = bmp2.GetPixel(crpX + i, crpY + y);
                        crpImg.SetPixel(i, y, pxlclr2);
                    }
             

                pictureBox4.Image = (Image)crpImg;
                pictureBox4.SizeMode = PictureBoxSizeMode.CenterImage;
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }          
        
        
       
    }
    }

