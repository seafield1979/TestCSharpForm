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
    public partial class FormRadio : Form
    {
        public FormRadio()
        {
            InitializeComponent();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.AppendText("radio1\n");
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.AppendText("radio2\n");
        }
    }
}
