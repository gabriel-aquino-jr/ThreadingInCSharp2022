using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThreadingUpdateUI
{
    public partial class PictureScaler : Form
    {
        public PictureScaler()
        {
            InitializeComponent();
        }

        int scaleTime = 2000;
        int width;
        int height;

        private void PictureScaler_Load(object sender, EventArgs e)
        {
            LoadImage(pictureBox1);
            LoadImage(pictureBox2);
            LoadMe(pictureBox5);
            width = pictureBox1.Width;
            height = pictureBox1.Height;
        }

        private void btnSpin_Click(object sender, EventArgs e)
        {
            Rotate();
        }

        bool rotate = true;

        private void Rotate()
        {
            rotate = true;
            int degrees = 1;
            Image img = pictureBox5.Image;
            Task.Factory.StartNew(() =>
            {
                while (rotate)
                {
                    if (degrees == 365)
                        degrees = 1;

                    degrees += 1;
                    Thread.Sleep(10);

                    this.Invoke((MethodInvoker)delegate
                    {
                        pictureBox5.Image = RotateImage(chkRecSpin.Checked ? pictureBox5.Image : img, degrees);
                    });
                }
            });
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            rotate = false;
            LoadMe(pictureBox5);
        }

        private void btnWhipe_Click(object sender, EventArgs e)
        {
            btnWhipe.Enabled = false;

            int widthScalePerMilli = scaleTime / width;
            int heightScalePerMilli = scaleTime / height;

            Task.Factory.StartNew(() =>
            {
                while (pictureBox1.Width > 0)
                {
                    Thread.Sleep(10);

                    this.Invoke((MethodInvoker)delegate
                    {
                        pictureBox1.Width -= widthScalePerMilli;
                        //pictureBox1.Height -= heightScalePerMilli;
                    });
                }
            }).ContinueWith(delegate
            {
                this.Invoke((MethodInvoker)delegate
                {
                    btnWhipe.Enabled = true;
                });
            });
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            LoadImage(pictureBox1);
            pictureBox1.Width = width;
            pictureBox1.Height = height;
        }

        private void LoadImage(PictureBox pb)
        {
            string path = @"images\logo.png";
            pb.Image = Image.FromFile(path);
        }

        private void LoadMe(PictureBox pb)
        {
            string path = @"images\git.png";
            pb.Image = Image.FromFile(path);
        }

        int currentX;
        int currentY;

        private void btnScale_Click(object sender, EventArgs e)
        {
            if (pictureBox2.Image == null) return;

            Bitmap bmp = (Bitmap)Bitmap.FromFile(@"images\logo.png");
            int sHeight = bmp.Height;
            int sWidth = bmp.Width;
            currentX = pictureBox2.Left;
            currentY = pictureBox2.Top;

            btnScale.Enabled = false;

            int widthScalePerMilli = scaleTime / sWidth;

            Task.Factory.StartNew(() =>
            {
                while (bmp != null)
                {
                    Thread.Sleep(10);

                    this.Invoke((MethodInvoker)delegate
                    {
                        pictureBox2.Image = ResizeBitmap(bmp, bmp.Width - widthScalePerMilli, ((sHeight * (bmp.Width - widthScalePerMilli) / sWidth)));
                        pictureBox2.Left = currentX + ((sWidth - bmp.Width) / 2);
                        pictureBox2.Top = currentY + ((sHeight - bmp.Height) / 2);
                        bmp = (Bitmap)pictureBox2.Image;
                    });
                }
            }).ContinueWith(delegate
            {
                this.Invoke((MethodInvoker)delegate
                {
                    btnScale.Enabled = true;
                });
            });
        }

        public Bitmap ResizeBitmap(Bitmap bmp, int width, int height)
        {
            if (width < 0) return null;

            if (height < 0) return null;
            Bitmap result = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(result))
            {
                g.DrawImage(bmp, 0, 0, width, height);
            }

            return result;
        }

        private void btnResetScale_Click(object sender, EventArgs e)
        {
            LoadImage(pictureBox2);
            pictureBox2.Width = width;
            pictureBox2.Height = height;
            pictureBox2.Left = currentX;
            pictureBox2.Top = currentY;
        }

        private void PictureScaler_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            lblWaiting.Text = "Player thinking...";

            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(2000);
                this.Invoke((MethodInvoker)delegate
                {
                    LoadImage(pictureBox3);
                    lblWaiting.Text = "";
                });
                Thread.Sleep(2000);

                this.Invoke((MethodInvoker)delegate
                {
                    lblWaiting.Text = "Player thinking...";
                });
            }).ContinueWith(delegate
            {
                Thread.Sleep(2000);
                this.Invoke((MethodInvoker)delegate
                {
                    LoadImage(pictureBox4);
                    lblWaiting.Text = "";
                });
            }).ContinueWith(delegate
            {
                Thread.Sleep(2000);
                this.Invoke((MethodInvoker)delegate
                {
                    pictureBox3.Image = null;
                    pictureBox4.Image = null;
                    lblWaiting.Text = "Play done";
                });
            });
        }


        private Image RotateImage(Image img, float rotationAngle)
        {
            //create an empty Bitmap image
            Bitmap bmp = new Bitmap(img.Width, img.Height);

            //turn the Bitmap into a Graphics object
            Graphics gfx = Graphics.FromImage(bmp);

            //now we set the rotation point to the center of our image
            gfx.TranslateTransform((float)bmp.Width / 2, (float)bmp.Height / 2);

            //now rotate the image
            gfx.RotateTransform(rotationAngle);

            gfx.TranslateTransform(-(float)bmp.Width / 2, -(float)bmp.Height / 2);

            //set the InterpolationMode to HighQualityBicubic so to ensure a high
            //quality image once it is transformed to the specified size
            gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;

            //now draw our new image onto the graphics object
            gfx.DrawImage(img, new Point(0, 0));

            //dispose of our Graphics object
            gfx.Dispose();

            //return the image
            return bmp;
        }


    }
}

