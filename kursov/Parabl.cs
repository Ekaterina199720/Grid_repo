using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursov
{
    class Parabl
    {
        List<List<double>> answer;
        public int N { get; set; }
        public int M { get; set; }
        public double T { get; set; }
        double H;
        double L;
        public Parabl(int n, double T, int m)
        {
            N = n;
            M = m;
            this.T = T;
            Create_Init_Answer();
        }

        private void Create_Init_Answer()
        {
            H = 1.0 / N;
            L = T / M;
            answer = new List<List<double>>();
            for (int t = 0; t <= M; t++)
            {
                if (t == 0)
                {
                    answer.Add(new List<double>());
                    for (int j = 1; j <= N; j++)
                    { answer[t].Add(Fi(j * H)); }
                }

                else { One_Level(t); }


            }
            for (int t = 0; t <= M; t++)
            {
                Console.WriteLine("Layer" + t);
                foreach (var item in answer[t])
                {
                    Console.Write(item+"    ");
                }
                Console.WriteLine();
                
            }
        }

                private double[,] Straigh(int t)
        {  double s = H * H / L;
            double[,] mas = new double[N - 1,2];
            for (int  u= 0; u < N-1; u++)
            {
                if (u == 0)

                {
                    mas[u, 0] = 1.0 / (2 + s);
                    mas[u, 1] = M1(t * L) + s * answer[t-1][u] + H * H * F(u * H, t * L);
                }
                else
                {
                    mas[u, 0] = 1 / (2 + s - mas[u - 1,0]);
                    mas[u, 1] =mas[u-1,0]*mas[u-1,1] + s * answer[t-1][u] + H * H * F(u * H, t * L);
                }
            }
            return mas;
        }       
        private void One_Level(int t)
        {
            List<double> a = new List<double>(N-1);
            double[,] mas = Straigh(t);

            for (int i = N; i >= 1; i--)
            { double x = (i == N) ? M2(H * N) : mas[i - 1, 0] * (mas[i - 1, 1] + answer[t - 1].Last());
                a.Add(x);
            }
            a.Reverse();
            answer.Add(a);
        }
        
        private double F(double x, double t)
        {
            return t * Math.Sin(x);
        }

        private double Fi(double x)
        {
            return x * Math.Exp(x);
        }
        private double M1(double t)
        {
            return 1 - Math.Cos(t);
        }
        private double M2(double t)
        {
            return t + Math.Exp(1);
        }
    }
}
