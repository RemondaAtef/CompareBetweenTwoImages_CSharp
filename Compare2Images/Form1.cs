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

        // if two images are identical return true , else return false
        private void Compare_Click(object sender, EventArgs e)
        {
            //bool result = AbuEhabHelper.ImagesArea.CompareIamges(pictureBox1, pictureBox2);
            //MessageBox.Show(result.ToString());         



            Bitmap img1 = new Bitmap(pictureBox3.Image);
            Bitmap img2 = new Bitmap(pictureBox4.Image);

            if (img1.Size != img2.Size)
            {
                string result = ("Images are of different sizes");
                // Console.Error.WriteLine("Images are of different sizes");
                //return;

                MessageBox.Show(result);
            }

            float diff = 0;

            for (int y = 0; y < img1.Height; y++)
            {
                for (int x = 0; x < img1.Width; x++)
                {
                    Color pixel1 = img1.GetPixel(x, y);
                    Color pixel2 = img2.GetPixel(x, y);

                    diff += Math.Abs(pixel1.R - pixel2.R);
                    diff += Math.Abs(pixel1.G - pixel2.G);
                    diff += Math.Abs(pixel1.B - pixel2.B);
                }
            }
            //string result1 = "Percentage difference between images: ";
            //float result2 = 100 * (diff / 255) / (img1.Width * img1.Height * 3);
            //string result3 = " %";

            //MessageBox.Show((result1, result2, result3).ToString());

            MessageBox.Show( ("Percentage difference between images: ", 100 * (diff / 255) / (img1.Width * img1.Height * 3), " %").ToString());
            
            
            //   MessageBox.Show(result1.ToString())

            //"diff: {0} %"
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

                //rectW = e.X - crpX;
                //rectH = e.Y - crpY;

                Graphics g = pictureBox2.CreateGraphics();
                g.DrawRectangle(crpPen, crpX, crpY, rectW2, rectH2);
                g.Dispose();
            }
        }
        private void CropArea2_Click(object sender, EventArgs e)
        {
            label2.Text = "Dimensions :" + rectW2 + "," + rectH2;
            //if (label2.Text == label1.Text)
            //{
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
              //  }

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

