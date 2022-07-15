using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPR.Processor
{
    class Maths
    {

        public static double[] Gauss(double[,] Matrix)
        {
            int n = Matrix.GetLength(0);
            double[,] Matrix_Clone = new double[n, n + 1];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n + 1; j++)
                    Matrix_Clone[i, j] = Matrix[i, j];

            for (int k = 0; k < n; k++) 
            {
                for (int i = 0; i < n + 1; i++)
                    Matrix_Clone[k, i] = Matrix_Clone[k, i] / Matrix[k, k]; 
                for (int i = k + 1; i < n; i++) 
                {
                    double K = Matrix_Clone[i, k] / Matrix_Clone[k, k];
                    for (int j = 0; j < n + 1; j++) 
                        Matrix_Clone[i, j] = Matrix_Clone[i, j] - Matrix_Clone[k, j] * K; 
                }
                for (int i = 0; i < n; i++) 
                    for (int j = 0; j < n + 1; j++)
                        Matrix[i, j] = Matrix_Clone[i, j];
            }


            for (int k = n - 1; k > -1; k--)
            {
                for (int i = n; i > -1; i--) 
                    Matrix_Clone[k, i] = Matrix_Clone[k, i] / Matrix[k, k];
                for (int i = k - 1; i > -1; i--) 
                {
                    double K = Matrix_Clone[i, k] / Matrix_Clone[k, k];
                    for (int j = n; j > -1; j--) 
                        Matrix_Clone[i, j] = Matrix_Clone[i, j] - Matrix_Clone[k, j] * K;
                }
            }

            double[] Answer = new double[n];
            for (int i = 0; i < n; i++)
                Answer[i] = Matrix_Clone[i, n];

            return Answer;
        }
    }
}

