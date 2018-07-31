using System;
using System.Drawing;
using System.Windows.Forms;

namespace TestCSharpForm
{
    // delegate
    public delegate void UpdateScroll(int value, int max, int largePage);
    

    public partial class FormScrollBar : Form
    {
        public FormScrollBar()
        {
            InitializeComponent();
            Initialize();
        }

        #region プロパティ

        Document1 document1;

        #endregion

        #region メソッド

        private void Initialize()
        {
            document1 = new Document1(this.ClientSize.Width, this.ClientSize.Height, UpdateScrollBarV, UpdateScrollBarH);

            hScrollBar2.Maximum = 10000;
            hScrollBar2.LargeChange = ClientSize.Width;
            hScrollBar2.Value = 0;

            vScrollBar2.Maximum = 10000;
            vScrollBar2.LargeChange = ClientSize.Height;
            vScrollBar2.Value = 0;

            // マウスホイール
            this.MouseWheel += new MouseEventHandler(this.MainForm_MouseWheel);
        }

        public void UpdateScrollBarV(int value, int max, int large)
        {
            vScrollBar2.Value = value;
            vScrollBar2.Maximum = max;
            vScrollBar2.LargeChange = large;
        }

        public void UpdateScrollBarH(int value, int max, int large)
        {
            hScrollBar2.Value = value;
            hScrollBar2.Maximum = max;
            hScrollBar2.LargeChange = large;
        }

        #endregion メソッド

        #region イベント
        private void MainForm_MouseWheel(object sender, MouseEventArgs e)
        {
            // ホイール量は e.Delta
            if (vScrollBar2.Value - e.Delta < vScrollBar2.Minimum)
            {
                vScrollBar2.Value = vScrollBar2.Minimum;
            }
            else
            {
                vScrollBar2.Value -= e.Delta;
            }
            document1.UpdateSBV(vScrollBar2.Value, vScrollBar2.Maximum, vScrollBar2.LargeChange);
            this.Invalidate();
        }

        private void FormScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            
        }

        private void FormScrollBar_Load(object sender, EventArgs e)
        {

        }

        #endregion イベント

        private void FormScrollBar_Resize(object sender, EventArgs e)
        {
            const int barW = 30;

            // 自前で用意したスクロールバーのサイズを更新する
            vScrollBar2.SetBounds(this.Size.Width - vScrollBar2.Width, 0, barW, this.Size.Height - barW);
            hScrollBar2.SetBounds(0, this.Size.Height - barW - 100, this.Size.Width - barW, barW);

            // 画像サイズを更新
            document1.Resize(this.ClientSize.Width, this.ClientSize.Height);
        }

        private void FormScrollBar_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            int width = this.ClientSize.Width;
            int height = this.ClientSize.Height;

            document1.Draw(g);
        }

        private void vScrollBar2_Scroll(object sender, ScrollEventArgs e)
        {
            ScrollBar sb = (ScrollBar)sender;
            document1.UpdateSBV(sb.Value, sb.Maximum, sb.LargeChange);
            this.Invalidate();
        }

        private void hScrollBar2_Scroll(object sender, ScrollEventArgs e)
        {
            ScrollBar sb = (ScrollBar)sender;
            document1.UpdateSBH(sb.Value, sb.Maximum, sb.LargeChange);
            this.Invalidate();
        }
    }

    public struct SBInfo
    {
        public int max;
        public int large;
        public int value;

        public void Init(int value, int max, int large)
        {
            this.value = value;
            this.max = max;
            this.large = large;
        }
    }

    class Document1
    {
        //
        // Properties
        //
        SBInfo sbV, sbH;

        private Image image;                // LogView描画先のImage

        // delegate
        UpdateScroll delegateSBV;
        UpdateScroll delegateSBH;

        public Document1(int width, int height, UpdateScroll delegateSBV, UpdateScroll delegateSBH)
        {
            Resize(width, height);

            sbV = new SBInfo();
            sbV.Init(0, 10000, height);
            sbH = new SBInfo();
            sbH.Init(0, 10000, width);

            this.delegateSBV = delegateSBV;
            this.delegateSBH = delegateSBH;
        }

        // 
        // Methods 
        //
        public void Resize(int width, int height)
        {
            if (image != null)
            {
                image.Dispose();
            }

            image = new Bitmap(width, height);

            sbH.large = width;
            sbV.large = height;
        }

        public void Draw(Graphics g)
        {
            using (Graphics g2 = Graphics.FromImage(image))
            {
                // clear background
                g2.FillRectangle(Brushes.Black, 0, 0, image.Width, image.Height);

                int x = sbH.value  - (sbH.value % 100);
                int y = sbV.value  - (sbV.value % 100);
                
                var font1 = new Font("Arial", 12);

                // 横のライン
                bool drawStr = true;
                while ( y < sbV.value + sbV.large)
                {
                    if (y < 0)
                    {
                        y += 100;
                        continue;
                    }
                    if (drawStr)
                    {
                        //drawStr = false;
                        g2.DrawString(String.Format("{0}ms", y ), font1, Brushes.Yellow, 0, y - sbV.value);
                    }
                    g2.DrawLine(Pens.White, 0, y - sbV.value, image.Width, y - sbV.value);
                    y += 100;
                }
                // 縦のライン
                drawStr = true;
                while ( x < sbH.value + sbH.large)
                {
                    //if (x < 0)
                    //{
                    //    x += 100;
                    //    continue;
                    //}
                    if (drawStr)
                    {
                        //drawStr = false;
                        g2.DrawString(String.Format("{0}ms", x), font1, Brushes.Yellow, x - sbH.value, 0);
                    }
                    g2.DrawLine(Pens.White, x - sbH.value, 0, x - sbH.value, image.Height);
                    x += 100;
                }
            }
            g.DrawImage(image, 0, 0);
        }

        // Document側で表示情報を更新
        public void UpdateSBV(int value, int max, int large)
        {
            // スクロールバーに反映
            sbV.max = max;
            sbV.large = large;
            sbV.value = value;

            delegateSBV(value, max, large);
        }

        // Document側で表示情報を更新
        public void UpdateSBH(int value, int max, int large)
        {
            // スクロールバーに反映
            sbH.max = max;
            sbH.large = large;
            sbH.value = value;

            delegateSBH(value, max, large);
        }
    }
}
