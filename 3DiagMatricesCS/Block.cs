using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DiagMatricesCS
{
    public class Block  // Class to represent a block in a block matrix.
                        // Each block here is a diagonal matrix of an arbitrary dimension. 
    {
        // Constants
        static double EPSILON = 1e-10;

        // Private fields
        uint dimension;
        double[] matrix;

        // Public properties
        public uint Dimension
        {
            get { return dimension; }
            private set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("_3DiagMatricesCS.Block.Block: Failed to create block: block dimension should be a positive number.");
                }
                dimension = value;
            }
        }
   
        // Constructors
        public Block(uint dimension, params double[] values)  // Most default constructor
        {
            try // Set up block dimension and forward exceptions, if any
            {
                Dimension = dimension;  // Possible exception
            }
            catch (ArgumentException e)
            {
                throw new ArgumentException("_3DiagMatricesCS.Block.Block: The first argument is incorrect.", e);
            }
            
            // Create matrix
            if (values.Length == 0  // Matrix filled with 0s
                || values.Length == 1  //  Scalar matrix
                || values.Length == dimension)  //  General diagonal matrix
            {
                matrix = new double[Dimension];  // Number of arguments is acceptable, so we can allocate memory

                // Initialize matrix elements
                if (values.Length == 0)  // Create an empty matrix of the specified size
                {
                    for (uint i = 0; i < Dimension; ++i)
                    {
                        matrix[i] = 0;
                    }
                }
                else if (values.Length == 1)  // Create scalar matrix
                {
                    for (uint i = 0; i < Dimension; ++i)
                    {
                        matrix[i] = values[0];
                    }
                }
                else  // Each element of a diagonal matrix is provided explicitly
                {
                    for (uint i = 0; i < Dimension; ++i)
                    {
                        matrix[i] = values[i];
                    }
                }
            }
            else  // No form of diagonal matrix creation can be applied; some parameters are missing/there are extra parameters
            {
                throw new ArgumentException("_3DiagMatricesCS.Block.Block: Can't derive values from this amount of parameters.");
            }
        }

        public Block(Block other)  // Make a deep copy of another Block
        {
            Dimension = other.Dimension;  // No exception expected
            matrix = new double[Dimension];
            for (uint i = 0; i < Dimension; ++i)
            {
                matrix[i] = other.matrix[i];
            }
        }

        public Block(uint dimension, double[] values, uint start = 0)  // Construct Block from double[]
        {
            try // Set up block dimension and forward exceptions, if any
            {
                Dimension = dimension;  // Possible exception
                if (values.Length - start < dimension)  // Avoid IndexOutOfRange exception
                {
                    throw new ArgumentException("_3DiagMatricesCS.Block.Block(double[]): Array's length is smaller than the dimension specified.");
                }
            }
            catch (ArgumentException e)
            {
                throw new ArgumentException("_3DiagMatricesCS.Block.Block(double[]): Some arguments are incorrect.", e);
            }

            matrix = new double[Dimension];  // Arguments are acceptable, so we can allocate memory

            for (uint i = 0; i < Dimension; ++i)  // Initialize matrix with values from double[], starting from the "start" index
            {
                matrix[i] = values[start + i];
            }
        }

        // Operators
        // Block addition
        public static Block operator+(Block a, Block b)
        {
            Block result = new Block(a.Dimension);
            for (uint i = 0; i < a.Dimension; ++i)
            {
                result.matrix[i] = a.matrix[i] + b.matrix[i];
            }
            return result;
        }

        // Block negation
        public static Block operator-(Block a)
        {
            Block result = new Block(a.Dimension);
            for (uint i = 0; i < a.Dimension; ++i)
            {
                result.matrix[i] = -a.matrix[i];
            }
            return result;
        }

        // Block subtraction
        public static Block operator-(Block a, Block b)
        {
            Block result = new Block(a.Dimension);
            for (uint i = 0; i < a.Dimension; ++i)
            {
                result.matrix[i] = a.matrix[i] - b.matrix[i];
            }
            return result;
        }

        // Block multiplication
        public static Block operator*(Block a, Block b)
        {
            Block result = new Block(a.Dimension);
            for (uint i = 0; i < a.Dimension; ++i)
            {
                result.matrix[i] = a.matrix[i] * b.matrix[i];
            }
            return result;
        }

        // Block-vector multiplication
        public static double[] operator*(Block block, double[] vec)
        {
            if (vec.Length != block.Dimension)
            {
                throw new ArgumentException("_3DiagMatricesCS.Block.operator*: Dimensions do not match.");
            }
            else
            {
                double[] result = new double[block.Dimension];
                for (uint i = 0; i < block.Dimension; ++i)
                {
                    result[i] = block.matrix[i] * vec[i];
                }
                return result;
            }
        }

        // Block-vector multiplication (version with array of bigger size)
        public double[] MultiplyWithVec(double[] vec, uint start=0, uint len=0, double[] result=null)
        {
            if (start + len > vec.Length)
            {
                throw new ArgumentException("_3DiagMatricesCS.Block.MultiplyWithVec: .")
            }

        }

        // Calculate inverse matrix
        public static Block inverse(Block block)
        {
            if (block.isSingular())
            {
                throw new ArithmeticException("_3DiagMatricesCS.Block.inverse: can't compute inverse of a singular matrix.");
            }

            Block result = new Block(block.Dimension);
            for (uint i = 0; i < block.Dimension; ++i)
            {
                result.matrix[i] = 1 / block.matrix[i];
            }
            return result;
        }

        // A utility to check matrix
        public bool isSingular()
        {
            for (uint i = 0; i < Dimension; ++i)
            {
                if (Math.Abs(matrix[i]) < EPSILON)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
