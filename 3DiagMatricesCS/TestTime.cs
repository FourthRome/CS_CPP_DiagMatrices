using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Diagnostics;

namespace _3DiagMatricesCS
{
    [Serializable]
    public class TestTime
    {
        //---------------
        // Private fields
        //---------------
        List<string> log;

        //------------------
        // Public properties
        //------------------


        //-------------
        // Constructors
        //-------------
        public TestTime()
        {
            log = new List<string>();
        }

        //--------
        // Methods
        //--------
        public void Add(string record)
        {
            log.Add(record);
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            foreach (var logString in log)
            {
                result.AppendLine(logString);
            }
            return result.ToString();
        }

        //---------------
        // Static methods
        //---------------
        public static bool Save(string filename, TestTime obj)
        {
            FileStream file = null;
            bool result = true;

            try
            {
                file = new FileStream(filename, FileMode.Create);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(file, obj);
            }
            catch (Exception e)
            {
                Console.WriteLine($"_3DiagMatricesCS.Save: Failed to save log to file. Error message: {e.Message}");
                result = false;
            }
            finally
            {
                if (file != null)
                {
                    file.Close();
                }
            }
            return result;

        }

        public static bool Load(string filename, ref TestTime obj)
        {
            FileStream file = null;
            bool result = true;

            try
            {
                file = new FileStream(filename, FileMode.Open);
                BinaryFormatter formatter = new BinaryFormatter();
                obj = formatter.Deserialize(file) as TestTime;
            }
            catch (Exception e)
            {
                Console.WriteLine($"_3DiagMatricesCS.Load: Failed to load log from file. Error message: {e.Message}");
                result = false;
            }
            finally
            {
                if (file != null)
                {
                    file.Close();
                }
            }
            return result;
        }

        public static string FormatString(int matDim, int bloDim, double csTime, double cppTime, double ratio)
        {
            return $"Порядок матрицы: {matDim}, порядок блока: {bloDim}, время C#: {csTime} миллисекунд, С++: {cppTime} миллисекунд, коэффициент C#/C++: {ratio}.";
        }
    }
}
