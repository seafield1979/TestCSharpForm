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
    public partial class FormCheckBox : Form
    {
        public FormCheckBox()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine("checkBox[0]:{0}", checkBox1.Checked);
            Console.WriteLine("checkBox[1]:{0}", checkBox2.Checked);
            Console.WriteLine("checkBox[2]:{0}", checkBox3.Checked);
            Console.WriteLine("checkBox[3]:{0}", checkBox4.Checked);
            Console.WriteLine("checkBox[4]:{0}", checkBox5.Checked);
            checkBox1.CheckState = CheckState.Indeterminate;

        }
        private void button2_Click(object sender, EventArgs e)
        {
            // 全項目数
            Console.WriteLine("ItemCount:{0}", checkedListBox1.Items.Count);

            // チェックされた項目数
            Console.WriteLine("CheckedItemCount:{0}", checkedListBox1.CheckedItems.Count);

            // チェックされた項目名を取得
            foreach (string item in checkedListBox1.CheckedItems)
            {
                Console.WriteLine("checkListItem:{0}", item);
            }

            // チェック項目のインデックスを取得
            foreach ( int index in checkedListBox1.CheckedIndices)
            {
                Console.WriteLine("checkedListIndex:{0}", index);
            }
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Console.WriteLine("SelectedIndexChanged");
        }

        private void checkedListBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            
            Console.WriteLine("SelectedValueChanged {0}");
        }

        private void FormCheckBox_Load(object sender, EventArgs e)
        {
            checkedListBox1.Items.Remove("hoge");
            checkedListBox1.Items.Clear();
            checkedListBox1.Items.Add("apple", CheckState.Checked);
            checkedListBox1.Items.Add("apple", CheckState.Indeterminate);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            Console.WriteLine("checkBox1:{0}", cb.Checked);
        }
    }
}
