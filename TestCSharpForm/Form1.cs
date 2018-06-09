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
    public partial class Form1 : Form
    {
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        static extern IntPtr SendMessage(
            IntPtr hWnd, int msg, int wParam, int[] lParam);

        public Form1()
        {
            InitializeComponent();
        }

        private void メニュー２ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("メニュー２");
        }

        private void メニュー３ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("メニュー３");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.Indent();
            System.Diagnostics.Debug.WriteLine("test1");
            System.Diagnostics.Debug.Unindent();

            // ラベル
            label1.Text = "てすと１";
            label1.ForeColor = Color.Red;
            label1.BackColor = Color.FromArgb(100, 0, 0, 0);

            // Arial の 12 ポイントに設定する
            label2.Font = new Font("Arial", 12);

            //// 上の例に加え、太字のスタイルを設定する
            //this.textBox1.Font = new Font("Arial", 12, FontStyle.Bold);

            //// 上の例に加え、斜体のスタイルを設定する
            //this.textBox1.Font = new Font("Arial", 12, FontStyle.Bold | FontStyle.Italic);

            //// 上の例に加え、フォントの大きさの指定をピクセルで設定する
            //this.textBox1.Font = new Font("Arial", 12, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Pixel);
            label2.TextAlign = ContentAlignment.BottomCenter;
            label2.BorderStyle = BorderStyle.FixedSingle;
            label2.Tag = 100;
            label2.Visible = true;

            const int EM_SETTABSTOPS = 0x00CB;
            SendMessage(textBox1.Handle, EM_SETTABSTOPS, 1, new int[] { 16 });

            Console.WriteLine(label2.Tag);

            // メニューに項目を追加
            // label
            ToolStripMenuItem1.DropDownItems.Add(new ToolStripLabel("hoge"));

            // button
            ToolStripButton button1 = new ToolStripButton("hoge2");
            button1.Click += new EventHandler(MenuButton1_Click);
            ToolStripMenuItem1.DropDownItems.Add(button1);

            // 既存のcomboboxに項目を追加
            ToolStripMenuCombo1.Items.Add("A");
            ToolStripMenuCombo1.Items.Add("B");
            ToolStripMenuCombo1.Items.Add("C");
            ToolStripMenuCombo1.Items.Add("D");
            ToolStripMenuCombo1.Items.Add("E");
            ToolStripMenuCombo1.Items.Add("F");

            ToolStripTextBox tstb1 = new ToolStripTextBox("tsTextBox1");
            tstb1.Text = "text1";
            ToolStripMenuItem1.DropDownItems.Add(tstb1);

            Test1();
        }

#if true

#endif  

        private void Test1()
        {

            System.Diagnostics.Debug.IndentSize = 10;
            System.Diagnostics.Debug.IndentLevel = 2;
            System.Diagnostics.Debug.Indent();
            System.Diagnostics.Debug.WriteLine("hoge");

            //progressBar1.
        }

        // フォームが閉じる前のイベント
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //メッセージボックスを表示する
            //MessageBox.Show("正しい値を入力してください。",
            //    "エラー",
            //    MessageBoxButtons.OK,
            //    MessageBoxIcon.Error);

            DialogResult result = MessageBox.Show("フォームを閉じますか？",
               "form closing",
               MessageBoxButtons.YesNo,
               MessageBoxIcon.Exclamation,
               MessageBoxDefaultButton.Button2);
            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void testMsgBox1()
        {
            //メッセージボックスを表示する
            DialogResult result = MessageBox.Show( "メッセージ1",
                "タイトル1",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Exclamation,
                MessageBoxDefaultButton.Button2);

            //何が選択されたか調べる
            switch (result)
            {
                case DialogResult.Yes:
                    Console.WriteLine("「はい」が選択されました");
                    break;
                case DialogResult.No:
                    Console.WriteLine("「いいえ」が選択されました");
                    break;
                case DialogResult.Cancel:
                    Console.WriteLine("「キャンセル」が選択されました");
                    break;
            }
        }

        // button1クリック
        private void button1_Click(object sender, EventArgs e)
        {
            testMsgBox1();
        }

        // メニューのボタンをクリック
        private void MenuButton1_Click(object sender, EventArgs e)
        {
            testMsgBox1();
        }

        // ドロップされた時の処理
        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] fileNames = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            foreach (var fileName in fileNames)
            {
                Console.WriteLine(fileName);
            }
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            Control c = (Control)sender;
            Console.WriteLine("フォームのサイズが{0}x{1}に変更されました",  c.Width, c.Height);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {

        }

        // フォーム上でキーが入力された
        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            Console.WriteLine("key_code:{0}", e.KeyChar);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Label1を再描画する
            label1.Update();

            progressBar1.Minimum = 0;
            progressBar1.Maximum = 10;
            progressBar1.Value = 0;

            //時間のかかる処理を開始する
            for (int i = 1; i <= 10; i++)
            {
                //1秒間待機する（時間のかかる処理があるものとする）
                System.Threading.Thread.Sleep(1000);

                //ProgressBar1の値を変更する
                progressBar1.Value = i;
                //Label1のテキストを変更する
                label1.Text = i.ToString();

                //Label1を再描画する
                //label1.Update();
                progressBar1.Value = i;
                //（フォーム全体を再描画するには、次のようにする）
                this.Update();
            }

            MessageBox.Show("complete!!");
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            Console.WriteLine("textBox1_Enter");
        }

        private void toolStripTextBox1_TextChanged(object sender, EventArgs e)
        {
            ToolStripTextBox tb = (ToolStripTextBox)sender;

            Console.WriteLine(((ToolStripTextBox)sender).Text);
            label2.Text = tb.Text;
        }
    }
}
