using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            int z = 0; //кол-во проблемных сфетофоров
            int x = 0;
            int y = 10;
            int z1 = 0;
            //y(строка), х(столбец) - размер таблицы(10, 5 ?)
            //основной массив, данные для формулы
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
            //инициализация Excel
            Excel.Application xlApp = new Excel.Application(); //открыть эксель
            Excel.Workbook xlWb = xlApp.Workbooks.Open(@"C:\Users\Администратор.000\Desktop\AdaptiveLight.xlsx");
            Excel.Worksheet xlSht = (Excel.Worksheet)xlApp.Sheets[1]; //получить 1 лист
            try
            {
                int r = 4; //кол-во направлений на перекрёстке, пока равно 4
                int R1 = 0;
                //массив отношения перекрёстков и направлениям к машинам
                int[][,] JagArrofData = new int[10][,];
                for (int i = 0; i < 10; i++)
                {
                    JagArrofData[i] = new int[r, 4]; //размер каждого массива 4,4
                    for (int C = 0; C < r; C++)
                    {
                        for (int R = 0; R < 4; R++)
                        {
                            //заполнение массива из Excel
                            JagArrofData[i][R, C] = Convert.ToInt32(xlSht.Cells[R1 + 3, C + 8].Text);
                            Console.Write(JagArrofData[i][R + 1, C]);
                        }
                    }
                    R1++;
                    //Console.WriteLine();
                }
                xlWb.Close(false); //закрываем файл
                xlApp.Quit(); //закрываем Excel
            }
            catch (Exception)
            {
                xlWb.Close(false);
                xlApp.Quit();
                // обрабатываем ошибку
            }

            {
                Console.ReadKey();
            }
        }
    }
}
