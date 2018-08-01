using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestCSharpForm
{
    public partial class FormPanel : Form
    {
        public FormPanel()
        {
            InitializeComponent();
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            vScrollBar1.Maximum = 200;
            hScrollBar1.Maximum = 200;
            vScrollBar1.Enabled = true;
            hScrollBar1.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            vScrollBar1.Maximum = 100;
            vScrollBar1.LargeChange = 110;
            hScrollBar1.Maximum = 100;
            hScrollBar1.LargeChange = 110;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            vScrollBar1.Enabled = false;
            hScrollBar1.Enabled = false;
        }
    }
}
