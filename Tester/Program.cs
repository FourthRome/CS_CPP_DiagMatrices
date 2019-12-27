using System;
using System.Diagnostics;

namespace Tester
{
    using _3DiagMatricesCS;
    class Program
    {
        static string logFilename = "testtime.log";

        static void Main(string[] args)
        {
            Test();
            Console.WriteLine();

            TestTime log = new TestTime();
            if (TestTime.Load(logFilename, ref log) == false)
            {
                Console.WriteLine("Не получилось загрузить лог; будет создан пустой лог.");
                Console.WriteLine();
            }

            Console.WriteLine("Данная программа сравнивает время решения линейных уравнений с блочной трёхдиагональной матрицей в коде на C# и на C++.");
            Console.WriteLine();

            bool continueTests = true;
            while (continueTests)
            {
                Console.WriteLine("Провести тест? Введите 'y' для продолжения, 'n' для выхода из программы");
                string userInput = Console.ReadLine();
                Console.WriteLine();

                if (userInput == "n")
                {
                    continueTests = false;
                }
                else if (userInput != "y")
                {
                    Console.WriteLine("Команда введена с ошибками; возврат в главное меню.");
                    Console.WriteLine();
                    continue;
                }
                else
                {
                    int matDim, bloDim;  // Dimensions

                    Console.WriteLine("Введите блочный порядок матрицы (количество блоков вдоль каждой из сторон матрицы):");
                    try  // Getting matrix dimension
                    {
                        matDim = int.Parse(Console.ReadLine());
                        if (matDim < 2)
                        {
                            throw new ArgumentException("Порядок матрицы должен быть натуральным числом, не меньшим 2.");
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Порядок блочной матрицы введён с ошибками");
                        Console.WriteLine();
                        Console.WriteLine($"Сообщение об ошибке: {e.Message}");
                        Console.WriteLine();
                        continue;
                    }

                    Console.WriteLine("Введите порядок блока (количество чисел вдоль каждой из сторон блока):");
                    try  // Getting block dimension
                    {
                        bloDim = int.Parse(Console.ReadLine());
                        if (bloDim < 1)
                        {
                            throw new ArgumentException("Порядок блока должен быть натуральным числом, не меньшим 1.");
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Порядок блока введён с ошибками");
                        Console.WriteLine();
                        Console.WriteLine($"Сообщение об ошибке: {e.Message}");
                        Console.WriteLine();
                        continue;
                    }

                    // Preparations

                    double[] rightSide = null;
                    double csTime = 0, cppTime = 0;
                    double[] csResult = null;
                    double[] cppResult = null;

                    try  // Allocating memory
                    {
                        rightSide = new double[matDim * bloDim];
                        for (int i = 0; i < rightSide.Length; ++i)
                        {
                            rightSide[i] = 1;
                        }
                        csResult = new double[rightSide.Length];
                        cppResult = new double[rightSide.Length];
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Не удалось выделить память под массивы правой части/решений. Вероятно, были указаны слишком большие размеры матрицы.");
                        Console.WriteLine();
                        Console.WriteLine($"Сообщение об ошибке: {e.Message}");
                        Console.WriteLine();
                        continue;
                    }

                    try  // C# solution
                    {
                        BlockMatrix mat = new BlockMatrix(matDim, bloDim);
                        Stopwatch watch = new Stopwatch();

                        try
                        {
                            watch.Start();
                            BlockMatrix.SolveSystem(mat, rightSide, csResult);
                            watch.Stop();
                        }
                        catch (ArithmeticException e)
                        {
                            Console.WriteLine("К сожалению, к данной матрице метод прогонки неприменим. Попробуйте взять матрицу с диагональным преобладанием.");
                            Console.WriteLine();
                            Console.WriteLine($"Сообщение об ошибке: {e.Message}");
                            Console.WriteLine();
                            continue;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            continue;
                        }
                        finally
                        {
                            watch.Stop();
                        }

                        csTime = watch.ElapsedMilliseconds;
                        Console.WriteLine($"Решение на C# успешно завершилось за {csTime} миллисекунд.");
                        Console.WriteLine();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Something broke unexpectedly: {e.Message}");
                        if (e.InnerException != null)
                        {
                            Console.WriteLine(e.InnerException.Message);
                        }

                        continue;
                    }

                    try  // CPP solution
                    {
                        
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Something broke unexpectedly: {e.Message}");
                        if (e.InnerException != null)
                        {
                            Console.WriteLine(e.InnerException.Message);
                        }

                        continue;
                    }


                    log.Add(TestTime.FormatString(matDim, bloDim, csTime, cppTime, 0));
                }
            }

            if (TestTime.Save(logFilename, log) == false)
            {
                Console.WriteLine("Не получилось сохранить лог; результаты данного запуска программы не сохранены.");
            }

            Console.WriteLine("Завершаем работу программы. Содержимое журнала:");
            Console.WriteLine();

            Console.WriteLine(log);

            Console.WriteLine("Нажмите ENTER для завершения...");
            Console.ReadLine();
        }

        static void Test()
        {
            Console.WriteLine("Запускаем Test() ...");
            Console.WriteLine();
            // Checking equation solution methods here
            double[] matrixValues = new double[]
            {
                -5, 14, -17,    // C0
                80, 24, -92,    // B0
                -32, -40, 94,   // A1
                -87, 42, 91,    // C1
                -13, -68, 12,   // B1
                -72, -89, -44,  // A2
                62, -66, -45    // C2
            };

            double[] rightSide = new double[]
            {
                51, -27, -59, 14, 70, 86, -7, -86, -12
            };

            BlockMatrix mat = new BlockMatrix(3, 3, matrixValues);

            Console.WriteLine("Решаем уравнение с матрицей:");
            Console.WriteLine();
            Console.WriteLine(mat);
            Console.WriteLine();
            Console.WriteLine("И правой частью:");
            Console.WriteLine();
            Console.WriteLine(rightSide.ToStringRow());
            Console.WriteLine();

            double[] csResult = new double[rightSide.Length];
            try
            {
                BlockMatrix.SolveSystem(mat, rightSide, csResult);
            }
            catch (ArithmeticException e)
            {
                Console.WriteLine("К сожалению, к данной матрице метод прогонки неприменим. Попробуйте взять матрицу с диагональным преобладанием.");
                Console.WriteLine();
                Console.WriteLine($"Сообщение об ошибке: {e.Message}");
                Console.WriteLine();
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return;
            }

            Console.WriteLine("Код на C# выдал решение:");
            Console.WriteLine();
            Console.WriteLine(csResult.ToStringColumn());
            Console.WriteLine();

            Console.WriteLine("Которое при умножении на матрицу слева даёт:");
            Console.WriteLine();
            Console.WriteLine((mat * csResult).ToStringRow());
            Console.WriteLine();
        }

        static void SolveEquationCpp()
        {

        }
    }
}
