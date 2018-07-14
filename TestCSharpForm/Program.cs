using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestCSharpForm
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Mutex名を決める（必ずアプリケーション固有の文字列に変更すること！）
            string mutexName = "MyApplicationName";
            //Mutexオブジェクトを作成する
            System.Threading.Mutex mutex = new System.Threading.Mutex(false, mutexName);

            bool hasHandle = false;
            try
            {
                try
                {
                    //ミューテックスの所有権を要求する
                    hasHandle = mutex.WaitOne(0, false);
                }
                //.NET Framework 2.0以降の場合
                catch (System.Threading.AbandonedMutexException)
                {
                    //別のアプリケーションがミューテックスを解放しないで終了した時
                    hasHandle = true;
                }
                //ミューテックスを得られたか調べる
                if (hasHandle == false)
                {
                    //得られなかった場合は、すでに起動していると判断して終了
                    MessageBox.Show("多重起動はできません。");
                    return;
                }

                //はじめからMainメソッドにあったコードを実行
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                int mode = 14;
                switch (mode)
                {
                    case 1:
                        Application.Run(new Form1());
                        break;
                    case 2:
                        Application.Run(new FormRadio());
                        break;
                    case 3:
                        Application.Run(new FormCheckBox());
                        break;
                    case 4:
                        Application.Run(new FormComboBox());
                        break;
                    case 5:
                        Application.Run(new FormListBox());
                        break;
                    case 6:
                        Application.Run(new FormTreeView());
                        break;
                    case 7:
                        Application.Run(new FormToolStripContainer());
                        break;
                    case 8:
                        Application.Run(new FormSplitter1());
                        break;
                    case 9:
                        Application.Run(new FormSplitter2());
                        break;
                    case 10:
                        Application.Run(new FormSplitContainer());
                        break;
                    case 11:
                        Application.Run(new FormProgressBar());
                        break;
                    case 12:
                        Application.Run(new FormScrollBar());
                        break;
                    case 13:
                        Application.Run(new FormListView());
                        break;
                    case 14:
                        Application.Run(new FormImage());
                        break;
                }
            }
            finally
            {
                if (hasHandle)
                {
                    //ミューテックスを解放する
                    mutex.ReleaseMutex();
                }
                mutex.Close();
            }
        }
    }
}
