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
    public partial class FormComboBox : Form
    {
        public FormComboBox()
        {
            InitializeComponent();
        }

        private void FormComboBox_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("add 1");
            comboBox1.Items.Add("add 2");
            comboBox1.Items.Add("add 3");

            comboBox1.SelectedIndex = 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 選択された項目番号を取得
            Console.WriteLine("comboBox1.SelectedIndex:{0}", comboBox1.SelectedIndex);

            // 選択されている項目を取得
            Console.WriteLine("comboBox1.SelectedItem:{0}", comboBox1.SelectedItem);
        }
    }
}
