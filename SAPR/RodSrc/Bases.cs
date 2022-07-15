using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace SAPR.RodSrc
{
    class Bases : INotifyPropertyChanged
    {
        private bool leftBaseCheck;
        private bool rightBaseCheck;
        public  bool LeftBaseCheck
        {
            get
            {

                return leftBaseCheck;
            }
            set
            {
                leftBaseCheck = value;
                NotifyPropertyChanged("LeftBase");
            }
        }
        public bool RightBaseCheck
        {
            get
            {

                return rightBaseCheck;
            }
            set
            {
                rightBaseCheck = value;
                NotifyPropertyChanged("RightBase");
            }
        }
        private CheckBox leftBase;
        private CheckBox rightBase;
        public Bases(CheckBox leftbase , CheckBox rightbase)
        {
            leftBase = leftbase;
            rightBase = rightbase;
            leftBase.IsChecked = true;
            rightBase.IsChecked = false;
            rightBase.Unchecked+= RightBase_Checked;
            leftBase.Unchecked += LeftBase_Checked;
            leftBase.Checked += LeftBase_Checked;
            rightBase.Checked += RightBase_Checked;
        }



        private void RightBase_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (rightBase.IsChecked == false)
            {
                if (leftBase.IsChecked == false)
                {
                    rightBase.IsChecked = true;
                    RightBaseCheck = true;
                }
                else
                {
                    rightBase.IsChecked = false;
                    RightBaseCheck = false;
                }


            }
            else RightBaseCheck = true;

        }

        private void LeftBase_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (leftBase.IsChecked == false)
            {
                if (rightBase.IsChecked == false)
                {
                    leftBase.IsChecked = true;
                    LeftBaseCheck = true;
                }
                else
                {
                    leftBase.IsChecked = false;
                    LeftBaseCheck = false;
                }
            }
            else LeftBaseCheck = true;

        }
        public event PropertyChangedEventHandler PropertyChanged;


        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
