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

        LVDocument1 document1;
        private bool isMouseDown;
        private Point mouseDownPos;
        private Point mouseOldPos;

        private bool isControl;
        private bool isShift;

        #endregion

        #region メソッド

        private void Initialize()
        {
            document1 = new LVDocument1(this.ClientSize.Width, this.ClientSize.Height, 0, hScrollBar2, vScrollBar2);

            // マウスホイールのイベント登録
            this.MouseWheel += new MouseEventHandler(this.MainForm_MouseWheel);

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

        // pixtime::
        private void PixTimeZoomUp()
        {
            if (document1.PixTimeZoomUp())
            {
                panel1.Invalidate();
            }
        }

        private void PixTimeZoomDown()
        {
            if (document1.PixTimeZoomDown())
            {
                panel1.Invalidate();
            }
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
            panel1.Invalidate();
        }

        private void hScrollBar2_Scroll(object sender, ScrollEventArgs e)
        {
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
                    if (isControl)
                    {
                        PixTimeZoomDown();
                    }
                    else { 
                        ZoomDown();
                    }
                    break;
                case Keys.Right:
                    break;
                case Keys.Down:
                    if (isControl)
                    {
                        PixTimeZoomUp();
                    }
                    else
                    {
                        ZoomUp();
                    }
                    break;
                case Keys.ControlKey:
                    isControl = true;
                    break;
                case Keys.ShiftKey:
                    isShift = true;
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
                case Keys.V:
                    document1.SetDirection(0);
                    panel1.Invalidate();
                    break;
                case Keys.H:
                    document1.SetDirection(1);
                    panel1.Invalidate();
                    break;
            }
        }
        
        private void FormScrollBar_KeyUp(object sender, KeyEventArgs e)
        {
            Debug.WriteLine(String.Format("keyup:{0}", e.KeyValue));

            switch ((Keys)e.KeyValue)
            {
                case Keys.ControlKey:
                    isControl = false;
                    break;
                case Keys.Shift:
                    isShift = false;
                    break;
            }
        }
        #endregion イベント

        private void FormScrollBar_KeyPress(object sender, KeyPressEventArgs e)
        {
            Debug.WriteLine(String.Format("keypress:{0}", e.KeyChar));

            switch ((Keys)e.KeyChar)
            {
                case Keys.Left:
                    break;
                case Keys.Control:
                    isControl = true;
                    break;
                case Keys.Shift:
                    isShift = true;
                    break;
            }
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
