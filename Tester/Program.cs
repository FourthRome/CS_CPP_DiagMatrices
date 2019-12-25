using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tester
{
    using _3DiagMatricesCS;
    class Program
    {
        static void Main(string[] args)
        {
            Test();

            TestTime log = new TestTime();
            TestTime.Load("testtime.log", ref log);


            Console.WriteLine("Данная программа сравнивает время решения линейных уравнений с блочной трёхдиагональной матрицей в коде на C# и на C++.");
            Console.WriteLine();

            bool continueTests = true;
            while (continueTests)
            {
                Console.WriteLine("Провести тест? Введите 'y' для продолжения, 'n' для выхода из программы");
                string userInput = Console.ReadLine();

                if (userInput == "n")
                {
                    break;
                }
                else if (userInput != "y")
                {
                    Console.WriteLine("Команда введена с ошибками; возврат в главное меню.");
                    Console.WriteLine();
                    continue;
                }
                else
                {
                    int matDim, bloDim;

                    Console.WriteLine("Введите блочный порядок матрицы (количество блоков вдоль каждой из сторон матрицы):");
                    try
                    {
                        matDim = int.Parse(Console.ReadLine());
                    }
                    catch
                    {
                        Console.WriteLine("Порядок блочной матрицы введён с ошибками; возврат в главное меню.");
                        Console.WriteLine();
                        continue;
                    }

                    Console.WriteLine("Введите порядок блока (количество чисел вдоль каждой из сторон блока):");
                    try
                    {
                        bloDim = int.Parse(Console.ReadLine());
                    }
                    catch
                    {
                        Console.WriteLine("Порядок блока введён с ошибками; возврат в главное меню.");
                        Console.WriteLine();
                        continue;
                    }

                    try
                    {

                        BlockMatrix mat = new BlockMatrix(matDim, bloDim);
                        Console.WriteLine(mat);
                        //double time = BlockMatrix.SolveSystem(mat)
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Something broke unexpectedly: {e.Message}");
                        if (e.InnerException != null)
                        {
                            Console.WriteLine(e.InnerException.Message);
                        }
                        
                    }

                    
                    
                }

                

            }





        }

        static void Test()
        {
            // Checking equation solution methods here 
        }

        static void SolveEquationCpp()
        {

        }
    }
}
