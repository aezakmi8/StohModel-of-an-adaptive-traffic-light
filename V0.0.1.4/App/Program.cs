using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;
using System.Threading.Tasks;
using System.Data.Odbc;
using System.Data;

namespace ConsoleApp1
{
    class Program
    {

        //public static

        static void Main(string[] args)
        {
            StohModel AdaptiveLighter = new StohModel();
            AdaptiveLighter.db_Conn();
            //string text = Console.ReadLine();
            string result = AdaptiveLighter.GetInterval();
            Console.WriteLine(result);
            Console.Write(AdaptiveLighter.JagArrofData[1].GetLength(0));
            Console.Write("\t" + AdaptiveLighter.JagArrofData.Length);
            Console.ReadKey();
        }

        public class StohModel
        {
            public int[][,] JagArrofData = new int[10][,];

            public void diffiс_Flow()
            {
                //выявление светофоров которые необходимо разгрузить, пока не применимо
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
            }

            public void db_Conn()
            {
                try
                {
                    string connString = @"Driver={Microsoft Access Driver (*.mdb)};Dbq=C:\Users\Администратор.000\Documents\GitHub\StohModel\V0.0.1.4\App\db_Stoh_Model.mdb;";
                    DataTable DtTable = new DataTable();
                    using (OdbcConnection dbcon = new OdbcConnection(connString))
                    {
                        OdbcCommand comm = new OdbcCommand("SELECT * FROM RoadsNet", dbcon);
                        dbcon.Open();
                        OdbcDataAdapter adapter = new OdbcDataAdapter(comm);
                        adapter.Fill(DtTable);
                        dbcon.Close();
                    }
                    //массив отношения перекрёстков и направлениям к машинам

                    for (int i = 0; i < 10; i++)
                    {
                        JagArrofData[i] = new int[4, 8]; //4 строки, 8 столбцов
                        for (int a = 0; a < 4; a++) //DtTable.Rows.Count
                        {
                            JagArrofData[i][a, 0] = Convert.ToInt32(DtTable.Rows[a]["crossroad"]);
                            JagArrofData[i][a, 1] = Convert.ToInt32(DtTable.Rows[a]["refCrossrd"]);
                            JagArrofData[i][a, 2] = Convert.ToInt32(DtTable.Rows[a]["direct"]);
                            JagArrofData[i][a, 3] = Convert.ToInt32(DtTable.Rows[a]["var_Lambda"]);
                            JagArrofData[i][a, 4] = Convert.ToInt32(DtTable.Rows[a]["var_Mu"]);
                            JagArrofData[i][a, 5] = Convert.ToInt32(DtTable.Rows[a]["carsInQuery"]);
                            JagArrofData[i][a, 6] = Convert.ToInt32(DtTable.Rows[a]["criticNumb"]);
                            JagArrofData[i][a, 7] = Convert.ToInt32(DtTable.Rows[a]["timeInterval"]);
                            //Console.Write("Отсчётный светофор {0}, Светофор(j) {1}, Направление(i) {2}, Lambda {3}, Mu {4}, carsInQuery{5}, criticNumb{6}, ", i, JagArrofData[i][a, 1], JagArrofData[i][a, 2], JagArrofData[i][a, 3], JagArrofData[i][a, 4], JagArrofData[i][a, 5], JagArrofData[i][a, 6]);
                            //Console.WriteLine();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("err" + ex);
                }
            }

            public double t = 100; //единица времени
            //double[,] timeInterval = { { 3, 6, 9, 4, 7, 9, 9, 1, 2, 3 }, { 3, 5, 1, 9, 4, 8, 4, 8, 4, 2 } };

            public string GetInterval()
            {
                string list = "";
                try
                {
                    int n = 1;
                    for (int i = 0; i < JagArrofData.Length; i++)
                    {
                        double a;
                        double b;
                        int dirCount = JagArrofData[1].GetLength(0); //кол-во направлений, может меняться)
                        double[,] resultInterval = new double[n, dirCount]; //формула с суммой
                        double[,] kRInterval = new double[n, dirCount]; //коэфф e

                        for (int j = 0; j < dirCount; j++)
                        {
                            int inCar = JagArrofData[i][j, 3]; //Лямбда, без единицы времени
                            int outCar = JagArrofData[i][j, 4]; //Мю, без единицы времени
                            double carsInQuery = JagArrofData[i][j, 5]; //Хо, машины оставшиеся не пропущенными на данном направлении j,  i перекрёстка
                            double criticNumb = JagArrofData[i][j, 6]; //L, некоторое критическое число
                            double timeInterval = JagArrofData[i][j, 7]; //интервал переключения светофоров на направлениях

                            a = (Math.Pow(inCar, 2)/timeInterval + Math.Pow(outCar, 2)/timeInterval) / (2 * outCar / timeInterval) + 1;
                            b = outCar/ timeInterval - inCar/ timeInterval + 1; //зараза, может нулём стать

                            kRInterval[n - 1, j] = 2 * Math.Exp(-((2 * b * carsInQuery) + (Math.Pow(b, 2) * t)) / (4 * a));

                            resultInterval[n - 1, j] += Math.Pow(-1, (n + 1)) * ((Math.Exp((b * criticNumb) / (2 * a)) * Math.Sin((Math.PI) * n * carsInQuery / criticNumb)) + Math.Sin(Math.PI * n * ((criticNumb - carsInQuery) / criticNumb))) /
                                (Math.PI * n + ((Math.Pow(b, 2) * Math.Pow(criticNumb, 2)) / 4 * Math.PI * n * Math.Pow(a, 2))) * Math.Exp(-((Math.Pow(Math.PI, 2) * Math.Pow(n, 2) * a * t) / (Math.Pow(criticNumb, 2))));

                            list += (resultInterval[n - 1, j] * kRInterval[n - 1, j]) + "\n";
                        }
                        n++;
                    }

                }
                catch (IndexOutOfRangeException ex)
                {
                    Console.WriteLine(ex);
                }
                return list;
            }

            public string 
        }
    }
}


/*
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
        catch (Exception ex)
        {
            Console.WriteLine("err" + ex);
            xlWb.Close(false);
            xlApp.Quit();
            // обрабатываем ошибку
        }
        */
