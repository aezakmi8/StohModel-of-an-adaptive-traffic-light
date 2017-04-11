using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveLighter
{
    class Program
    {
        static void Main(string[] args)
        {
            StohModel AdaptiveLighter = new StohModel();
            //Console.WriteLine(AdaptiveLighter.GetInterval());
            string text = Console.ReadLine();
            string result = AdaptiveLighter.GetInterval();
            Console.WriteLine(result);
            Console.ReadKey();
        }
        public class StohModel
        {
            public double[,] carsInQuery = { { 5, 6, 7, 8, 3, 9, 3, 2, 6, 4 }, { 4, 5, 3, 3, 7, 8, 4, 8, 9, 2 } }; //Хо, машины оставшиеся не пропущенными на данном направлении i, данного j-перекрестка, после выполнения (k-1) шага
            public double[,] resultInterval = new double[2, 10]; //формула с суммой
            public double[,] a = new double[2, 10];
            public double[,] b = new double[2, 10];
            public double[,] kRInterval = new double[2, 10]; //коэфф e
            public double t = 100; //единица времени
            public double[,] criticNumb = { { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 }, { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 } }; // L, некоторое критическое число
            public string GetInterval()
            {
                double[,] inCar = { { 12, 3, 6, 4, 3, 7, 2, 8, 5, 4 }, { 5, 6, 3, 2, 7, 14, 6, 10, 8, 9 } }; //Лямбда, без единицы времени
                double[,] outCar = { { 9, 1, 8, 3, 7, 8, 4, 2, 4, 6 }, { 7, 3, 9, 4, 6, 1, 7, 4, 7, 3 } }; //Мю, без единицы времени
                double[,] timeInterval = { { 3, 6, 9, 4, 7, 9, 9, 1, 2, 3 }, { 3, 5, 1, 9, 4, 8, 4, 8, 4, 2 } };

                string list = "";
                try
                {
                    int n = 1;
                    //for (int n = 1; n <= 20; n++)
                    //{
                    //for (int k = 20; k <= 1; k--)
                        for (int i = 0; i < resultInterval.GetLength(0); i++)
                            for (int j = 0; j < resultInterval.GetLength(1); j++)
                            {
                                //n++;
                                a[i, j] = ((inCar[i, j] / timeInterval[i, j]) * (inCar[i, j] / timeInterval[i, j]) + (outCar[i, j] / timeInterval[i, j]) * (outCar[i, j] / timeInterval[i, j])) / 2 * (outCar[i, j] / timeInterval[i, j]);
                                b[i, j] = (outCar[i, j] / timeInterval[i, j]) - (inCar[i, j] / timeInterval[i, j]);
                                resultInterval[i, j] += Math.Pow(-1, Convert.ToDouble(n + 1)) * ((Math.Exp((b[i, j] * criticNumb[i, j]) / (2 * a[i, j])) * Math.Sin((Math.PI) * n * carsInQuery[i, j] / criticNumb[i, j])) + Math.Sin(Math.PI * n * ((criticNumb[i, j] - carsInQuery[i, j]) / criticNumb[i, j]))) / (Math.PI * n + ((b[i, j] * b[i, j] * criticNumb[i, j] * criticNumb[i, j]) / 4 * Math.PI * n * a[i, j] * a[i, j])) * Math.Exp(-((Math.PI * Math.PI * n * n * a[i, j] * t) / (criticNumb[i, j] * criticNumb[i, j])));
                                kRInterval[i, j] = 2 * (Math.Exp(-((2 * b[i, j] * carsInQuery[i, j]) + (b[i, j] * b[i, j] * t)) / (4 * a[i, j])));
                                //resultInterval[0, 0] = Math.Pow(-1, Convert.ToDouble(n + 1)) * ((Math.Exp((b[0, 0] * criticNumb[0, 0]) / (2 * a[0, 0])) * Math.Sin((Math.PI) * n * carsInQuery[0, 0] / criticNumb[0, 0])) + Math.Sin(Math.PI * n * ((criticNumb[0, 0] - carsInQuery[0, 0]) / criticNumb[0, 0]))) / (Math.PI * n + ((b[0, 0] * b[0, 0] * criticNumb[0, 0] * criticNumb[0, 0]) / 4 * Math.PI * n * a[0, 0] * a[0, 0])) * Math.Exp(-((Math.PI * Math.PI * n * n * a[0, 0] * t) / (criticNumb[0, 0] * criticNumb[0, 0])));
                                //list = Convert.ToString(resultInterval[0, 0]);
                                list += resultInterval[i, j] * kRInterval[i, j] + "\n";
                                //list += b[i, j] + "\n";
                                //list += kRInterval[i, j] + "\n";
                                //carsInQuery[i, j] = /* Math.Pow(Xo, k-1) * Cумму(*/Math.Pow(inCar[i, j], k - 1) * ;
                            }
                    //list = Convert.ToString(--n);
                    //for (int i = 0; i < a.GetLength(0); i++)
                    //    for (int j = 0; j < a.GetLength(1); j++)
                    //    {
                    //        double c = 0;
                    //        listA_B += i+" "+j+"  "+ "a: "+a[i, j]+"  b: "+b[i,j] + "\n";
                    //        kRInterval[i, j] = 2 * (Math.Exp(-((2 * b[i, j] * carsInQuery[i, j]) + (b[i, j] * b[i, j] * t)) / (4 * a[i, j])));
                    //    }
                    // }
                }
                catch (IndexOutOfRangeException ex)
                {
                    Console.WriteLine(ex);
                }
                return list;
            }
        }
    }
}

