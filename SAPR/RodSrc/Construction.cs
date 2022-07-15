using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SAPR.RodSrc
{
    class Construction : INotifyPropertyChanged
    {
        private static Construction instance;
        public static Construction Instance
        {
            get { return instance ?? (instance = new Construction()); }
            set
            {
                instance = value;
            }
        }
        protected Construction() { }
        private bool leftbase = true;
        private bool rightbase = false;
        public bool LeftBase
        {
            get
            {
                return leftbase;
            }
            set
            {
                leftbase = value;
                NotifyPropertyChanged("LeftBase");

            }
        }
        public bool RightBase
        {
            get
            {
                return rightbase;
            }
            set
            {
                rightbase = value;
                NotifyPropertyChanged("RightBase");
            }
        }
        private double allowStraining;
        public double AllowStraining
        {
            get
            {
                return allowStraining;
            }
            set
            {
                allowStraining = value;
                NotifyPropertyChanged("AllowStraining");
            }

        }
        public LinkedList<Rod> Rods = new LinkedList<Rod>();
        public void SetBaseEvent(Bases bases)
        {
            bases.PropertyChanged += Bases_PropertyChanged;
        }

        private void Bases_PropertyChanged(object sender,PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "LeftBase")
            {
                LeftBase = ((Bases)sender).LeftBaseCheck;
                return;
            }
            if (e.PropertyName == "RightBase")
            {
                RightBase = ((Bases)sender).RightBaseCheck;
                return;
            }
            return;
        }

        public event PropertyChangedEventHandler PropertyChanged;


        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
