using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DiagMatricesCS
{
    public static class Extensions
    {
        public static string ToString(this double[] arr)
        {
            if (arr == null)
            {
                throw new ArgumentException("_3DiagMatricesCS.Extentions.ToString(double[]): Null passed as an array.");
            }

            StringBuilder result = new StringBuilder();
            for (int i = 0; i < arr.Length; ++i)
            {
                result.Append($"{arr[i]} ");
            }
            
            if (arr.Length > 0)
            {
                result.Length -= 1;
            }
            return result.ToString();
        }
    }
}
