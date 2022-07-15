using SAPR.RodSrc;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SAPR.DrawSrc
{
    class Draw
    {
        const double maxLength = 400;
        const double maxArea = 200;
        const double minLength = 100;
        const double minArea = 50;
        BitmapImage rightArrow;
        BitmapImage leftArrow;
        BitmapImage leftSupport;
        BitmapImage rightSupport;
        private Canvas canvas;
        LinkedList<Rectangle> rectangleRods = new LinkedList<Rectangle>();
        LinkedList<Shape> rectanglePointForces = new LinkedList<Shape>();
        LinkedList<Rectangle> rectangleLinearStraining = new LinkedList<Rectangle>();
        LinkedList<Rectangle> rectangleBases = new LinkedList<Rectangle>();
        public Draw(Canvas canvas)
        {
            this.canvas = canvas;
            LoadImgs();
        }



        private void LoadImgs()
        {
            rightArrow = new BitmapImage(new Uri("..\\..\\imgSrc\\rightarrow.png", UriKind.Relative));
            leftArrow = new BitmapImage(new Uri("..\\..\\imgSrc\\leftarrow.png", UriKind.Relative));
            leftSupport = new BitmapImage(new Uri("..\\..\\imgSrc\\leftSupport.png", UriKind.Relative));
            rightSupport = new BitmapImage(new Uri("..\\..\\imgSrc\\rightSupport.png", UriKind.Relative));
        }

        private void DrawRectangle(double Area, double Length, double width)
        {
                Rectangle rectangle = new Rectangle()
                {
                    Height = Area ,
                    Width = Length ,
                    StrokeThickness = 3,
                    Stroke = new SolidColorBrush(Colors.Black)
                };
                rectangleRods.AddLast(rectangle);
                canvas.Children.Add(rectangle);
                Canvas.SetLeft(rectangle,width + 250);
                Canvas.SetTop(rectangle,300 - rectangle.Height / 2);
        }
        
        public void TrueDraw(Construction construction)
        {
            LinkedList<Rod> rods = construction.Rods;
            Clear();
            double width = 0;
            foreach (Rod currentRod in rods)
            {   
                int currentRodAreaCounter = 0;
                int currentRodLengthCounter = 0;
                foreach (Rod rod in rods)
                {
                    if (currentRod != rod)
                    {
                        if (currentRod.Area < rod.Area)
                        {
                            currentRodAreaCounter++;
                        }
                        if (currentRod.Length < rod.Length)
                        {
                            currentRodLengthCounter++;
                        }
                    }
                }
                double Area = maxArea - currentRodAreaCounter * 60;
                if (Area < minArea)
                {
                    Area = minArea;
                }
                double Length = maxLength - currentRodLengthCounter * 100;
                if (Length < minLength)
                {
                    Length = minLength;
                }
                DrawRectangle(Area,Length,width);
                if (currentRod.FirstNode != 0) { DrawArrowCorrect(Area, Length, width, currentRod.FirstNode); }
                if (currentRod.LastNode != 0) { DrawArrowCorrect(Area, Length, width + 3 *Length /4, currentRod.LastNode); }
                if(currentRod.LinearStraining != 0) { DrawArrows(Area, Length, width, currentRod.LinearStraining); }
                width += Length;
            }
            DrawSupports(width,construction.LeftBase,construction.RightBase);
            CanvasRescale();
        }

        private void DrawArrows(double Area, double Length, double width, double ql)
        {
            if (ql == 0)
            {
                return;
            }
            BitmapImage arrow;
            if (ql < 0)
            {
                arrow = leftArrow;
            }
            else arrow = rightArrow;
            int arrowcount = 12;
            for(int i = 0; i < arrowcount; i++)
            {
                Rectangle rectangle = new Rectangle()
                {
                    Height = Area / 12,
                    Width = Length / 12,
                    StrokeThickness = 0,
                    Fill = new ImageBrush(arrow)

                };
                rectangleLinearStraining.AddLast(rectangle);
                canvas.Children.Add(rectangle);
                Canvas.SetLeft(rectangle, width + 250 + (Length/arrowcount * i));
                Canvas.SetTop(rectangle, 300 - rectangle.Height / 2);
            }
            
        }

        private void DrawSupports(double width, bool leftBase, bool rightBase)
        {
            double supportsHeight;
            double supportWidth = 22;
            if(rectangleRods.First.Value.Height > rectangleRods.Last.Value.Height)
            {
                supportsHeight = rectangleRods.First.Value.Height * 1.125;
            } 
            else
                supportsHeight = rectangleRods.Last.Value.Height * 1.125;
            if (leftBase)
            {
                Rectangle rectangle = new Rectangle()
                {
                    Height = supportsHeight,
                    Width = supportWidth ,
                    StrokeThickness = 0,
                    Fill = new ImageBrush(leftSupport)
                };
                rectangleBases.AddLast(rectangle);
                canvas.Children.Add(rectangle);
                Canvas.SetLeft(rectangle, 250 - supportWidth);
                Canvas.SetTop(rectangle, 300 - rectangle.Height / 2);
            }
            if (rightBase)
            {
                Rectangle rectangle = new Rectangle()
                {
                    Height = supportsHeight ,
                    Width = supportWidth,
                    StrokeThickness = 0,
                    Fill = new ImageBrush(rightSupport)
                };
                rectangleBases.AddLast(rectangle);
                canvas.Children.Add(rectangle);
                Canvas.SetLeft(rectangle, width + 250);
                Canvas.SetTop(rectangle, 300 - rectangle.Height / 2);
            }

        }

        private void DrawArrowCorrect(double Area, double Length, double width, double q) 
        {
            double sumHeight = Area /4;
            double sumWidth = Length / 4;
            if(q == 0)
            {
                return;
            }
            Polygon arrow = new Polygon()
            {
                Points = new PointCollection(),
                Fill = Brushes.Black
            };
            Rectangle rectangle = new Rectangle()
            {
                Height = sumHeight / 2,
                Width = 2 * sumWidth / 4,
                StrokeThickness = 0,
                Fill = Brushes.Black
            };
            canvas.Children.Add(arrow);
            rectanglePointForces.AddLast(arrow);
            canvas.Children.Add(rectangle);
            rectanglePointForces.AddLast(rectangle);
            if (q < 0)
            {
                Canvas.SetLeft(rectangle, width + 250 + sumWidth /2);
                Canvas.SetTop(rectangle, 300 - rectangle.Height / 2);
                arrow.Points.Add(new System.Windows.Point(width + 250 + rectangle.Width, (300) - (4 * rectangle.Height / 4)));
                arrow.Points.Add(new System.Windows.Point(width + 250 + rectangle.Width, (300) + (4 * rectangle.Height / 4)));
                arrow.Points.Add(new System.Windows.Point(width + 250 , 300));
            }
            if (q > 0)
            {
               
                Canvas.SetLeft(rectangle, width + 250);
                Canvas.SetTop(rectangle, 300 - rectangle.Height / 2);
                arrow.Points.Add(new System.Windows.Point(width + 250  + rectangle.Width, (300 ) -  (4 * rectangle.Height/4)));
                arrow.Points.Add(new System.Windows.Point(width + 250 + rectangle.Width,(300 ) + (4 * rectangle.Height / 4)));
                arrow.Points.Add(new System.Windows.Point(width + 250 + sumWidth,300));
                
            }
        }

        private void CanvasRescale()
        {
            double width = 400;
            foreach(Rectangle rod in rectangleRods)
            {
                width += rod.Width;
            }
            if(width< 1600)
            {
                width = 1600;
            }
            canvas.Width = width;
        }

        private void Clear()
        {
            foreach (Rectangle rectnagle in rectangleRods)
            {
                canvas.Children.Remove(rectnagle);
            }
            foreach(Shape force in rectanglePointForces)
            {
                canvas.Children.Remove(force);
            }
            foreach (Rectangle force in rectangleLinearStraining)
            {
                canvas.Children.Remove(force);
            }
            foreach(Rectangle support in rectangleBases)
            {
                canvas.Children.Remove(support);
            }
            rectangleBases.Clear();
            rectangleRods.Clear();
            rectanglePointForces.Clear();
            rectangleLinearStraining.Clear();        
        }
    }
}
