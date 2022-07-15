using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
namespace SAPR.RodSrc
{   
    
    class Rod : INotifyPropertyChanged
    {

        public Rod (int number, double area, double length, double elastic,double linearstraining, double firstnode, double lastnode)
        {
            Number = number;
            Area = area;
            Length = length;
            Elastic = elastic;
            FirstNode = firstnode;
            LastNode = lastnode;
            LinearStraining = linearstraining;
        }
        int _Number;
        double _Area;
        double _Length;
        double _Elastic;
        double _LinearStraining;
        double _FirstNode;
        double _LastNode;

        public int Number
        {
            get
            {
                return _Number;
            }
            set
            {
                _Number = value;
                NotifyPropertyChanged("Number");
            }
        }
        public double Area
        {
            get
            {
                return _Area;
            }
            set
            {
                _Area = value;
                NotifyPropertyChanged("Area");
            }
        }
        public double Elastic
        {
            get
            {
                return _Elastic;
            }
            set
            {
                _Elastic = value;
                NotifyPropertyChanged("Elastic");
            }
        }
        public double Length
        {
            get
            {
                return _Length;
            }
            set
            {
                    _Length = value;
                    NotifyPropertyChanged("Length");

            }   
        }
        public double FirstNode

        {
            get
            {
                return _FirstNode
;
            }
            set
            {
                _FirstNode = value;
                NotifyPropertyChanged("FirstNode");
            }
        }
        public double LastNode

        {
            get
            {
                return _LastNode
;
            }
            set
            {
                _LastNode = value;
                NotifyPropertyChanged("LastNode");
            }
        }
        public double LinearStraining

        {
            get
            {
                return _LinearStraining
;
            }
            set
            {

                _LinearStraining = value;
                NotifyPropertyChanged("LinearLoad");
            }
        }
       

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



    }
}
