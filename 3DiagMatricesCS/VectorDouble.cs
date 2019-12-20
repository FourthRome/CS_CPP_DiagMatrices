using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DiagMatricesCS
{
    struct VectorDouble
    {
        double[] vec;

        public int Length
        {
            get
            {
                return vec.Length;
            }
        }

        public VectorDouble(double[] source, int start=0, int size=0)
        {
            if (source.Length < start + size)
            {
                throw new ArgumentException("_3DiagMatricesCS.VectorDouble.VectorDouble: Not enough elements in source double[] array.");
            }

            if (size == 0)
            {
                size = (int)source.Length - start;
            }

            vec = new double[size];
            for (int i = 0; i < size; ++i)
            {
                vec[i] = source[start + i];
            }
        }


        public static double[] operator+(VectorDouble v, double[] arr)
        {
            if (v.Length != arr.Length)
            {
                throw new ArgumentException("_3DiagMatricesCS.VectorDouble.operator+: Dimensions do not match.");
            }

            double[] result = new double[v.Length];

            for (int i = 0; i < v.Length; ++i)
            {
                result[i] = v.vec[i] + arr[i];
            }
            return result;
        }

        public static double[] operator-(VectorDouble v, double[] arr)
        {
            if (v.Length != arr.Length)
            {
                throw new ArgumentException("_3DiagMatricesCS.VectorDouble.operator+: Dimensions do not match.");
            }

            double[] result = new double[v.Length];

            for (int i = 0; i < v.Length; ++i)
            {
                result[i] = v.vec[i] - arr[i];
            }
            return result;
        }

        //public static explicit operator VectorDouble(double[] arr)
        //{
        //    return new VectorDouble(arr);
        //}
    }
}
