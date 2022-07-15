using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPR.PostProcessor
{
    class CharsTable
    {
        public CharsTable(int rodNumber,double n ,double u, string normalstrainig)
        {
            RodNumber = rodNumber;
            N = n;
            U = u;
            Normal = normalstrainig;
        }
        public int RodNumber { get; set; }
        public double N { get; set; }
        public double U { get; set; }
        public string Normal { get; set; }
    }
}
