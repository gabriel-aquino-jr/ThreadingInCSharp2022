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

      
        private void btnGo_Click(object sender, EventArgs e)
        {

        }

        private void btnGoNoThread_Click(object sender, EventArgs e)
        {
            int.TryParse(textBox1.Text, out int i);
            int.TryParse(textBox2.Text, out int sleep);

            btnGoNoThread.Enabled = textBox1.Enabled = textBox2.Enabled = false;

            for (int j = 0; j <= i; j++)
            {
                lblCounter.Text = j.ToString();
                System.Threading.Thread.Sleep(sleep);
            }

            btnGoNoThread.Enabled = textBox1.Enabled = textBox2.Enabled = true;

            return;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //We need to kill any running threads
            
        }
    }
}
