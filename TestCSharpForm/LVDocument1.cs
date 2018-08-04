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

    //public struct SBInfo
    //{
    //    public int max;
    //    public int large;
    //    public int value;

    //    public void Init(int value, int max, int large)
    //    {
    //        this.value = value;
    //        this.max = max;
    //        this.large = large;
    //    }
    //}


    class LVDocument1
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
        
        private double topTime;

        public double TopTime
        {
            get { return topTime; }
            set { topTime = value; }
        }

        private double endTime;

        public double EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }

        private double dispTopTime;

        public double DispTopTime
        {
            get { return dispTopTime; }
            set { dispTopTime = value; }
        }

        private double dispEndTime;

        public double DispEndTime
        {
            get { return dispEndTime; }
            set { dispEndTime = value; }
        }


        private float zoomRate = 1.0f;

        public float ZoomRate
        {
            get { return zoomRate; }
            set { zoomRate = value; }
        }

        private LogViewPixTime pixTime;


        private Image image;                // LogView描画先のImage

        HScrollBar scrollBarH;
        VScrollBar scrollBarV;

        #endregion

        public LVDocument1(int width, int height, HScrollBar scrollBarH, VScrollBar scrollBarV)
        {

            this.scrollBarH = scrollBarH;
            this.scrollBarV = scrollBarV;

            pixTime = new LogViewPixTime();

            topTime = 0.0;
            endTime = 10.0;
            dispTopTime = 0.0;
            dispEndTime = GetDispEndTime();
            
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

            scrollBarH.LargeChange = width - (topMarginX + bottomMarginX);
            scrollBarV.LargeChange = height - (topMarginY + bottomMarginY);

            scrollBarH.Maximum = 1000;
            scrollBarV.Maximum = (int)pixTime.timeToPix(endTime);
        }

        /**
         * 表示領域の端の時間
         * 画面に表示されるViewの一番端のピクセルの時間
         */
        private double GetDispEndTime()
        {
            return dispTopTime + pixTime.pixToTime(scrollBarV.LargeChange);
        }

        public void setZoomRate(float zoomRate)
        {
            this.zoomRate = zoomRate;
        }

        private void UpdateScrollY()
        {
            dispTopTime = topTime + pixTime.pixToTime(scrollBarV.Value);
            dispEndTime = GetDispEndTime();
            Debug.WriteLine("dispTopTime:{0} {1}", dispTopTime, scrollBarV.Value);
        }

        public bool ScrollX(int delta)
        {
            return ScrollXY(scrollBarH, delta);
        }

        public bool ScrollY(int delta)
        {
            bool ret = ScrollXY(scrollBarV, delta);
            UpdateScrollY();
            return ret;
        }

        public bool ScrollXY(ScrollBar sb, int delta)
        {
            int oldValue = sb.Value;

            if (sb.Value + delta > sb.Maximum)
            {
                sb.Value = sb.Maximum;
            }
            else if (sb.Value + delta < 0)
            {
                sb.Value = 0;
            }
            else
            {
                sb.Value += delta;
            }
            Debug.WriteLine("scrollXY");

            if (oldValue != sb.Value)
            {   
                return true;
            }
            return false;
        }

        public bool ScrollDown()
        {
            return ScrollY(scrollBarV.LargeChange);
        }

        public bool ScrollUp()
        {
            return ScrollY(-scrollBarV.LargeChange);
        }

        public void Draw(Graphics g)
        {
            using (Graphics g2 = Graphics.FromImage(image))
            {
                UpdateSBH(scrollBarH.Value);
                UpdateSBV(scrollBarV.Value);

                // clear background
                g2.FillRectangle(Brushes.Black, 0, 0, image.Width, image.Height);

                var font1 = new Font("Arial", 12);

                // テキスト表示
                int x0 = 10;
                int y0 = 10;

                g2.DrawString(String.Format("dispTopTime:{0} dispEndTime:{1}", dispTopTime, dispEndTime), font1, Brushes.White, x0, y0);
                y0 += 20;
                g2.DrawString(String.Format("[sbH] value:{0},large:{1} max:{2}", scrollBarH.Value, scrollBarV.LargeChange, scrollBarV.Maximum),
                    font1, Brushes.White, x0, y0);
                y0 += 20;
                g2.DrawString(String.Format("[sbV] value:{0},large:{1} max:{2}", scrollBarV.Value, scrollBarV.LargeChange, scrollBarV.Maximum),
                    font1, Brushes.White, x0, y0);
                y0 += 20;
                g2.DrawString(String.Format("zoomRate:{0} pixTime.zoom:{1}", zoomRate, pixTime.Val), font1, Brushes.White, x0, y0);
                y0 += 20;

                // クリッピング設定
                Rectangle rect1 = new Rectangle(topMarginX, topMarginY, scrollBarH.LargeChange, scrollBarV.LargeChange);
                g2.SetClip(rect1);

                g2.FillRectangle(Brushes.DarkRed, rect1);

                int x = -(scrollBarH.Value % intervalX);
                int y = -(scrollBarV.Value % intervalY);

                // 横のライン
                while (y < scrollBarV.LargeChange)
                {
                    double time = dispTopTime + pixTime.pixToTime(y);
                    string str = String.Format("{0}s", time);
                    g2.DrawString(str,
                        font1, Brushes.Yellow, topMarginX, topMarginY + y);

                    if (str.Length > 4)
                    {
                        Debug.WriteLine("hoge");
                    }
                    
                    g2.DrawLine(Pens.White, topMarginX, topMarginY + y,
                        image.Width - bottomMarginX, topMarginY + y);
                    y += intervalY;
                }
                // 縦のライン
                while (x < scrollBarH.LargeChange)
                {
                    g2.DrawString(String.Format("{0}", topMarginX + x),
                        font1, Brushes.Yellow, x - scrollBarH.Value, topMarginY);
                    g2.DrawLine(Pens.White, topMarginX + x, topMarginY,
                        topMarginX + x, image.Height - bottomMarginY);
                    x += intervalX;
                }

            }
            g.DrawImage(image, 0, 0);
        }

        // Document側で表示情報を更新
        public void UpdateSBV(int value)
        {
            // スクロールバーに反映
            scrollBarV.Value = value;
            Debug.WriteLine("value:{0}", scrollBarV.Value);
            UpdateScrollY();
        }

        // Document側で表示情報を更新
        public void UpdateSBH(int value)
        {
            // スクロールバーに反映
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
            scrollBarH.Maximum = (int)(10000.0f * zoomRate);
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
