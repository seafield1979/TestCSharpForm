using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestCSharpForm
{
    public partial class FormImage : Form
    {
        //
        // Properties
        //
        int drawMode = 0;

        public FormImage()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            drawMode = 0;
            Invalidate();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            drawMode = 1;
            Invalidate();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            drawMode = 2;
            Invalidate();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            drawMode = 3;
            Invalidate();
        }
    
        private void FormImage_Paint(object sender, PaintEventArgs e)
        {
            // Graphicsオブジェクトの作成
            Graphics g = this.CreateGraphics();

            g.SmoothingMode = SmoothingMode.AntiAlias;

            switch (drawMode)
            {
                case 0:
                    DrawMode1(g);
                    break;
                case 1:
                    DrawMode2(g);
                    break;
                case 2:
                    DrawMode3(g, 5);
                    break;
                case 3:
                    DrawMode4(g);
                    break;
            }

            // Graphicsを解放する
            g.Dispose();

        }

        public void DrawMode1(Graphics g)
        {
            // penを作成
            Pen blackPen = new Pen(Color.Black, 1);
            Pen redPen = new Pen(Color.Red, 2);
            Pen bluePen = new Pen(Color.Blue, 3);
            Pen yellowPen = new Pen(Color.Yellow, 4);
            Pen greenPen = new Pen(Color.Green, 5);
            Pen pen2 = new Pen(Color.FromArgb(100, 100, 100));

            // lineの始点と終点を設定
            Point Start_point1 = new Point(50, 40);
            Point End_point1 = new Point(250, 40);

            Point Start_point2 = new Point(50, 80);
            Point End_point2 = new Point(250, 80);

            Point Start_point3 = new Point(50, 120);
            Point End_point3 = new Point(250, 120);

            Point Start_point4 = new Point(50, 160);
            Point End_point4 = new Point(250, 160);

            Point Start_point5 = new Point(50, 200);
            Point End_point5 = new Point(250, 200);

            // lineを描画
            g.DrawLine(blackPen, Start_point1, End_point1);
            g.DrawLine(redPen, Start_point2, End_point2);
            g.DrawLine(bluePen, Start_point3, End_point3);
            g.DrawLine(yellowPen, Start_point4, End_point4);
            g.DrawLine(greenPen, Start_point5, End_point5);

            // penを解放する
            blackPen.Dispose();
            redPen.Dispose();
            bluePen.Dispose();
            yellowPen.Dispose();
            greenPen.Dispose();
        }

        // Bitmap経由で描画
        public void DrawMode2(Graphics g)
        {
            //描画先とするImageオブジェクトを作成する
            Bitmap canvas = new Bitmap(500, 500);
            //ImageオブジェクトのGraphicsオブジェクトを作成する
            Graphics g2 = Graphics.FromImage(canvas);

            //(10, 20)-(100, 200)に、幅1の黒い線を引く
            g2.DrawLine(Pens.Black, 10, 20, 100, 200);

            //リソースを解放する
            g2.Dispose();

            g.DrawImage(canvas, new Point(0, 0));
        }

        public void DrawMode3(Graphics g, int mode)
        {
            // ペンを作成
            Pen pen1 = new Pen(Color.FromArgb(200, 100, 0));

            //　ブラシを作成
            SolidBrush brush1 = new SolidBrush(Color.FromArgb(100, 200, 50, 100));
            Brush brush2 = Brushes.Red;

            

            switch (mode)
            {
                case 1:
                    {
                        // 線を描画
                        // 1line
                        g.DrawLine(pen1, 50, 50, 100, 100);
                        // multi line
                        Point[] points = new Point[] { new Point(50, 50), new Point(150, 50), new Point(150, 150), new Point(50, 150), new Point(50, 50) };
                        g.DrawLines(pen1, points);
                    }
                    break;
                case 2:
                    // rect
                    g.DrawRectangle(pen1, 50, 50, 100, 100);
                    // fill rect
                    g.FillRectangle(brush1, 250, 50, 100, 100);
                    break;
                case 3:
                    // texture
                    {
                        Bitmap bitmap1 = new Bitmap(@"..\..\Resources\apple.jpg");
                        TextureBrush textbrush = new TextureBrush(bitmap1);
                        g.FillRectangle(textbrush, 50, 50, 100, 100);
                        g.FillEllipse(textbrush, 200, 50, 100, 100);
                        g.DrawImage(bitmap1, 400, 50);
                    }
                    break;
                case 4:
                    // circle
                    g.DrawEllipse(pen1, 100, 50, 100, 100);
                    // fill circle
                    g.FillEllipse(brush1, 200, 50, 200, 100);
                    break;
                case 5:
                    // triangle
                    {
                        Point[] points = new Point[] { new Point(50, 50), new Point(150, 50), new Point(50, 150) };
                        g.DrawPolygon(pen1, points);

                        Point[] points2 = new Point[] { new Point(250, 50), new Point(350, 150), new Point(150, 150) };
                        g.FillPolygon(brush1, points2);
                    }
                    break;
            }

        }

        // 文字列を描画
        public void DrawMode4(Graphics g)
        {
            Brush brush1 = Brushes.Blue;

            //フォントオブジェクトの作成
            Font font1 = new Font("MS UI Gothic", 20);

            string str = "hello world";
            g.DrawString(str, font1, brush1, 100, 100);

            // 描画領域のサイズを取得
            //幅の最大値が1000ピクセルとして、文字列を描画するときの大きさを計測する
            SizeF stringSize = g.MeasureString(str, font1, 1000);

            g.DrawRectangle(Pens.Yellow, new Rectangle(100, 100, (int)stringSize.Width, (int)stringSize.Height));
        }


    }
}
