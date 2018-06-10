﻿using System;
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
    public partial class FormTabControl : Form
    {
        public FormTabControl()
        {
            InitializeComponent();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabControl tabc = (TabControl)sender;
            Console.WriteLine("TabIndexChanged:{0}", tabc.SelectedIndex);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Console.WriteLine("checkbox1:{0}", checkBox1.Checked);
            Console.WriteLine("checkbox1:{1}", checkBox2.Checked);
            Console.WriteLine("checkbox1:{2}", checkBox3.Checked);
        }
    }
}
