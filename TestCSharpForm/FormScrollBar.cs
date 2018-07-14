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
    public partial class FormScrollBar : Form
    {
        public FormScrollBar()
        {
            InitializeComponent();
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            label1.Text = String.Format("value={0}", hScrollBar1.Value);
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            label2.Text = String.Format("value={0}", vScrollBar1.Value);
        }

        private void FormScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            label3.Text = String.Format("HScroll value={0}", this.HorizontalScroll.Value);
            label4.Text = String.Format("VScroll value={0}", this.VerticalScroll.Value);
        }

        private void FormScrollBar_Load(object sender, EventArgs e)
        {

        }
    }
}
