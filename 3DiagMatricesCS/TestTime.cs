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
    class TestTime
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
            StringBuilder result = new StringBuilder("Log of performance checks on matrix equations' solutions:");
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

        public static string FormatString(int matDim, int bloDim, long csTime, long cppTime, double ratio)
        {
            return $"Equation with block matrix ({matDim} blocks wide), where each block is {bloDim} wide, was solved in " +
                   $"{csTime} milliseconds by C# program and in {cppTime} milliseconds by C++ program, giving us performance ratio of {ratio}.";
        }
    }
}
