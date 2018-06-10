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
    public partial class FormSplitter2 : Form
    {
        private bool isVisibleAll = true;

        public FormSplitter2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (panel3.Visible)
            {
                panel3.Hide();
                splitter1.Hide();
            }
            else
            {
                panel3.Show();
                splitter1.Show();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (panel4.Visible)
            {
                panel4.Hide();
                splitter2.Hide();
            }
            else
            {
                panel4.Show();
                splitter2.Show();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (panel5.Visible)
            {
                panel5.Hide();
            }
            else
            {
                panel5.Show();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (panel2.Visible)
            {
                panel2.Hide();
            }
            else
            {
                panel2.Show();
            }
        }
    }
}
