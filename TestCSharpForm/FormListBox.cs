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
    public partial class FormListBox : Form
    {
        private int itemCnt = 0;

        public FormListBox()
        {
            InitializeComponent();
        }

        private void FormListBox_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < 10; i++) {
                listBox1.Items.Add(String.Format("item{0}", i+1));
            }

            if (listBox1.Items.Count > 0)
            {
                listBox1.SelectedIndex = 0;
            }
            updateInfo();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 項目数
            Console.WriteLine("項目数:{0}", listBox1.Items.Count);
            
            // 選択されている項目番号
            Console.WriteLine("選択されている項目番号:{0}", listBox1.SelectedIndex);

            updateInfo();
        }

        // 項目を追加ボタンクリック
        private void button2_Click(object sender, EventArgs e)
        {
            // 項目を追加
            listBox1.Items.Add(String.Format("item{0}", itemCnt + 1));

            itemCnt++;
            updateInfo();
        }

        // 項目をクリアボタンクリック
        private void button3_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            itemCnt = 0;

            updateInfo();
        }

        private void updateInfo()
        {
            textBox1.Text = listBox1.Items.Count.ToString();
            textBox2.Text = listBox1.SelectedIndex.ToString();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateInfo();
        }

        // 項目がダブルクリックされた
        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            Console.WriteLine("DoubleClicked:{0}", listBox1.SelectedIndex);
            updateInfo();
        }
    }
}
