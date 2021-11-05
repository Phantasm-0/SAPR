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
namespace SAPR
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private RodManager rodMananger;


        public MainWindow()
        {
            InitializeComponent();
            customInit();
        }

        private void customInit()
        {
            rodMananger = new RodManager(rodGrid,mainCanvas);
            
        }


        private void LeftBase_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void LeftBase_Copy_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void RightBase_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_Add_Rod(object sender, RoutedEventArgs e)
        {
            
        }

    }
}
