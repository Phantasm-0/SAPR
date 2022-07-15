using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPR.Processor
{
    class Characteristics
    {
        public double Nx;
        public double NormalStraining;
        public double Ux;
        public Characteristics(double nx,double normalstrainig,double ux)
        {
            Nx = nx;
            NormalStraining = normalstrainig;
            Ux = ux;
        }
    }
}
