using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace TestCSharpForm
{
    class LVDocument1
    {
        //
        // Consts
        //
        #region Consts

        private int topMarginX = 140;
        private int topMarginY = 120;
        private int bottomMarginX = 100;
        private int bottomMarginY = 100;

        private int intervalX = 100;
        private int intervalY = 100;

        #endregion

        //
        // Properties
        //
        #region Properties

        // 表示の方向
        private int direction;

        public int Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        // 全体の先頭の時間
        private double topTime;

        public double TopTime
        {
            get { return topTime; }
            set { topTime = value; }
        }

        // 全体の最後の時間
        private double endTime;

        public double EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }

        // 表示範囲の先頭の時間
        private double dispTopTime;

        public double DispTopTime
        {
            get { return dispTopTime; }
            set { dispTopTime = value; }
        }

        // 表示範囲の末尾の時間
        private double dispEndTime;

        public double DispEndTime
        {
            get { return dispEndTime; }
            set { dispEndTime = value; }
        }

        // 全体のズーム率
        private float zoomRate = 1.0f;

        public float ZoomRate
        {
            get { return zoomRate; }
            set { zoomRate = value; }
        }

        // 1ピクセル当たりの時間
        private LogViewPixTime pixTime;


        private Image image;                // LogView描画先のImage

        private HScrollBar scrollBarH;
        private VScrollBar scrollBarV;

        private ScrollBar scrollBar1;       // 縦モードのときは水平スクロールバー。横モードのときは垂直スクロールバー
        private ScrollBar scrollBar2;       // 縦モードのときは垂直スクロールバー。横モードのときは水平スクロールバー

        #endregion

        public LVDocument1(int width, int height, int direction, HScrollBar scrollBarH, VScrollBar scrollBarV)
        {
            pixTime = new LogViewPixTime();

            this.scrollBarH = scrollBarH;
            this.scrollBarV = scrollBarV;

            this.direction = direction;
            if (direction == 0)
            {
                scrollBar1 = scrollBarH;
                scrollBar2 = scrollBarV;
            }
            else
            {
                scrollBar1 = scrollBarV;
                scrollBar2 = scrollBarH;
            }

            topTime = 0.0;
            endTime = 10.0;
            dispTopTime = 0.0;
            dispEndTime = GetDispEndTime();

            scrollBar1.LargeChange = width - (topMarginX + bottomMarginX);
            scrollBar2.LargeChange = height - (topMarginY + bottomMarginY);

            scrollBar1.Maximum = 1000;
            scrollBar2.Maximum = (int)pixTime.timeToPix(endTime);

            Resize(width, height);

        }

        // 
        // Methods 
        //

        public void SetDirection(int direction)
        {
            this.direction = direction;

            if (direction == 0)
            {
                scrollBar1 = scrollBarH;
                scrollBar2 = scrollBarV;
            }
            else
            {
                scrollBar1 = scrollBarV;
                scrollBar2 = scrollBarH;
            }

            scrollBarH.LargeChange = image.Width - (topMarginX + bottomMarginX);
            scrollBarV.LargeChange = image.Height - (topMarginY + bottomMarginY);

            scrollBar1.Maximum = 1000;
            scrollBar2.Maximum = (int)pixTime.timeToPix(endTime);

        }
        public void Resize(int width, int height)
        {
            if (image != null)
            {
                image.Dispose();
            }

            image = new Bitmap(width, height);

            SetDirection(direction);
        }

        /**
         * 表示領域の端の時間
         * 画面に表示されるViewの一番端のピクセルの時間
         */
        private double GetDispEndTime()
        {
            return dispTopTime + pixTime.pixToTime(scrollBar2.LargeChange) / zoomRate;
        }

        public void setZoomRate(float zoomRate)
        {
            this.zoomRate = zoomRate;
        }

        public int getZoomValue(int value)
        {
            return (int)(value * zoomRate);
        }

        public float getZoomValue(float value)
        {
            return (float)(value * zoomRate);
        }

        // scroll::
        private void UpdateScrollY()
        {
            dispTopTime = topTime + pixTime.pixToTime(scrollBar2.Value) / zoomRate;
            dispEndTime = GetDispEndTime();
        }

        public bool ScrollX(int delta)
        {
            return ScrollXY(scrollBarH, delta);
        }

        public bool ScrollY(int delta)
        {
            return ScrollXY(scrollBarV, delta);
        }

        public bool ScrollXY(ScrollBar sb, int delta)
        {
            int oldValue = sb.Value;

            if (sb.Value + delta > sb.Maximum - sb.LargeChange)
            {
                sb.Value = sb.Maximum - sb.LargeChange;
            }
            else if (sb.Value + delta < 0)
            {
                sb.Value = 0;
            }
            else
            {
                sb.Value += delta;
            }
            
            if (oldValue != sb.Value)
            {   
                return true;
            }
            return false;
        }

        public bool ScrollDown()
        {
            return ScrollY(scrollBar2.LargeChange);
        }

        public bool ScrollUp()
        {
            return ScrollY(-scrollBar2.LargeChange);
        }

        public void Draw(Graphics g)
        {
            using (Graphics g2 = Graphics.FromImage(image))
            {
                if (direction == 0)
                {
                    DrawV(g2);
                }
                else
                {
                    DrawH(g2);
                }
            }
            g.DrawImage(image, 0, 0);
        }

        private void DrawH(Graphics g)
        {
            UpdateScrollY();
            
            // clear background
            g.FillRectangle(Brushes.Black, 0, 0, image.Width, image.Height);

            var font1 = new Font("Arial", 10);

            // テキスト表示
            int x0 = 10;
            int y0 = 10;

            g.DrawString(String.Format("dispTopTime:{0} dispEndTime:{1}", dispTopTime, dispEndTime), font1, Brushes.White, x0, y0);
            y0 += 20;
            g.DrawString(String.Format("[sbH] value:{0},large:{1} max:{2}", scrollBarH.Value, scrollBarH.LargeChange, scrollBarH.Maximum),
                font1, Brushes.White, x0, y0);
            y0 += 20;
            g.DrawString(String.Format("[sbV] value:{0},large:{1} max:{2}", scrollBarV.Value, scrollBarV.LargeChange, scrollBarV.Maximum),
                font1, Brushes.White, x0, y0);
            y0 += 20;
            g.DrawString(String.Format("zoomRate:{0:0.######} pixTime.zoom:{1:0.########}", zoomRate, pixTime.Val), font1, Brushes.White, x0, y0);
            y0 += 20;

            // クリッピング設定
            Rectangle rect1 = new Rectangle(topMarginX, topMarginY, scrollBarH.LargeChange, scrollBarV.LargeChange);
            g.SetClip(rect1);

            g.FillRectangle(Brushes.DarkRed, rect1);

            int x = -(scrollBarH.Value % intervalX);
            int y = -(scrollBarV.Value % intervalY);
            int intervalX2 = (int)(intervalX * zoomRate);
            int intervalY2 = (int)(intervalY * zoomRate);

            //--------------------------------
            // ライン
            //--------------------------------
            var font2 = new Font("Arial", getZoomValue(10));
            // 横のライン
            while (y < scrollBarV.LargeChange)
            {
                g.DrawLine(Pens.White, topMarginX, topMarginY + y,
                    image.Width - bottomMarginX, topMarginY + y);
                y += intervalY2;
            }
            // 縦のライン
            int offsetX = scrollBarV.Value;
            while (x < scrollBarH.LargeChange)
            {
                g.DrawLine(Pens.White, topMarginX + x, topMarginY,
                    topMarginX + x, image.Height - bottomMarginY);
                x += intervalX2;
            }

            //--------------------------------
            // 文字列
            //--------------------------------
            x = -(scrollBarH.Value % intervalX2);
            y = -(scrollBarV.Value % intervalY2);

            StringFormat sf1 = new StringFormat();
            sf1.Alignment = StringAlignment.Far;
            sf1.LineAlignment = StringAlignment.Center;

            // set cliping
            Rectangle rect2 = new Rectangle(0, topMarginY - 10, image.Width, image.Height - topMarginY + 10);
            g.SetClip(rect2);

            // 横のテキスト
            while (y < scrollBarV.LargeChange)
            {
                g.DrawString(String.Format("{0}", y / zoomRate + offsetX),
                    font2, Brushes.Yellow, topMarginX - 5, topMarginY + y, sf1);
                y += intervalY2;
            }

            // 縦のテキスト
            Rectangle rect3 = new Rectangle(topMarginX - 10, 0, scrollBarH.LargeChange + 10, image.Height - topMarginY);
            g.SetClip(rect3);
            sf1.Alignment = StringAlignment.Center;
            sf1.LineAlignment = StringAlignment.Far;

            while (x < scrollBarH.LargeChange + 20)
            {
                double time = dispTopTime + pixTime.pixToTime(x) / zoomRate;
                g.DrawString(String.Format("{0:0.######}s", time),
                    font2, Brushes.Yellow, topMarginX + x, topMarginY - 5, sf1);
                x += intervalX2;
            }
        }

        private void DrawV(Graphics g)
        {
            UpdateScrollY();
            
            // clear background
            g.FillRectangle(Brushes.Black, 0, 0, image.Width, image.Height);

            var font1 = new Font("Arial", 10);

            // テキスト表示
            int x0 = 10;
            int y0 = 10;

            g.DrawString(String.Format("dispTopTime:{0} dispEndTime:{1}", dispTopTime, dispEndTime), font1, Brushes.White, x0, y0);
            y0 += 20;
            g.DrawString(String.Format("[sbH] value:{0},large:{1} max:{2}", scrollBarH.Value, scrollBarH.LargeChange, scrollBarH.Maximum),
                font1, Brushes.White, x0, y0);
            y0 += 20;
            g.DrawString(String.Format("[sbV] value:{0},large:{1} max:{2}", scrollBarV.Value, scrollBarV.LargeChange, scrollBarV.Maximum),
                font1, Brushes.White, x0, y0);
            y0 += 20;
            g.DrawString(String.Format("zoomRate:{0:0.######} pixTime.zoom:{1:0.########}", zoomRate, pixTime.Val), font1, Brushes.White, x0, y0);
            y0 += 20;

            // クリッピング設定
            Rectangle rect1 = new Rectangle(topMarginX, topMarginY, scrollBarH.LargeChange, scrollBarV.LargeChange);
            g.SetClip(rect1);

            g.FillRectangle(Brushes.DarkRed, rect1);

            int x = -(scrollBarH.Value % intervalX);
            int y = -(scrollBarV.Value % intervalY);
            int intervalX2 = (int)(intervalX * zoomRate);
            int intervalY2 = (int)(intervalY * zoomRate);

            //--------------------------------
            // ライン
            //--------------------------------
            var font2 = new Font("Arial", getZoomValue(10));
            // 横のライン
            while (y < scrollBarV.LargeChange)
            {
                g.DrawLine(Pens.White, topMarginX, topMarginY + y,
                    image.Width - bottomMarginX, topMarginY + y);
                y += intervalY2;
            }
            // 縦のライン
            int offsetX = scrollBarH.Value;
            while (x < scrollBarH.LargeChange)
            {
                g.DrawLine(Pens.White, topMarginX + x, topMarginY,
                    topMarginX + x, image.Height - bottomMarginY);
                x += intervalX2;
            }

            //--------------------------------
            // 文字列
            //--------------------------------
            x = -(scrollBarH.Value % intervalX2);
            y = -(scrollBarV.Value % intervalY2);

            StringFormat sf1 = new StringFormat();
            sf1.Alignment = StringAlignment.Far;
            sf1.LineAlignment = StringAlignment.Center;

            // set cliping
            Rectangle rect2 = new Rectangle(0, topMarginY - 10, image.Width, image.Height - topMarginY + 10);
            g.SetClip(rect2);

            // 横のテキスト
            while (y < scrollBarV.LargeChange)
            {
                double time = dispTopTime + pixTime.pixToTime(y) / zoomRate;
                g.DrawString(String.Format("{0:0.######}s", time),
                    font2, Brushes.Yellow, topMarginX - 5, topMarginY + y, sf1);
                y += intervalY2;
            }

            // 縦のテキスト
            Rectangle rect3 = new Rectangle(topMarginX - 10, 0, scrollBarH.LargeChange + 10, image.Height - topMarginY);
            g.SetClip(rect3);
            sf1.Alignment = StringAlignment.Center;
            sf1.LineAlignment = StringAlignment.Far;
            while (x < scrollBarH.LargeChange + 20)
            {
                g.DrawString(String.Format("{0}", x / zoomRate + offsetX),
                    font2, Brushes.Yellow, topMarginX + x, topMarginY - 5, sf1);
                x += intervalX2;
            }

        }

        // Document側で表示情報を更新
        public void UpdateSBV(int value)
        {
            // スクロールバーに反映
            scrollBarV.Value = value;
            UpdateScrollY();
        }

        // Document側で表示情報を更新
        public void UpdateSBH(int value)
        {
            // スクロールバーに反映
            scrollBarH.Value = value;
        }

        // zoom::

        // １秒当たりのピクセル数のズーム
        // 拡大
        public bool PixTimeZoomUp()
        {
            pixTime.ZoomIn();
            ChangeZoomRate();
            return true;
        }

        // 縮小
        public bool PixTimeZoomDown()
        {
            pixTime.ZoomOut();
            ChangeZoomRate();
            return true;
        }

        public bool ZoomUp()
        {
            zoomRate *= 2.0f;
            ChangeZoomRate();
            return true;
        }

        // 縮小
        public bool ZoomDown()
        {
            zoomRate *= 0.5f;
            ChangeZoomRate();
            return true;
        }

        // 拡大率が変化したときの処理
        private void ChangeZoomRate()
        {
            // 拡大したときの動作としてスクロールバーのmaxが変化するパターンと
            // LargeChangeが変化するパターンがあるが、ここではmaxが変換するパターンを採用
            scrollBarV.Maximum = (int)((endTime - topTime) * zoomRate / pixTime.Val);
        }
    }

    /**
     * LogViewの1pixelあたりの表示時間
     */
    class LogViewPixTime
    {
        //
        // Properties
        //

        // 1pixelあたりの時間
        private double val;

        public double Val
        {
            get { return val; }
            set { val = value; }
        }

        //
        // Constructor
        //
        public LogViewPixTime()
        {
            // 1000pix = 1sec -> 1pix = 0.001sec = 1ms
            val = 0.001;
        }

        //
        // Medhots
        //
        /**
         * ズームイン
         * 表示される領域が狭くなる(1pixあたりの時間が小さくなる)
         * 1pix = 0.01秒 -> 1pix = 0.005秒
         */
        public void ZoomIn()
        {
            val *= 0.8f;
        }

        /**
         * ズームアウト
         * 表示される領域が広くなる(1pixあたりの時間が大きくなる)
         * 1pix = 0.01秒 -> 1pix = 0.02秒
         */
        public void ZoomOut()
        {
            val *= 1.2f;
        }

        public override string ToString()
        {
            //return String.Format("1pix={0}sec", val);
            return String.Format("1sec={0}pix", val);
        }

        /**
         * 時間を現在のズーム率でpixelに変換する
         * @input time : 変換元の時間(sec)
         * @output 変換後のpixel数
         */
        public UInt64 timeToPix(double time)
        {
            // 1秒あたりのpixel数を取得してから時間(sec)を書ける
            return (UInt64)((1.0 / val) * time);
        }

        /**
         * pixelを現在のズーム率で時間に変換する
         * @input pix : 変換元のpixel数
         * @output 変換後の時間(sec)
         */
        public double pixToTime(int pix)
        {
            return pix * val;
        }
    }
}
