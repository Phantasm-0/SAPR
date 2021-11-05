
using SAPR.RodSrc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SAPR.DrawSrc
{
    class Draw
    {
        private Canvas canvas;
        private LinkedList<double> areasScaled = new LinkedList<double>();
        private LinkedList<double> lengthScaled = new LinkedList<double>();
        LinkedList<Rectangle> rectangles = new LinkedList<Rectangle>();
        public Draw(Canvas canvas)
        {
            this.canvas = canvas;
        }
        private void Scale(LinkedList<Rod> rods) //width = ширина(Length) heigth - высота(Area)
        {
            double sumLength = 0;
            double sumArea = 0;
            foreach(Rod rod in rods)
            {
                sumArea += rod.Area;
                sumLength += rod.Length;
            }
            foreach (Rod rod in rods)
            {
                areasScaled.AddLast(rod.Area / sumArea);
                lengthScaled.AddLast( rod.Length / sumLength);
            }
            
        }
        public void DrawOnCanvas(LinkedList<Rod> rods)
        {
            Clear();
            Scale(rods);
            double width = 0;
            for (int i = 0; i <= areasScaled.Count; i++)
            {
                Rectangle rectangle = new Rectangle()
                {
                    Height = areasScaled.First.Value *150,
                    Width = lengthScaled.First.Value * 300,
                    StrokeThickness = 4,
                    Stroke = new SolidColorBrush(Colors.Black)
                    
                };
                rectangles.AddLast(rectangle);
                canvas.Children.Add(rectangle);
                Canvas.SetLeft(rectangle,width + 20);
                Canvas.SetTop(rectangle, 150);
                width += rectangle.Width;
                areasScaled.RemoveFirst();
                lengthScaled.RemoveFirst();
            }
        }
        private void Clear()
        {
            foreach (Rectangle rectnagle in rectangles)
            {
                canvas.Children.Remove(rectnagle);
            }
            areasScaled.Clear();
            lengthScaled.Clear();
        }
    }
}
