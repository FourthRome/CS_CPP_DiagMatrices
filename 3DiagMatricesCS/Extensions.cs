using System;
using System.Text;

namespace _3DiagMatricesCS
{
    public static class Extensions
    {
        public static string ToStringColumn(this double[] arr)
        {
            if (arr == null)
            {
                throw new ArgumentException("_3DiagMatricesCS.Extentions.ToString(double[]): Null passed as an array.");
            }

            StringBuilder result = new StringBuilder();
            for (int i = 0; i < arr.Length; ++i)
            {
                result.AppendLine($"{arr[i]}");
            }

            return result.ToString().TrimEnd();
        }

        public static string ToStringRow(this double[] arr)
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

            return result.ToString().TrimEnd();
        }
    }
}
