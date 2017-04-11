using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            int z = 0;
            int x = 0;
            int y = 10;
            int z1 = 0;
            //y(строка), х(столбец) - размер таблицы(10, 5 ?)
            //основной массив
            int[,] myArray = new int[10, 2] { { 0, 1 }, { 0, 2 }, { 0, 3 }, { 1, 4 }, { 0, 5 }, { 1, 6 }, { 0, 7 }, { 0, 8 }, { 0, 9 }, { 1, 10 } };
            //узнаем кол-во проблемных сфетофоров
            for (int y1 = 0; y1 < y; y1++)
            {
                if (myArray[y1, x] == 1)
                {
                    z++;
                }
            }
            //массив с проблемыми сфетофорами
            int[,] Massof_on = new int[z, 2];
            for (int y1 = 0; y1 < y; y1++)
            {
                if (myArray[y1, x] == 1)
                {
                    Massof_on[z1, 0] = y1;
                    Massof_on[z1, 1] = x + 1;
                    Console.Write("{0},", Massof_on[z1, 0]);
                    Console.WriteLine(Massof_on[z1, 1]);
                    z1++;
                }
            }
            Console.ReadKey();
        }
    }
}
