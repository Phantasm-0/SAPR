using SAPR.DrawSrc;
using SAPR.Processor;
using System.Windows;
using SAPR.RodSrc;
using SAPR.PostProcessor;
using System.Collections.Generic;
using System.Windows.Data;
using System;
using System.Globalization;
using System.Windows.Media;

namespace SAPR
{
    /// <summary>
    /// Логика взаимодействия для DiagramWindow.xaml
    /// </summary>
    public partial class DiagramWindow : Window
    {
        public DiagramWindow()
        {
            InitializeComponent();
            CustomInit();
        }
        private void CustomInit()
        {
            DiagramDraw.Draw(Diagrams, SAPR_Processor.GetCharacteristics(Construction.Instance,1000),Construction.Instance.Rods);
            
        }

        private void CharsTable_Loaded(object sender, RoutedEventArgs e)
        {
            LinkedList < LinkedList < Characteristics >> data =   SAPR_Processor.GetCharacteristics(Construction.Instance,10);
            List<CharsTable> result = new List<CharsTable>();
            int rodnumber = 0;
            foreach(LinkedList<Characteristics> charslist  in data)
            {
                rodnumber += 1;
                foreach (Characteristics character in charslist)
                {
                    string Normal = character.NormalStraining.ToString();
                    if (Math.Abs(character.NormalStraining) > Math.Abs(Construction.Instance.AllowStraining))
                        Normal += '*';
                    result.Add(new CharsTable(rodnumber, character.Nx,character.Ux,Normal));
                }
            }
            CharsTable.ItemsSource = result;                
        }

        private void Nx_Checked(object sender, RoutedEventArgs e)
        {
            DiagramDraw.DrawNx(Diagrams);
        }

        private void Nx_Unchecked(object sender, RoutedEventArgs e)
        {
            DiagramDraw.DeleteNx(Diagrams);
        }

        private void Ux_Checked(object sender, RoutedEventArgs e)
        {
            DiagramDraw.DrawUx(Diagrams);
        }

        private void Ux_Unchecked(object sender, RoutedEventArgs e)
        {
            DiagramDraw.DeleteUx(Diagrams);
        }

        private void Normal_Checked(object sender, RoutedEventArgs e)
        {
            DiagramDraw.DrawNormal(Diagrams);
        }

        private void Normal_Unchecked(object sender, RoutedEventArgs e)
        {
            DiagramDraw.DeleteNormal(Diagrams);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DiagramDraw.Normallines.Clear();
            DiagramDraw.Nxlines.Clear();
            DiagramDraw.Uxlines.Clear();
        }
    }
}



