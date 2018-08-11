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
        private ZoomRate zoomRate;

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
            zoomRate = new ZoomRate();

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
            return dispTopTime + pixTime.pixToTime(scrollBar2.LargeChange) / zoomRate.Value;
        }

        public void setZoomRate(ZoomRate zoomRate)
        {
            this.zoomRate = zoomRate;
        }

        // 指定の整数値にズーム率をかけた結果を取得
        public int getZoomValue(int value)
        {
            return (int)(value * zoomRate.Value);
        }

        // 指定の浮動小数値にズーム率をかけた結果を取得
        public float getZoomValue(float value)
        {
            return (float)(value * zoomRate.Value);
        }

        // scroll::
        private void UpdateScrollY()
        {
            dispTopTime = topTime + pixTime.pixToTime(scrollBar2.Value) / zoomRate.Value;
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
                // clear background
                g2.FillRectangle(Brushes.Black, 0, 0, image.Width, image.Height);

                var font1 = new Font("Arial", 10);

                // テキスト表示
                int x0 = 10;
                int y0 = 10;

                g2.DrawString(String.Format("dispTopTime:{0} dispEndTime:{1}", dispTopTime, dispEndTime), font1, Brushes.White, x0, y0);
                y0 += 20;
                g2.DrawString(String.Format("[sbH] value:{0},large:{1} max:{2}", scrollBarH.Value, scrollBarH.LargeChange, scrollBarH.Maximum),
                    font1, Brushes.White, x0, y0);
                y0 += 20;
                g2.DrawString(String.Format("[sbV] value:{0},large:{1} max:{2}", scrollBarV.Value, scrollBarV.LargeChange, scrollBarV.Maximum),
                    font1, Brushes.White, x0, y0);
                y0 += 20;
                g2.DrawString(String.Format("zoomRate:{0:0.######} pixTime.zoom:{1:0.########}", zoomRate, pixTime.Val), font1, Brushes.White, x0, y0);
                y0 += 20;

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

        // 横にログが進んでいく描画モード
        private void DrawH(Graphics g)
        {
            UpdateScrollY();
            
            // クリッピング設定
            Rectangle rect1 = new Rectangle(topMarginX, topMarginY, scrollBarH.LargeChange, scrollBarV.LargeChange);
            g.SetClip(rect1);

            g.FillRectangle(Brushes.DarkRed, rect1);

            int x = -(scrollBarH.Value % intervalX);
            int y = -(scrollBarV.Value % intervalY);
            int intervalX2 = (int)(intervalX * zoomRate.Value);
            int intervalY2 = (int)(intervalY * zoomRate.Value);

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
                g.DrawString(String.Format("{0}", (y + offsetX) / zoomRate.Value),
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
                double time = dispTopTime + pixTime.pixToTime(x) / zoomRate.Value;
                g.DrawString(String.Format("{0:0.######}s", time),
                    font2, Brushes.Yellow, topMarginX + x, topMarginY - 5, sf1);
                x += intervalX2;
            }
        }

        // 縦(下)にログが進んでいくモード
        private void DrawV(Graphics g)
        {
            UpdateScrollY();
            
            // クリッピング設定
            Rectangle rect1 = new Rectangle(topMarginX, topMarginY, scrollBarH.LargeChange, scrollBarV.LargeChange);
            g.SetClip(rect1);

            g.FillRectangle(Brushes.DarkRed, rect1);

            int x = -(scrollBarH.Value % intervalX);
            int y = -(scrollBarV.Value % intervalY);
            int intervalX2 = (int)(intervalX * zoomRate.Value);
            int intervalY2 = (int)(intervalY * zoomRate.Value);

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
                double time = dispTopTime + pixTime.pixToTime(y) / zoomRate.Value;
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
                g.DrawString(String.Format("{0}", (x + offsetX) / zoomRate.Value),
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

        // 全体のズーム率
        // 拡大
        public bool ZoomUp()
        {
            zoomRate.ZoomIn();
            ChangeZoomRate();
            return true;
        }

        // 縮小
        public bool ZoomDown()
        {
            zoomRate.ZoomOut();
            ChangeZoomRate();
            return true;
        }

        // 拡大率が変化したときの処理
        private void ChangeZoomRate()
        {
            // 拡大したときの動作としてスクロールバーのmaxが変化するパターンと
            // LargeChangeが変化するパターンがあるが、ここではmaxが変換するパターンを採用
            scrollBarH.Maximum = (int)(1000 * zoomRate.Value);
            scrollBarV.Maximum = (int)((endTime - topTime) * zoomRate.Value / pixTime.Val);

            if (scrollBarH.LargeChange > scrollBarH.Maximum)
            {
                scrollBarH.Enabled = false;
                scrollBarH.LargeChange = scrollBarH.Maximum;
            }
            else
            {
                scrollBarH.Enabled = true;
            }

            if (scrollBarV.LargeChange > scrollBarV.Maximum)
            {
                scrollBarH.Enabled = false;
                scrollBarV.LargeChange = scrollBarV.Maximum;
            }
            else
            {
                scrollBarH.Enabled = true;
            }
        }
    }

    /**
     * LogViewの1pixelあたりの表示時間
     */
    class LogViewPixTime
    {
        struct SPixTime
        {
            //public string unitName;    // 時間の単位
            public EUnitType unitType;  // 時間の単位
            public double pixTime;     // 1ピクセル当たりの時間
            public double calcTime;    // unitTime計算用の掛け算の値
        }

        // 時間の単位
        enum EUnitType : byte
        {
            Nano,       // ナノ秒  1/1000000000
            Micro,      // マイクロ秒 1/1000000
            Milli,      // ミリ秒 1/1000
            Second      // 秒
        }

        enum EPixTime : byte
        {
            E1_0N,     // 1pix = 1nano s
            E1_5N,
            E2_0N,
            E3_0N,
            E5_0N,
            E7_5N,
            E10N,
            E15N,
            E20N,
            E30N,
            E50N,
            E75N,
            E100N,
            E150N,
            E200N,
            E300N,
            E500N,
            E750N,
            E1_0U,       // 1pix = 1 micro s
            E1_5U,
            E2_0U,
            E3_0U,
            E5_0U,
            E7_5U,
            E10U,
            E15U,
            E20U,
            E30U,
            E50U,
            E75U,
            E100U,
            E150U,
            E200U,
            E300U,
            E500U,
            E750U,
            E1_0M,      // 1pix = 1 milli second
            E1_5M,
            E2_0M,
            E3_0M,
            E5_0M,
            E7_5M,
            E10M,       
            E15M,
            E20M,
            E30M,
            E50M,
            E75M,
            E100M,
            E150M,
            E200M,
            E300M,
            E500M,
            E750M,
            E1_0S,      // 1pix = 1 second
            E1_5S,
            E2_0S,
            E3_0S,
            E5_0S,
            E7_5S,
            E10S,
            E15S,
            E20S,
            E30S,
            E50S,
            E75S,
            E100S,
            E150S,
            E200S,
            E300S,
            E500S,
            E750S,
            E1000S
        }

        static SPixTime[] pixTimeTbl = new SPixTime[]
        {
            new SPixTime(){ unitType=EUnitType.Nano, pixTime = 0.000000001, calcTime=0.1 },
            new SPixTime(){ unitType=EUnitType.Nano, pixTime = 0.0000000015 },
            new SPixTime(){ unitType=EUnitType.Nano, pixTime = 0.000000002 },
            new SPixTime(){ unitType=EUnitType.Nano, pixTime = 0.000000003 },
            new SPixTime(){ unitType=EUnitType.Nano, pixTime = 0.000000005 },
            new SPixTime(){ unitType=EUnitType.Nano, pixTime = 0.0000000075 },

            new SPixTime(){ unitType=EUnitType.Nano, pixTime = 0.00000001 },
            new SPixTime(){ unitType=EUnitType.Nano, pixTime = 0.000000015 },
            new SPixTime(){ unitType=EUnitType.Nano, pixTime = 0.00000002 },
            new SPixTime(){ unitType=EUnitType.Nano, pixTime = 0.00000003 },
            new SPixTime(){ unitType=EUnitType.Nano, pixTime = 0.00000005 },
            new SPixTime(){ unitType=EUnitType.Nano, pixTime = 0.000000075 },

            new SPixTime(){ unitType=EUnitType.Nano, pixTime = 0.0000001 },
            new SPixTime(){ unitType=EUnitType.Nano, pixTime = 0.00000015 },
            new SPixTime(){ unitType=EUnitType.Nano, pixTime = 0.0000002 },
            new SPixTime(){ unitType=EUnitType.Nano, pixTime = 0.0000003 },
            new SPixTime(){ unitType=EUnitType.Nano, pixTime = 0.0000005 },
            new SPixTime(){ unitType=EUnitType.Nano, pixTime = 0.00000075 },

            new SPixTime(){ unitType=EUnitType.Micro, pixTime = 0.000001 },
            new SPixTime(){ unitType=EUnitType.Micro, pixTime = 0.0000015 },
            new SPixTime(){ unitType=EUnitType.Micro, pixTime = 0.000002 },
            new SPixTime(){ unitType=EUnitType.Micro, pixTime = 0.000003 },
            new SPixTime(){ unitType=EUnitType.Micro, pixTime = 0.000005 },
            new SPixTime(){ unitType=EUnitType.Micro, pixTime = 0.0000075 },

            new SPixTime(){ unitType=EUnitType.Micro, pixTime = 0.00001 },
            new SPixTime(){ unitType=EUnitType.Micro, pixTime = 0.000015 },
            new SPixTime(){ unitType=EUnitType.Micro, pixTime = 0.00002 },
            new SPixTime(){ unitType=EUnitType.Micro, pixTime = 0.00003 },
            new SPixTime(){ unitType=EUnitType.Micro, pixTime = 0.00005 },
            new SPixTime(){ unitType=EUnitType.Micro, pixTime = 0.000075 },

            new SPixTime(){ unitType=EUnitType.Micro, pixTime = 0.0001 },
            new SPixTime(){ unitType=EUnitType.Micro, pixTime = 0.00015 },
            new SPixTime(){ unitType=EUnitType.Micro, pixTime = 0.0002 },
            new SPixTime(){ unitType=EUnitType.Micro, pixTime = 0.0003 },
            new SPixTime(){ unitType=EUnitType.Micro, pixTime = 0.0005 },
            new SPixTime(){ unitType=EUnitType.Micro, pixTime = 0.00075 },

            new SPixTime(){ unitType=EUnitType.Milli, pixTime = 0.001 },
            new SPixTime(){ unitType=EUnitType.Milli, pixTime = 0.0015 },
            new SPixTime(){ unitType=EUnitType.Milli, pixTime = 0.002 },
            new SPixTime(){ unitType=EUnitType.Milli, pixTime = 0.003 },
            new SPixTime(){ unitType=EUnitType.Milli, pixTime = 0.005 },
            new SPixTime(){ unitType=EUnitType.Milli, pixTime = 0.0075 },

            new SPixTime(){ unitType=EUnitType.Milli, pixTime = 0.01 },
            new SPixTime(){ unitType=EUnitType.Milli, pixTime = 0.015 },
            new SPixTime(){ unitType=EUnitType.Milli, pixTime = 0.02 },
            new SPixTime(){ unitType=EUnitType.Milli, pixTime = 0.03 },
            new SPixTime(){ unitType=EUnitType.Milli, pixTime = 0.05 },
            new SPixTime(){ unitType=EUnitType.Milli, pixTime = 0.075 },

            new SPixTime(){ unitType=EUnitType.Milli, pixTime = 0.1 },
            new SPixTime(){ unitType=EUnitType.Milli, pixTime = 0.15 },
            new SPixTime(){ unitType=EUnitType.Milli, pixTime = 0.2 },
            new SPixTime(){ unitType=EUnitType.Milli, pixTime = 0.3 },
            new SPixTime(){ unitType=EUnitType.Milli, pixTime = 0.5 },
            new SPixTime(){ unitType=EUnitType.Milli, pixTime = 0.75 },

            new SPixTime(){ unitType=EUnitType.Second, pixTime = 1 },
            new SPixTime(){ unitType=EUnitType.Second, pixTime = 1.5 },
            new SPixTime(){ unitType=EUnitType.Second, pixTime = 2.0 },
            new SPixTime(){ unitType=EUnitType.Second, pixTime = 3.0 },
            new SPixTime(){ unitType=EUnitType.Second, pixTime = 5.0 },
            new SPixTime(){ unitType=EUnitType.Second, pixTime = 7.5 },

            new SPixTime(){ unitType=EUnitType.Second, pixTime = 10 },
            new SPixTime(){ unitType=EUnitType.Second, pixTime = 15 },
            new SPixTime(){ unitType=EUnitType.Second, pixTime = 20 },
            new SPixTime(){ unitType=EUnitType.Second, pixTime = 30 },
            new SPixTime(){ unitType=EUnitType.Second, pixTime = 50 },
            new SPixTime(){ unitType=EUnitType.Second, pixTime = 75 },

            new SPixTime(){ unitType=EUnitType.Second, pixTime = 100 },
            new SPixTime(){ unitType=EUnitType.Second, pixTime = 150 },
            new SPixTime(){ unitType=EUnitType.Second, pixTime = 200 },
            new SPixTime(){ unitType=EUnitType.Second, pixTime = 300 },
            new SPixTime(){ unitType=EUnitType.Second, pixTime = 500 },
            new SPixTime(){ unitType=EUnitType.Second, pixTime = 750 },

            new SPixTime(){ unitType=EUnitType.Second, pixTime = 1000 },
        };

        //
        // Properties
        //

        private EPixTime pixTime;

        // 1pixelあたりの時間
        public double Val
        {
            get {
                return pixTimeTbl[(byte)pixTime].pixTime;
            }
        }

        //
        // Constructor
        //
        public LogViewPixTime()
        {
            // 1000pix = 1sec -> 1pix = 0.001sec = 1ms
            pixTime = EPixTime.E1_0M;
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
            if(pixTime > EPixTime.E1_0N)
            {
                pixTime--;
            }
        }

        /**
         * ズームアウト
         * 表示される領域が広くなる(1pixあたりの時間が大きくなる)
         * 1pix = 0.01秒 -> 1pix = 0.02秒
         */
        public void ZoomOut()
        {
            if (pixTime < EPixTime.E1000S)
            {
                pixTime++;
            }
        }

        public override string ToString()
        {
            //return String.Format("1pix={0}sec", val);
            return String.Format("1sec={0}pix", Val);
        }

        /**
         * 時間を現在のズーム率でpixelに変換する
         * @input time : 変換元の時間(sec)
         * @output 変換後のpixel数
         */
        public UInt64 timeToPix(double time)
        {
            // 1秒あたりのpixel数を取得してから時間(sec)を書ける
            return (UInt64)((1.0 / Val) * time);
        }

        /**
         * pixelを現在のズーム率で時間に変換する
         * @input pix : 変換元のpixel数
         * @output 変換後の時間(sec)
         */
        public double pixToTime(int pix)
        {
            return pix * Val;
        }
    }

    /*
     * 拡大率を管理するクラス
     */
    class ZoomRate
    {
        enum EZoomRate : byte
        {
            E50P = 0,
            E67P,
            E75P,
            E80P,
            E90P,
            E100P,  // 100%
            E110P,
            E125P,
            E150P,
            E175P,
            E200P,
            E250P,
            E300P,
            E400P
        }
        private EZoomRate zoomRate;

        private float value;

        public float Value
        {
            get { return value; }
            set { value = value; }
        }

        //
        // Consts
        //
        private float[] eToV = new float[] { 0.5f, 0.67f, 0.75f, 0.8f, 0.9f, 1.0f, 1.1f, 1.25f, 1.5f, 1.75f, 2.0f, 2.5f, 3.0f, 4.0f };

        //
        // Constructor
        // 
        public ZoomRate()
        {
            zoomRate = EZoomRate.E100P;
            SetZoomValue();
        }

        private void SetZoomValue()
        {
            value = eToV[(byte)zoomRate];
        }

        public float ZoomIn()
        {
            if (zoomRate < EZoomRate.E400P)
            {
                zoomRate++;
            }
            SetZoomValue();
            return value;
        }

        public float ZoomOut()
        {
            if (zoomRate > EZoomRate.E50P)
            {
                zoomRate--;
            }
            SetZoomValue();
            return value;
        }
    }
}
