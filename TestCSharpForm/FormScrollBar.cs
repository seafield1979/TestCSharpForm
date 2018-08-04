using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace TestCSharpForm
{
    // delegate
    public delegate void UpdateScroll(int value);
    

    public partial class FormScrollBar : Form
    {
        public FormScrollBar()
        {
            InitializeComponent();
            Initialize();
        }

        #region プロパティ

        Document1 document1;
        private bool isMouseDown;
        private Point mouseDownPos;
        private Point mouseOldPos;

        #endregion

        #region メソッド

        private void Initialize()
        {
            document1 = new Document1(this.ClientSize.Width, this.ClientSize.Height, hScrollBar2, vScrollBar2);

            // マウスホイールのイベント登録
            this.MouseWheel += new MouseEventHandler(this.MainForm_MouseWheel);

        }

        public void UpdateScrollBarV(int value)
        {
            vScrollBar2.Value = value;
        }

        public void UpdateScrollBarH(int value)
        {
            hScrollBar2.Value = value;
        }

        private void ScrollX(int move)
        {
            
        }

        private void ScrollY(int move)
        {
            vScrollBar2.Value += move;
            panel1.Invalidate();
        }

        // zoom::
        private void ZoomUp()
        {
            if (document1.ZoomUp())
            {
                panel1.Invalidate();
            }
        }

        private void ZoomDown()
        {
            if (document1.ZoomDown())
            {
                panel1.Invalidate();
            }
            ;
        }

        #endregion メソッド

        #region イベント
        private void MainForm_MouseWheel(object sender, MouseEventArgs e)
        {
            // ホイール量は e.Delta
            if (document1.ScrollY(-e.Delta) == true)
            {
                panel1.Invalidate();
            }
        }

        private void FormScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            
        }

        private void FormScrollBar_Load(object sender, EventArgs e)
        {

        }

        private void FormScrollBar_Resize(object sender, EventArgs e)
        {
            const int barW = 30;

            // 自前で用意したスクロールバーのサイズを更新する
            vScrollBar2.SetBounds(this.Size.Width - vScrollBar2.Width, 0, barW, this.Size.Height - barW);
            hScrollBar2.SetBounds(0, this.Size.Height - barW - 100, this.Size.Width - barW, barW);

            // 画像サイズを更新
            document1.Resize(this.ClientSize.Width, this.ClientSize.Height);

            panel1.Invalidate();
        }

        private void FormScrollBar_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void vScrollBar2_Scroll(object sender, ScrollEventArgs e)
        {
            ScrollBar sb = (ScrollBar)sender;
            document1.UpdateSBV(sb.Value);
            panel1.Invalidate();
        }

        private void hScrollBar2_Scroll(object sender, ScrollEventArgs e)
        {
            ScrollBar sb = (ScrollBar)sender;
            document1.UpdateSBH(sb.Value);
            panel1.Invalidate();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            int width = this.ClientSize.Width;
            int height = this.ClientSize.Height;

            document1.Draw(g);
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;
            mouseDownPos = e.Location;
            mouseOldPos = e.Location;
        }

        private void panel1_MouseLeave(object sender, EventArgs e)
        {
            isMouseDown = false;
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                int moveX = e.X - mouseOldPos.X;
                int moveY = e.Y - mouseOldPos.Y;
                mouseOldPos.X = e.X;
                mouseOldPos.Y = e.Y;
                //Debug.WriteLine("mouse move {0} {1}", moveX, moveY); 

                // ホイール量は e.Delta
                if (document1.ScrollX(moveX) == true)
                {
                    panel1.Invalidate();
                }
                if (document1.ScrollY(moveY) == true)
                {
                    panel1.Invalidate();
                }
            }
        }
        
        private void FormScrollBar_KeyDown(object sender, KeyEventArgs e)
        {
            Debug.WriteLine(String.Format("keydown:{0}", e.KeyValue));

            switch((Keys)e.KeyValue)
            {
                case Keys.Left:
                    break;
                case Keys.Up:
                    ZoomDown();
                    break;
                case Keys.Right:
                    break;
                case Keys.Down:
                    ZoomUp();
                    break;
                case Keys.Control:
                    break;
                case Keys.PageDown:
                    if (document1.ScrollDown())
                    {
                        panel1.Invalidate();
                    }
                    break;
                case Keys.PageUp:
                    if (document1.ScrollUp())
                    {
                        panel1.Invalidate();
                    }
                    break;
            }
        }
        #endregion イベント

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
        // Consts
        //
        #region Consts

        private int topMarginX = 100;
        private int topMarginY = 100;
        private int bottomMarginX = 100;
        private int bottomMarginY = 100;

        private int intervalX = 100;
        private int intervalY = 100;

        #endregion

        //
        // Properties
        //
        #region Properties
        SBInfo sbV, sbH;

        private Image image;                // LogView描画先のImage

        HScrollBar scrollBarH;
        VScrollBar scrollBarV;

        float zoomRate = 1.0f;

        #endregion

        public Document1(int width, int height, HScrollBar scrollBarH, VScrollBar scrollBarV)
        {

            this.scrollBarH = scrollBarH;
            this.scrollBarV = scrollBarV;
            sbV = new SBInfo();
            sbV.Init(0, 10000, height - (topMarginY + bottomMarginY));
            sbH = new SBInfo();
            sbH.Init(0, 10000, width - (topMarginX + bottomMarginX));

            Resize(width, height);

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

            scrollBarH.LargeChange = sbH.large = width - (topMarginX + bottomMarginX);
            scrollBarV.LargeChange = sbV.large = height - (topMarginY + bottomMarginY);

            scrollBarH.LargeChange = sbH.large;
            scrollBarH.Maximum = sbH.max;

            scrollBarV.LargeChange = sbV.large;
            scrollBarV.Maximum = sbV.max;

        }

        public void setZoomRate(float zoomRate)
        {
            this.zoomRate = zoomRate;
        }

        public bool ScrollX(int delta)
        {
            int oldValue = sbH.value;

            sbH.value += delta;

            if (sbH.value < 0)
            {
                sbH.value = 0;
            }
            if (sbH.value > sbH.max - sbH.large)
            {
                sbH.value = sbH.max - sbH.large;
            }

            if (oldValue != sbH.value)
            {
                scrollBarH.Value = sbH.value;
                return true;
            }
            return false;
        }

        public bool ScrollY(int delta)
        {
            int oldValue = sbV.value;

            sbV.value += delta;

            if (sbV.value < 0)
            {
                sbV.value = 0;
            }
            if (sbV.value > sbV.max - sbV.large)
            {
                sbV.value = sbV.max - sbV.large;
            }
            if (oldValue != sbV.value)
            {
                scrollBarV.Value = sbV.value;
                return true;
            }
            return false;
        }

        public bool ScrollDown()
        {
            return ScrollY(sbV.large);
        }

        public bool ScrollUp()
        {
            return ScrollY(-sbV.large);
        }

        public void Draw(Graphics g)
        {
            using (Graphics g2 = Graphics.FromImage(image))
            {
                // clear background
                g2.FillRectangle(Brushes.Black, 0, 0, image.Width, image.Height);

                var font1 = new Font("Arial", 12);

                // テキスト表示
                int x0 = 10;
                int y0 = 10;

                g2.DrawString(String.Format("value {0},{1} max {2},{3}", sbH.value, sbV.value, sbH.max, sbV.max), font1, Brushes.White, x0, y0);
                y0 += 25;
                g2.DrawString(String.Format("large {0}", sbH.large, sbV.large), font1, Brushes.White, x0, y0);
                y0 += 25;
                g2.DrawString(String.Format("zoomRate {0}", zoomRate), font1, Brushes.White, x0, y0);
                y0 += 25;

                // クリッピング設定
                Rectangle rect1 = new Rectangle(topMarginX, topMarginY, sbH.large, sbV.large);
                g2.SetClip(rect1);

                g2.FillRectangle(Brushes.DarkRed, rect1);

                int x = topMarginX + sbH.value - (sbH.value % intervalX);
                int y = topMarginY + sbV.value - (sbV.value % intervalY);

                // 横のライン
                bool drawStr = true;
                while ( y < sbV.value + sbV.large + intervalY)
                {
                    if (drawStr)
                    {
                        //drawStr = false;
                        g2.DrawString(String.Format("{0}ms", (y - topMarginY) / zoomRate ), font1, Brushes.Yellow, topMarginX, y - sbV.value);
                    }
                    g2.DrawLine(Pens.White, topMarginX, y - sbV.value, image.Width - bottomMarginX, y - sbV.value);
                    y += intervalY;
                }
                // 縦のライン
                drawStr = true;
                while ( x < sbH.value + sbH.large + intervalX)
                {
                    if (drawStr)
                    {
                        //drawStr = false;
                        g2.DrawString(String.Format("{0}ms", (x - topMarginX) / zoomRate), font1, Brushes.Yellow, x - sbH.value, topMarginY);
                    }
                    g2.DrawLine(Pens.White, x - sbH.value, topMarginY, x - sbH.value, image.Height - bottomMarginY);
                    x += intervalX;
                }

            }
            g.DrawImage(image, 0, 0);
        }

        // Document側で表示情報を更新
        public void UpdateSBV(int value)
        {
            // スクロールバーに反映
            sbV.value = value;

            scrollBarV.Value = value;
            Debug.WriteLine(String.Format("sbV value:{0} max:{1} large:{2}", scrollBarV.Value, scrollBarV.Maximum, scrollBarV.LargeChange));
        }

        // Document側で表示情報を更新
        public void UpdateSBH(int value)
        {
            // スクロールバーに反映
            sbH.value = value;

            scrollBarH.Value = value;
            Debug.WriteLine(String.Format("sbH value:{0} max:{1} large:{2}", scrollBarH.Value, scrollBarH.Maximum, scrollBarH.LargeChange));
        }


        // zoom::
        // 拡大
        public bool ZoomUp()
        {
            zoomRate *= 1.2f;
            ChangeZoomRate();
            return true;
        }

        // 縮小
        public bool ZoomDown()
        {
            zoomRate *= 0.8f;
            ChangeZoomRate();
            return true;
        }

        // 拡大率が変化したときの処理
        private void ChangeZoomRate()
        {
            // 拡大したときの動作としてスクロールバーのmaxが変化するパターンと
            // LargeChangeが変化するパターンがあるが、ここではmaxが変換するパターンを採用
            scrollBarH.Maximum = sbV.max = (int)(10000.0f * zoomRate);
        }
    }


    public class DoubleBufferingPanel : System.Windows.Forms.Panel
    {
        public DoubleBufferingPanel()
        {
            this.DoubleBuffered = true;
        }
    }
}
