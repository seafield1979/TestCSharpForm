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
    public partial class FormSplitter1 : Form
    {
        public FormSplitter1()
        {
            InitializeComponent();
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void splitter1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            updateLabelText();
        }

        private void splitter2_SplitterMoved(object sender, SplitterEventArgs e)
        {
            updateLabelText();
        }

        private void updateLabelText()
        {
            label1.Text = String.Format("[{0},{1}]", panel2.Size.Width, panel2.Size.Height);
            label3.Text = String.Format("[{0},{1}]", panel3.Size.Width, panel3.Size.Height);
            label4.Text = String.Format("[{0},{1}]", panel4.Size.Width, panel4.Size.Height);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (panel3.Visible)
            {
                panel3.Hide();
            }
            else
            {
                panel3.Show();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (panel4.Visible)
            {
                panel4.Hide();
            }
            else
            {
                panel4.Show();
            }
        }
    }
}
