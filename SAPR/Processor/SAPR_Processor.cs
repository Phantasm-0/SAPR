using SAPR.RodSrc;
using SAPR.Processor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPR.Processor
{
    class SAPR_Processor
    {
        static private double[] displacements;
        public static double[] GetDisplacements(Construction construction)
        {
            LinkedList<Rod> rods = construction.Rods;
            double[,] A = new double [rods.Count+1,rods.Count+1];
            double[,] b = new double[rods.Count + 1, 1];
            double[,] extended = new double[rods.Count+1,rods.Count+2];
            for ( int i= 0 ; i < rods.Count + 1; i++)
            {
                for (int j = 0; j < rods.Count + 1; j++)
                {
                    A[i,j] = 0;
                    b[i, 0] = 0;
                }
            }
            LinkedListNode<Rod> Rod = rods.First;

            int g = 0;
            for (int i = 0;i <= rods.Count - 1;)
            {

                A[i, g] = Rod.Value.Elastic * Rod.Value.Area / Rod.Value.Length + PrevNodeCheckAndReturnK(Rod);
                A[i +1, g] =  -1 * Rod.Value.Elastic * Rod.Value.Area / Rod.Value.Length;
                A[i, g + 1] = -1 * Rod.Value.Elastic * Rod.Value.Area / Rod.Value.Length;
                A[i+1, g+1] = (Rod.Value.Elastic * Rod.Value.Area / Rod.Value.Length)+ NextNodeCheckAndReturnK(Rod);
                Rod = Rod.Next;
                i++;
                g++;
            }

            Rod = rods.First;
            for (int i = 0; i <= rods.Count - 1; i++)
            {
                b[i, 0] = PrevNodeCheckAndReturnSumQ(Rod) + (Rod.Value.FirstNode + (Rod.Value.Length * Rod.Value.LinearStraining / 2));
                b[i + 1, 0] = NextNodeCheckAndReturnSumQ(Rod) + (Rod.Value.LastNode +(Rod.Value.Length * Rod.Value.LinearStraining /2 ));
                Rod = Rod.Next;
            }

            if (construction.LeftBase)
            {
                for (int i = 0; i <= rods.Count; i++)
                {
                    A[i,0] = 0;
                    A[0, i] = 0;
                }
                A[0, 0] = 1;
                b[0,0] = 0;
                
            }
            if (construction.RightBase)
            {
                for (int i = 0; i <= rods.Count; i++)
                {
                    A[i, rods.Count] = 0;
                    A[rods.Count, i] = 0;
                }
                A[rods.Count, rods.Count] = 1;
                b[rods.Count, 0] = 0;
            }
            for (int i = 0; i < rods.Count + 1 ; i++)
            {
                for (int j = 0; j < rods.Count + 2; j++)
                {
                    if (j == rods.Count + 1)
                    {
                        extended[i, j] = b[i ,0];
                    }
                    else
                    {
                        extended[i, j] = A[i, j];
                    }
                }
            }
            double[] answer = Maths.Gauss(extended);
            displacements = answer;
            return answer;
        }

        public static Characteristics GetCharacteristic(Rod rod, double x)
        {
            double[] displacements = GetDisplacements(Construction.Instance);
            double nx = (rod.Elastic * rod.Area / rod.Length ) *(displacements[rod.Number] - displacements[rod.Number - 1]) + ((rod.LinearStraining * rod.Length / 2) * (1 - 2* x / rod.Length));
            double ux = displacements[rod.Number - 1] +( x / rod.Length ) * (displacements[rod.Number] - displacements[rod.Number - 1]) + (rod.LinearStraining * rod.Length  * x / (2 * rod.Elastic * rod.Area ) * (1 - x / rod.Length)); //displacements[rod.Number - 1] + x/rod.Length * (displacements[rod.Number] - displacements[rod.Number - 1]) + (rod.LinearStraining * rod.Length * rod.Length * x ) / ( 2 * rod.Elastic * rod.Area * rod.Length) * (1 - 2 * x / rod.Length);
            double normalstraining =  nx / rod.Area;
            return new Characteristics(nx, normalstraining, ux);

        }
        public static LinkedList<LinkedList<Characteristics>> GetCharacteristics(Construction construction, int dotnumbers)
        {
            double[] displacements = GetDisplacements(construction);
            LinkedList<LinkedList<Characteristics>> allCharacteristics = new LinkedList<LinkedList<Characteristics>>();
            foreach (Rod rod in construction.Rods)
            {
                double dotnumber = rod.Length / dotnumbers;
                LinkedList<Characteristics> currentRodCharacteristics = new LinkedList<Characteristics>();
                for ( double i = 0; i < rod.Length; i += dotnumber)
                {
                    currentRodCharacteristics.AddLast(GetCharacteristic(rod, i));
                }
                allCharacteristics.AddLast(currentRodCharacteristics);

            } 
            return allCharacteristics; 
        }

        private static double NextNodeCheckAndReturnSumQ(LinkedListNode<Rod> rod)
        {
            if (!NextNodeCheck(rod))
            {
                return 0;
            }
            return rod.Next.Value.FirstNode + (rod.Next.Value.Length * rod.Next.Value.LinearStraining / 2);
        }

        private static double NextNodeCheckAndReturnK(LinkedListNode<Rod> Rod)
        {
            if (!NextNodeCheck(Rod))
            {
                return 0;
            }
            return (Rod.Next.Value.Elastic * Rod.Next.Value.Area / Rod.Next.Value.Length);
        }

        private static bool NextNodeCheck(LinkedListNode<Rod> Rod)
        {
            bool flag = true;
            if(Rod.Next is null)
            {
                flag = false;
            }
            return flag;
        }

        private static double PrevNodeCheckAndReturnK(LinkedListNode<Rod> Rod)
        {
            if (!PrevNodeCheck(Rod))
            {
                return 0;
            }
            return (Rod.Previous.Value.Elastic * Rod.Previous.Value.Area / Rod.Previous.Value.Length);
        }

        private static bool PrevNodeCheck(LinkedListNode<Rod> Rod)
        {
            bool flag = true;
            if (Rod.Previous is null)
            {
                flag = false;
            }
            return flag;
        }

        private static double PrevNodeCheckAndReturnSumQ(LinkedListNode<Rod> rod)
        {
            if (!PrevNodeCheck(rod))
            {
                return 0;
            }
            return (rod.Previous.Value.Length * rod.Previous.Value.LinearStraining / 2 ) + rod.Previous.Value.LastNode; 
        }

    }
}
