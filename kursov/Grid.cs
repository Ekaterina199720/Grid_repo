using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MathNet.Numerics.LinearAlgebra.Double;

namespace kursov
{
    abstract class Grid
    {
        protected static int N;
        public int InternalNodes;
        protected static int CountNodes;
        protected virtual Array SolveGrid(double[,] MatrixOfGrid, double[] VectorOfGrid)
        {
            var array = SparseMatrix.Build.DenseOfArray(MatrixOfGrid).
               Solve(DenseVector.Build.DenseOfArray(VectorOfGrid)).ToArray();
            ViewInFile(array);
            return array;
        }

        protected void ViewInFile(IEnumerable<double> list)
        {

            using (StreamWriter stream = new StreamWriter("G:\\Example\\Solve_matrix.txt"))
            {
                int i = 0;
                foreach (var item in list)
                {
                    i++;
                    if (i == N) { stream.WriteLine(); i = 0; }
                    else stream.Write("{0} ", item);
                }
            }
            return;
        }


    }
}
