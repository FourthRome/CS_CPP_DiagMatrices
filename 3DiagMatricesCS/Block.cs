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
        //----------
        // Constants
        //----------
        static double EPSILON = 1e-10;

        //---------------
        // Private fields
        //---------------
        uint dimension;
        double[] matrix;

        //------------------
        // Public properties
        //------------------
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
   
        //-------------
        // Constructors
        //-------------
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

        //----------
        // Operators
        //----------
        public static Block operator+(Block a, Block b)  // Block addition
        {
            Block result = new Block(a.Dimension);
            for (uint i = 0; i < a.Dimension; ++i)
            {
                result.matrix[i] = a.matrix[i] + b.matrix[i];
            }
            return result;
        }

        public static Block operator-(Block a)  // Block negation
        {
            Block result = new Block(a.Dimension);
            for (uint i = 0; i < a.Dimension; ++i)
            {
                result.matrix[i] = -a.matrix[i];
            }
            return result;
        }

        public static Block operator-(Block a, Block b)  // Block subtraction
        {
            Block result = new Block(a.Dimension);
            for (uint i = 0; i < a.Dimension; ++i)
            {
                result.matrix[i] = a.matrix[i] - b.matrix[i];
            }
            return result;
        }

        public static Block operator*(Block a, Block b)  // Block multiplication
        {
            Block result = new Block(a.Dimension);
            for (uint i = 0; i < a.Dimension; ++i)
            {
                result.matrix[i] = a.matrix[i] * b.matrix[i];
            }
            return result;
        }

        
        public static double[] operator*(Block block, double[] vec)  // Block-vector multiplication
        {
            if (vec.Length != block.Dimension)
            {
                throw new ArgumentException("_3DiagMatricesCS.Block.operator*: Dimensions do not match.");
            }
            
            double[] result = new double[block.Dimension];
            for (uint i = 0; i < block.Dimension; ++i)
            {
                result[i] = block.matrix[i] * vec[i];
            }
            return result;
        }

        //--------
        // Methods
        //--------
        public double[] MultiplyWithVec(double[] vec, uint start=0,
                                        double[] result=null, uint resultStart=0)  // Block-vector multiplication (version with array of bigger size)
        {
            if (start + Dimension > vec.Length)  // Check if start and len are specified correctly (vector can be longer than block)
            {
                throw new ArgumentException("_3DiagMatricesCS.Block.MultiplyWithVec: Vector doesn't have enough values in it (check vec and start parameters).");
            }

            if (result == null)  // Create new array if storage for the result was not provided
            {
                result = new double[Dimension];
            }

            if (resultStart + Dimension > result.Length)  // Check if there is enough space to store the result (it should not 
            {
                throw new ArgumentException("_3DiagMatricesCS.Block.MultiplyWithVec: Not enough space to store the result (check result and resultStart parameters).");
            }

            for (uint i = 0; i < Dimension; ++i)
            {
                result[resultStart + i] = matrix[i] * vec[start + i];
            }
            return result;  // Note: return value can be an existing array passed to the function as a parameter.
                            // Does this have any effect on GC? Should we consider returning null instead?
        }

        
        public bool isSingular()  // Needed in Block.inverse() method
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

        //---------------
        // Static methods
        //---------------

        public static Block inverse(Block block)  // Calculate inverse matrix
        {
            if (block.isSingular())  // Check if inverse matrix exists
            {
                throw new ArithmeticException("_3DiagMatricesCS.Block.inverse: Can't compute inverse of a singular matrix.");
            }

            Block result = new Block(block.Dimension);
            for (uint i = 0; i < block.Dimension; ++i)  // Luckily, our matrix is diagonal, so 
            {
                result.matrix[i] = 1 / block.matrix[i];
            }
            return result;
        }

        
    }
}
