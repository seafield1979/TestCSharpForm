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
    public partial class FormProgressBar : Form
    {
        public FormProgressBar()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            ((Button)sender).Enabled = false;

            // Progressクラスのインスタンスを生成
            var p = new Progress<int>(ShowProgress);

            string result = await Task.Run(() => DoWork(p, 100));

            ((Button)sender).Enabled = true;
        }

        // 進捗を表示するメソッド（これはUIスレッドで呼び出される）
        private void ShowProgress(int percent)
        {
            label1.Text = percent + "％完了";
            progressBar1.Value = percent;
        }

        // バックグラウンドで行う処理
        private string DoWork(IProgress<int> progress, int n)
        {
            //時間のかかる処理を開始する
            for (int i = 1; i <= 10; i++)
            {
                System.Threading.Thread.Sleep(200);

                int parcentage = i * 100 / 10;
                progress.Report(parcentage);

            }

            return "complete!!";
        }

        private void FormProgressBar_Load(object sender, EventArgs e)
        {
            progressBar1.Maximum = 100;
        }
    }
}
