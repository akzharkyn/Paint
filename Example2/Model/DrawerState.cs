using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Example2.Model
{
    public enum Shape
    {
        Line,
        Circle,
        Rectangle,
        Pencil,
        Eraser,
        Triangle
        
    }
    public enum DrawTool
    {
        Pen,
        Brush
    }

    public class DrawerState //класс
    {
        public Pen pen = new Pen(Color.Red); //изначально ред пен
        public Bitmap bmp;
        Graphics g;
        GraphicsPath path;
        private PictureBox pictureBox1;

        public Point prevPoint;

        public DrawTool DrawTool { get; set; } //svoistva classa
        public Shape Shape { get; set; }
       

        public void FixPath()  //METHOD
        {
            if (path != null)
            {
                g.DrawPath(pen, path); //?
                path = null;
            }
        }

        public DrawerState(PictureBox pictureBox1)
        {
            this.pictureBox1 = pictureBox1;
            
            Load("");

            DrawTool = DrawTool.Pen;
            Shape = Shape.Pencil;
            

            pictureBox1.Paint += PictureBox1_Paint; //v picturebox dobavlyaem method paint
            
        }

        private void PictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (path != null)
            {
                e.Graphics.DrawPath(pen, path);
            }
        }

        public void Draw(Point currentPoint) //method risovania
        {
            Rectangle rect = new Rectangle(prevPoint.X, prevPoint.Y, currentPoint.X - prevPoint.X, currentPoint.Y - prevPoint.Y);
            switch (Shape)
            {
                case Shape.Line:
                    path = new GraphicsPath();
                    path.AddLine(prevPoint, currentPoint);
                    break;
                case Shape.Circle:
                    path = new GraphicsPath();
                    path.AddEllipse(rect);
                    
                    break;
                case Shape.Rectangle:
                    path = new GraphicsPath();
                    PointF[] rec = new PointF[4];
                    rec[0] = new PointF(prevPoint.X, prevPoint.Y);
                    rec[1] = new PointF(currentPoint.X, prevPoint.Y);
                    rec[2] = new PointF(currentPoint.X, currentPoint.Y);
                    rec[3] = new PointF(prevPoint.X, currentPoint.Y);
                    
                    path.AddPolygon(rec);
                    
                    break;
                case Shape.Triangle:
                    path = new GraphicsPath();
                    PointF[] trian = new PointF[3];
                    trian[0] = new PointF((prevPoint.X + currentPoint.X) / 2,prevPoint.Y);
                    
                    trian[1]= new PointF(prevPoint.X, currentPoint.Y);
                    trian[2] = new PointF(currentPoint.X, currentPoint.Y);
                    path.AddPolygon(trian);
                        break;
                case Shape.Pencil:
                    g.DrawLine(pen, prevPoint, currentPoint);
                    prevPoint = currentPoint;
                    break;
                

                case Shape.Eraser:
                    g.DrawLine(new Pen(Color.White,pen.Width), prevPoint, currentPoint);
                    break;
                default:
                    break;
            }

            pictureBox1.Refresh();
        }

        public void Save(string fileName)
        {
            bmp.Save(fileName);
        }

        public void Load(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                bmp = new Bitmap(this.pictureBox1.Width, this.pictureBox1.Height);
            }
            else {
                bmp = new Bitmap(fileName);
            }

            g = Graphics.FromImage(bmp); //g eto bitmap
            pictureBox1.Image = bmp; //e eto picturebox
        }
    }
}
