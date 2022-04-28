using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThreadingUpdateUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        List<Label> threadLabels = new List<Label>();
      
        private void btnGo_Click(object sender, EventArgs e)
        {
            if (threadLabels.Count < 10)
            {
                int.TryParse(textBox1.Text, out int i);
                int.TryParse(textBox2.Text, out int sleep);

                Label l = new Label();
                l.Text = DateTime.Now.ToLongTimeString();

                threadLabels.Add(l);

                l.Top = (panel1.Controls.Count) * l.Height + 20;
                l.Left = 5;

                this.panel1.Controls.Add(l);

                //btnGoNoThread.Enabled = textBox1.Enabled = textBox2.Enabled = false;

                Task.Factory.StartNew(() =>
                   {
                   for (int j = 0; j <= i; j++)
                   {
                           // to avoid exception of using thread
                           this.Invoke((MethodInvoker)delegate
                           {
                               l.Text = j.ToString();
                           }
                           );                           
                           System.Threading.Thread.Sleep(sleep);
                       }
                   });
            }
        }

        private void btnGoNoThread_Click(object sender, EventArgs e)
        {
            int.TryParse(textBox1.Text, out int i);
            int.TryParse(textBox2.Text, out int sleep);

            btnGoNoThread.Enabled = textBox1.Enabled = textBox2.Enabled = false;

            for (int j = 0; j <= i; j++)
            {
                lblCounter.Text = j.ToString();
                lblCounter.Refresh();
                System.Threading.Thread.Sleep(sleep);
            }

            btnGoNoThread.Enabled = textBox1.Enabled = textBox2.Enabled = true;

            return;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //We need to kill any running threads
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}
