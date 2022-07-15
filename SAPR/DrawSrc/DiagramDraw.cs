
using SAPR.Processor;
using SAPR.RodSrc;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SAPR.DrawSrc
{
    class DiagramDraw
    {
        public static LinkedList<Polyline> Nxlines = new LinkedList<Polyline>();
        public static LinkedList<Polyline> Uxlines = new LinkedList<Polyline>();
        public static LinkedList<Polyline> Normallines = new LinkedList<Polyline>();

        static public void Draw(Canvas canvas, LinkedList<LinkedList<Characteristics>> characteristics, LinkedList<Rod> rods)
        {
            DrawVertLines(canvas,characteristics,rods);
            
        }
        static private void DrawVertLines(Canvas canvas, LinkedList<LinkedList<Characteristics>> characteristics, LinkedList<Rod> rods)
        {
            double max = 0;
            foreach(LinkedList<Characteristics> chars_list in characteristics)
            {
                foreach (Characteristics character in chars_list)
                {
                    if (max < Math.Abs(character.Nx))
                    {
                        max = Math.Abs(character.Nx);
                    }
                    if (max < Math.Abs(character.Ux))
                    {
                        max = Math.Abs(character.Ux);
                    }
                    if (max < Math.Abs(character.NormalStraining))
                    {
                        max = Math.Abs(character.NormalStraining);
                    }

                }
            }
            const int STROKETHICKNESS = 4;
            const double minLength = 100;
            const double maxLength = 400;
            Line RoadLengthLine = new Line()
            {
                Stroke = Brushes.Black,
                StrokeThickness = STROKETHICKNESS,
                X1 = canvas.Width / 4,
                X2 = canvas.Width / 4,
                Y1 = canvas.Height / 8,
                Y2 = 7 * canvas.Height / 8
            };
            canvas.Children.Add(RoadLengthLine);
            double width = 0;
            LinkedListNode<LinkedList<Characteristics>> currCharacteristics = characteristics.First;
            foreach (Rod currentRod in rods)
            {
                int currentRodLengthCounter = 0;
                foreach (Rod rod in rods)
                {
                    if (currentRod != rod)
                    {
                        if (currentRod.Length < rod.Length)
                        {
                            currentRodLengthCounter++;
                        }
                    }
                }
                double Length = maxLength - currentRodLengthCounter * 100;
                if (Length < minLength)
                {
                    Length = minLength;
                }
                DrawChars(canvas, currCharacteristics, (canvas.Width / 4 + width), canvas.Width / 4 + width + Length,max);
                width += Length;
                Line RoadLengthLineMass = new Line()
                {
                    Stroke = Brushes.Black,
                    StrokeThickness = STROKETHICKNESS,
                    X1 = canvas.Width / 4 + width,
                    X2 = canvas.Width / 4 + width,
                    Y1 = canvas.Height / 8,
                    Y2 = 7 * canvas.Height / 8
                };
                canvas.Children.Add(RoadLengthLineMass);
                currCharacteristics = currCharacteristics.Next;
            }

            DrawHorizLines(canvas,canvas.Width / 4 + width);
            canvas.Width = width + 400;
        }


        static private void DrawHorizLines(Canvas canvas, double width)
        {
            const int STROKETHICKNESS = 4;
            //Line RoadLengthLineNx = new Line()
            //{
            //    Stroke = Brushes.Black,
            //    StrokeThickness = STROKETHICKNESS,
            //    X1 = canvas.Width / 4,
            //    X2 = width,
            //    Y1 = canvas.Height / 4,
            //    Y2 = canvas.Height / 4
            //};
            Line RoadLengthLineUx = new Line()
            {
                Stroke = Brushes.Black,
                StrokeThickness = STROKETHICKNESS,
                X1 = canvas.Width / 4,
                X2 = width,
                Y1 = canvas.Height / 2,
                Y2 = canvas.Height / 2
            };
            //Line RoadLengthLineNS = new Line()
            //{
            //    Stroke = Brushes.Black,
            //    StrokeThickness = STROKETHICKNESS,
            //    X1 = canvas.Width / 4,
            //    X2 = width,
            //    Y1 = 6 * canvas.Height / 8,
            //    Y2 = 6 * canvas.Height / 8
            //};
            //canvas.Children.Add(RoadLengthLineNS);
            canvas.Children.Add(RoadLengthLineUx);
            //canvas.Children.Add(RoadLengthLineNx);
        }

        static void DrawChars(Canvas canvas, LinkedListNode<LinkedList<Characteristics>> characteristics, double start, double end, double max)
        {

            double step = (end - start) / characteristics.Value.Count;
            Polyline lineNx = new Polyline()
            {
                Stroke = Brushes.Red,
                StrokeThickness = 6
            };
            Polyline lineUx = new Polyline()
            {
                Stroke = Brushes.Green,
                StrokeThickness = 6
            };
            Polyline lineNormal = new Polyline()
            {
                Stroke = Brushes.Blue,
                StrokeThickness = 6
            };
            double x = start;
            foreach (Characteristics charects in characteristics.Value)
            {
                double yNx = (canvas.Height / 4) / max * charects.Nx;
                double yUx = (canvas.Height / 4) / max * charects.Ux;
                double yNormal = (canvas.Height / 4) / max * charects.NormalStraining;

                lineNx.Points.Add(new Point(x, canvas.Height / 2 - yNx));
                lineUx.Points.Add(new Point(x, canvas.Height / 2 - yUx));
                lineNormal.Points.Add(new Point(x, canvas.Height / 2 - yNormal));
                x += step;
            }

            canvas.Children.Add(lineNx);
            canvas.Children.Add(lineUx);
            canvas.Children.Add(lineNormal);
            Nxlines.AddLast(lineNx);
            Uxlines.AddLast(lineUx);
            Normallines.AddLast(lineNormal);


        }

        static public void DrawNx(Canvas canvas)
        {
            foreach(Polyline Nxline in Nxlines)
            {
                canvas.Children.Add(Nxline);
            }

        }
        static public void DeleteNx(Canvas canvas)
        {
            foreach (Polyline Nxline in Nxlines)
            {
                canvas.Children.Remove(Nxline);
            }
        }
        static public void DrawUx(Canvas canvas)
        {
            foreach(Polyline Uxline in Uxlines)
            {
                canvas.Children.Add(Uxline);
            }
        }
        static public void DeleteUx(Canvas canvas)
        {
            foreach (Polyline Uxline in Uxlines)
            {
                canvas.Children.Remove(Uxline);
            }
        }

        static public void DrawNormal(Canvas canvas)
        {
            foreach(Polyline Normalline in Normallines)
            {
                canvas.Children.Add(Normalline);
            }
        }
        static public void DeleteNormal(Canvas canvas)
        {
            foreach (Polyline Normalline in Normallines)
            {
                canvas.Children.Remove(Normalline);
            }
        }
    }
}
