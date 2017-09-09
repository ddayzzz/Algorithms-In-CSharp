using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DrawingFunctionGraph
{
    public struct MyPoint
    {
        public float X { get; set; }
        public float Y { get; set; }
        public MyPoint(float x,float y) { X = x;Y = y; }
    }
    //定义绘制的标志
    public enum OriginPointType
    {
        Center=1,
        NoNegative
    };
    
    public partial class Form1 : Form
    {
        //画笔
        Pen pointPen = new Pen(Color.Red, 1);
        private int unit = 30;//30个像素一个单位
        //原点
        Point origin;
        //绘制的点
        IEnumerable<MyPoint> points;
        //原点的类型
        OriginPointType pointType;
        //比例
        float x_scale=1.0f;
        float y_scale=1.0f;

        public Form1(int unit, float x_scale=1.0f,float y_scale=1.0f)
        {
            InitializeComponent();
            this.unit = unit;
            this.x_scale = x_scale;
            this.y_scale = y_scale;
        }
        //绘制坐标轴
        private void _DrawingCoordinary(Graphics graphics, Panel panel, Point origin)
        {
            //绘制坐标轴
            Pen mypen = new Pen(Color.Black, 1);
            graphics.DrawLine(mypen, 0, origin.Y, panel1.Width, origin.Y);
            graphics.DrawLine(mypen, origin.X, 0, origin.X, panel1.Height);
            //绘制单位
            //绘制X负半轴
            int xcnt = origin.X / unit;
            for (int xc = 0; xc < xcnt; ++xc)
            {
                graphics.DrawRectangle(mypen, origin.X - unit * xc, origin.Y - 2, 1, 2);
                if(xc == 0)
                {
                    graphics.DrawString(xc.ToString(), new Font("Consolas",10),Brushes.Blue, origin.X - unit * xc -5, origin.Y + 5);
                }
                else
                { 
                    graphics.DrawString("-" + xc.ToString(), new Font("Consolas", 10), Brushes.Blue, origin.X - unit * xc -10, origin.Y + 5);
                }
            }
            //x正半轴
            int xposcnt = 1;
            for(int xpoint = origin.X + unit; xpoint < panel1.Width;xpoint +=unit)
            {
                graphics.DrawRectangle(mypen, xpoint, origin.Y - 2, 1, 2);
                graphics.DrawString((xposcnt++).ToString(), new Font("Consolas", 10), Brushes.Blue, xpoint - 5, origin.Y + 5);
            }
            //绘制Y轴的

        }
        //绘制函数
        //设置X轴的比例。实际的坐标为x / x_scale
        private void _DrawingFunction(Graphics graphics, Func<double, double> func,Point origin, double beg,double end, double delta)
        {
            Pen pen = new Pen(Color.Red, 1);
            double res;
            for(;Math.Abs(end - beg)>1e-6;beg+=delta)
            {
                res = func(beg);
                //画一个很小的点
                graphics.DrawEllipse(pen, (float)(origin.X + unit * beg / x_scale), (float)(origin.Y  - unit * res / y_scale), 0.1f, 0.1f);
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            switch (pointType)
            {
                case OriginPointType.Center:
                    origin = new Point(this.panel1.Width / 2, this.panel1.Height / 2);
                    break;
                default:
                    origin = new Point(5, this.panel1.Height - 10);
                    break;
            }
            this._DrawingCoordinary(panel1.CreateGraphics(), panel1, origin);
            //画点
            var graphics = this.panel1.CreateGraphics();
            foreach (var p in points)
            {
                graphics.DrawEllipse(pointPen, (float)(origin.X + unit * p.X / x_scale), (float)(origin.Y - unit * p.Y / y_scale), 0.1f, 0.1f);
            }
        }
        //公开接口：绘制坐标轴
        public void DrawingCoordinary(OriginPointType pointType)
        {
            this.pointType = pointType;
        }
        //公开接口：绘制一个点
        public void DrawingPoints(IEnumerable<MyPoint> points)
        {
            this.points = points;
        }
    }
}
