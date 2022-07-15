using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SAPR.RodSrc;
using SAPR.DrawSrc;
using SAPR.Processor;
using SAPR.Utility;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.IO;
using System.Globalization;

namespace SAPR
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private RodManager rodMananger;

        private Window ChildrenWindow;

        public MainWindow()
        {
            InitializeComponent();
            customInit();
        }

        private void customInit()
        {
            rodMananger = new RodManager(rodGrid,mainCanvas, new Bases(LeftBase,RightBase), Construction.Instance,Load,Save,allowStraining);
            Processor.Click += Processor_Click;

        }
        private void Processor_Click(object sender, RoutedEventArgs e)
        {
            if (ErrorCheck.CheckErrors(rodGrid) is false)
            {
                return;
            }
            if(ChildrenWindow != null)
            {
                ChildrenWindow.Close();
            }
            Window window = new DiagramWindow();
            window.Show();
            ChildrenWindow = window;
        }

        private void Point_characteristics_Click(object sender, RoutedEventArgs e)
        {   
            if (ErrorCheck.CheckErrors(rodGrid) is false)
            {
                return;
            }
            int? number = ParseInt(RodNumberForChars);
            double? point = ParseDouble(RodLengthForChars);
            Construction construction = Construction.Instance;
            if ((number is null) || (point is null))
            {
                MessageBox.Show("incorrect data");
                return;
            }
            if(number > construction.Rods.Count)
            {
                MessageBox.Show("incorrect rod number");
                return;
            }
            Rod rod = construction.Rods.First.Value;
            foreach(Rod temp in construction.Rods)
            {
                if(temp.Number == number)
                {
                    rod = temp;
                }
            }
            if((Math.Abs((double)point)!= point) || (point > rod.Length))
            {
                MessageBox.Show("incorrect point");
                return;
            }
            Characteristics chars = SAPR_Processor.GetCharacteristic(rod, (double)point);
            MessageBox.Show($" Nx : {chars.Nx}  Ux :{chars.Ux} σx = {chars.NormalStraining}");
            //stringBuilder.Append();
            //MessageBox.Show(answer)

        }
        private double? ParseDouble(TextBox tb)
        {
            {
                string strValue = tb.Text.ToString();

                if (string.IsNullOrEmpty(strValue))
                    return null;
                bool canConvert = false;
                double doubleVal = 0;
                canConvert = double.TryParse(strValue, NumberStyles.Float, new CultureInfo("en-US"), out doubleVal); ;
                if (canConvert)
                {
                    return doubleVal;
                }
                return null;
            }
        }

        private int? ParseInt(TextBox tb)
        {
            {
                string strValue = tb.Text.ToString();

                if (string.IsNullOrEmpty(strValue))
                    return null;
                bool canConvert = false;
                int intVal = 0;
                canConvert = int.TryParse(strValue, NumberStyles.Integer, new CultureInfo("es-ES"), out intVal);
                if (canConvert)
                {
                    return intVal;
                }
                return null;
            }
        }

        private void RodNumberForChars_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text == String.Empty)
            {
                tb.Text = "Rod Number";
            }
        }

        private void RodLengthForChars_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text == String.Empty)
            {
                tb.Text = "Point";
            }
        }

        private void CleanTextBoxOnFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.Text = String.Empty;
        }
    }
}
