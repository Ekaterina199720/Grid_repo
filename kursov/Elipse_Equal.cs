using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace kursov
{
   
    class Elipse_Equal : Grid
    {
        enum Axes { Ox, Oy };
        protected List<List<int>> Dots;
        int Count_X = 5;
        int Count_Y = 4;
        private double[,] MatrixOfGrid;
        private double[] VectorOfGrid;
        public double Step;
        public Elipse_Equal(double h)
        {
            Count_X = (int)(4 / h);
            Count_Y = (int)(5 / h);
            Step = h;

           
            Dots = Init_Dots();
            InitMatrixOfGrid();
            Matrix_In_File(MatrixOfGrid);
            ViewInFile((IEnumerable<double>) SolveGrid(MatrixOfGrid, VectorOfGrid));
        }
        private void Matrix_In_File (double [,] a)
        {
            using (StreamWriter stream = new StreamWriter("G:\\Example\\matrix.txt"))
            { int N = a.GetLength(0);
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < N; j++)
                    {
                        if (j == N-1) { stream.WriteLine(); }
                        else stream.Write("{0} ", a[i, j]);
                    }
                }
            }
        }
        protected void InitMatrixOfGrid()
        {
           MatrixOfGrid = new double[Dots.Count, Dots.Count];
            VectorOfGrid = new double[Dots.Count];

            for (int i = 0; i < Dots.Count; i++)
            {
                double x = Step * Math.Abs(Dots[i][0]);
                double y = Step * Math.Abs(Dots[i][1]);
                if (Check_Inside_Bord( x, y+Step) && Check_Inside_Bord(x+Step, y))
                {
                   Internal_One_Equation(i);
                }
                else
                {
                    Board_One_Equation(i);
                }
            }
        }
        protected void Internal_One_Equation(int i)
        {
            for (int j = 0; j < Dots.Count; j++)
            {
                MatrixOfGrid[i, j] = 0;

            }
            double x = Dots[i][0]*Step;
            double y = Dots[i][1]*Step;
            VectorOfGrid[i] = Step * Step * f(x,y);
            MatrixOfGrid[i, i] = -4;
            MatrixOfGrid[i, IndexUpNode(i)] = 1;
            MatrixOfGrid[i, IndexDownNode(i)] = 1;
            MatrixOfGrid[i, IndexLeftNode(i)] = 1;
            MatrixOfGrid[i, IndexRightNode(i)] = 1;

        }
        protected void Board_One_Equation(int i)
        {
            for (int j = 0; j < Dots.Count; j++)
            {
                MatrixOfGrid[i, j] = 0;

            }
            double x = Math.Abs(Dots[i][0]) * Step;
            double y = Math.Abs(Dots[i][1]) * Step;
            
            if (Check_Min(Board(y, Axes.Oy) - x, Board(x, Axes.Ox) - y))
            {//
                double betta = Board(y, Axes.Oy) - x;
                Quatering(i, betta, Axes.Ox);
                VectorOfGrid[i] = Fi(Board(y, Axes.Oy),y)*Step/(Step+betta);

            }
            
            else if((Board(y, Axes.Oy) - x)==0&&( Board(x, Axes.Ox) - y)==0)
             {
                MatrixOfGrid[i,i] = 1;
                VectorOfGrid[i] = Fi(x, y);
            }
            else
            {
                double betta = Board(x, Axes.Ox) - y;
                Quatering(i, betta, Axes.Oy);
                VectorOfGrid[i] = Fi(x, Board(x, Axes.Ox)) * Step / (Step + betta);
            }

            MatrixOfGrid[i, i] = 1;
           }
        private void Quatering(int i, double betta , Axes axes)
        {
            if (Dots[i][0] <= 0 && Dots[i][1] <= 0) Third_Quater(i,betta ,axes);
            else if (Dots[i][0] >= 0 && Dots[i][1] <= 0) Fourth_Quater(i,betta, axes);
            else if (Dots[i][0] >= 0 && Dots[i][1] >=0) First_Quater(i,betta, axes);
            else if (Dots[i][0] <= 0 && Dots[i][1] >= 0) Second_Quater(i,betta, axes);
        }

        private void First_Quater(int i,double betta, Axes axes)
        {
            if (axes == Axes.Ox) {
                MatrixOfGrid[i, IndexLeftNode(i)] = -1.0 / (Step + betta);
               }
            else MatrixOfGrid[i, IndexDownNode(i)] = -1.0 / (Step + betta);
        }
        private void Second_Quater(int i,double betta, Axes axes)

        {
            if (axes == Axes.Ox)
            {
                MatrixOfGrid[i, IndexRightNode(i)] = -1.0 / (Step + betta);
            }
            else MatrixOfGrid[i, IndexDownNode(i)] = -1.0 / (Step + betta);
        }
        private void Third_Quater(int i,double betta, Axes axes)
        {
            if (axes == Axes.Ox)
            {
                MatrixOfGrid[i, IndexRightNode(i)] = -1.0 / (Step + betta);
            }
            else MatrixOfGrid[i, IndexUpNode(i)] = -1.0 / (Step + betta);
        }
        private void Fourth_Quater(int i, double betta, Axes axes)
        {
            if (axes == Axes.Ox)
            {
                MatrixOfGrid[i, IndexLeftNode(i)] = -1.0 / (Step + betta);
            }
            else MatrixOfGrid[i, IndexUpNode(i)] = -1.0 / (Step + betta);
        }
        private double Board(double x, Axes axes)
        {
            return (axes == Axes.Ox) ? Math.Sqrt(25 * (1 - x * x / 16)) : Math.Sqrt(16 * (1 - x * x / 25));
        }
        private double Fi(double x, double y)
        {
            return Math.Abs(x * y);
        }
        
        private bool Check_Min(double x, double y)
        {
          return  (x > y) ? true : false;
        }

        private double f(double x, double y)
        {
            return -x * y;
        }

        private List<List<int>> Init_Dots()
        {
            List<List<int>> point = new List<List<int>>();
                InternalNodes = 0 ;
            int k = 0;
            for (int i = 0, u=0; i <=Count_X; i++, u++)
            {
                for (int j = 0, v=0; j <=Count_Y; j++)
                {
                    if (Check_Inside_Bord((i+1) * Step, (j +1) * Step))
                    {
                        Add_Dots(ref point, ref k, i, u, j, ref v, 0);

                    }
                    else if (Check_Inside_Bord(i * Step, j * Step))
                    {
                        Add_Dots(ref point, ref k, i, u, j, ref v, 1);

                    }

                }
            }

            for (int i = 0; i < point.Count; i++)
            {
                Console.Write(i+"   "+point[i][0]+"  ");
                Console.Write(point[i][1]);
                Console.WriteLine();
            };
            return point;
          
        }

        private void Add_Dots(ref List<List<int>> point, ref int k, int i, int u, int j, ref int v, int Cond_Board)
        {
            if (i == 0 | j == 0)
            {
                if (i == 0 && j == 0)
                {
                    point = Add_Point(point, ref k, u, v, Cond_Board);

                }
                else if (i == 0)
                {
                    point = Add_Point(point, ref k, u, -1 * v, Cond_Board);
                    point = Add_Point(point, ref k, u, v, Cond_Board);
                }
                else if (j == 0)
                {
                    point = Add_Point(point, ref k, -1 * u, v, Cond_Board);
                    point = Add_Point(point, ref k, u, v, Cond_Board);
                }

            }
            else
            {
                point = Add_Point(point, ref k, u, v, Cond_Board);
                point = Add_Point(point, ref k, -1 * u, v, Cond_Board);
                point = Add_Point(point, ref k, u, -1 * v, Cond_Board);
                point = Add_Point(point, ref k, -1 * u, -1 * v, Cond_Board);
            }
            v++;
        }

        private List<List<int>> Add_Point(List<List<int>> point,ref int k, int u, int v, int condit_board)
        {
            point.Add(new List<int>(3));
            point[k].Add(u);
            point[k].Add(v);
            point[k].Add(condit_board);
            InternalNodes++;
            k++;
            return point;
        }
        protected int IndexUpNode(int k)
        {
            return UnionIK(Dots[k][0], Dots[k][1] + 1);
        }
        protected int IndexDownNode(int k)
        { return UnionIK(Dots[k][0], Dots[k][1]- 1); }
        protected int IndexLeftNode(int k)
        { return UnionIK(Dots[k][0] - 1, Dots[k][1]); }
        protected int IndexRightNode(int k)
        {
            return UnionIK(Dots[k][0] + 1, Dots[k][1]);
        }

        protected int UnionIK(int i, int k)
        {
            return Dots.FindIndex(a =>a == Dots.First(n => n[0] == i && n[1] == k));
        }
                
        private bool Check_Inside_Bord(double x, double y)
        {
             return  (x* x / 16.0 + y* y / 25.0 <= 1) ?true : false;
        }

       

    }
}
