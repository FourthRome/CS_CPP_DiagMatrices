using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace _3DiagMatricesCS
{
    class BlockMatrix  // Class to represent 3-diagonal matrix consisting of diagonal blocks
    {
        //----------
        // Constants
        //----------

        //---------------
        // Private fields
        //---------------
        int blockDimension;
        int matrixDimension;
        Block[][] matrix;  // First index is diagonal number: 0=A, 1=C, 2=B; second is the number of element in this diagonal

        //------------------
        // Public properties
        //------------------
        public int MatrixDimension
        {
            get { return matrixDimension; }
            private set
            {
                if (matrixDimension <= 1)
                {
                    throw new ArgumentException("_3DiagMatricesCS.BlockMatrix.MatrixDimension: block matrix dimension should be 2 or bigger.");
                }
                matrixDimension = value;
            }
        }

        public int BlockDimension
        {
            get { return BlockDimension; }
            private set
            {
                if (blockDimension <= 0)
                {
                    throw new ArgumentException("_3DiagMatricesCS.BlockMatrix.BlockDimension: block dimension should be a positive number.");
                }
                blockDimension = value;
            }
        }

        //-------------
        // Constructors
        //-------------
        BlockMatrix(int matrixDimension, int blockDimension, params double[] values)  // Most default constructor
        {
            try  // Set up matrix dimensions and forward exceptions, if any
            {
                MatrixDimension = matrixDimension;
                BlockDimension = blockDimension;
            }
            catch (ArgumentException e)
            {
                throw new ArgumentException("_3DiagMatricesCS.BlockMatrix.BlockMatrix: At least one of the dimensions is incorrect.", e);
            }

            // Create matrix
            if (values.Length == 0  // A matrix for Test() (from "Tester" project)
                || values.Length == 1  // A scalar 1-diag matrix
                || values.Length == 3  // A 3-diag matrix where each diagonal is scalar
                || values.Length == 3 * matrixDimension - 2  // A 3-diag matrix where each block is scalar
                || values.Length == (3 * matrixDimension - 2) * blockDimension)  // A general 3-diag matrix
            {
                matrix = new Block[3][];  // Number of arguments is acceptable, so we can allocate memory
                for (int i = 0; i < 3; ++i)
                {
                    matrix[i] = new Block[MatrixDimension];
                }

                // Initialize matrix elements
                if (values.Length == 0)  // Create default matrix - it is used in Test (from "Tester" project)
                {
                    

                }
                else if (values.Length == 1)  // Create 1-diagonal scalar matrix (each block of the main diagonal is scalar with the same value)
                {
                    for (int j = 0; j < MatrixDimension; ++j)  // For every block in each diagonal
                    {
                        matrix[0][j] = new Block(BlockDimension);               // A diagonal (filled with 0s)
                        matrix[1][j] = new Block(BlockDimension, values[0]);    // C diagonal (scalar)
                        matrix[2][j] = new Block(BlockDimension);               // B diagonal (filled with 0s)
                    }
                }
                else if (values.Length == 3)  // Create 3-diagonal matrix where each diagonal is scalar.
                                              // (Blocks are scalar and equal along each diagonal)
                {
                    for (int j = 0; j < MatrixDimension; ++j)  // For every block in each diagonal
                    {
                        matrix[0][j] = new Block(BlockDimension, values[0]);    // A diagonal (scalar)
                        matrix[1][j] = new Block(BlockDimension, values[1]);    // C diagonal (scalar)
                        matrix[2][j] = new Block(BlockDimension, values[2]);    // B diagonal (scalar)
                    }
                }
                else if (values.Length == 3 * MatrixDimension - 2)  // Create 3-diagonal matrix where each block is a scalar matrix.
                                                                    // Blocks are different (but each of them is scalar).
                                                                    // Blocks are initialized in the following order (row-by-row):
                                                                    // C(0), B(0), A(1), C(1), B(1), ... , A(MatrixDimension - 1), C(MatrixDimension - 1)
                {
                    // Initialize the first row (row 0)
                    matrix[0][0] = new Block(BlockDimension);             // A(0) block (filled with 0s)
                    matrix[1][0] = new Block(BlockDimension, values[0]);  // C(0) block (scalar)
                    matrix[2][0] = new Block(BlockDimension, values[1]);  // B(0) block (scalar)

                    // Initialize rows 1 to (BlockDimension - 2)
                    for (int i = 0; i < 3; ++i)  // For every diagonal
                    {
                        for (int j = 1; j < MatrixDimension - 1; ++j)  // For every non-end block in the diagonal
                        {
                            matrix[i][j] = new Block(BlockDimension, values[2 + 3 * (j - 1) + i]);  // First rows' offset + number of the diagonal
                        }
                    }

                    // Initialize the last row
                    matrix[0][MatrixDimension - 1] = new Block(BlockDimension, values[3 * MatrixDimension - 4]);   // A(MatrixDimension - 1) block (scalar)
                    matrix[1][MatrixDimension - 1] = new Block(BlockDimension, values[3 * MatrixDimension - 3]);  // C(MatrixDimension - 1) block (scalar)
                    matrix[2][MatrixDimension - 1] = new Block(BlockDimension);                                   // B(MatrixDimension - 1) block (filled with 0s)
                }
                else   // Create 3-diagonal matrix where each block is an arbitrary diagonal matrix
                       // Matrix is filled like this (row-by-row for blocks, but each block is provided at once):
                       // C(0), B(0), A(1), C(1), B(1), ... , A(matrixDimension - 1), C(matrixDimension - 1)
                {
                    // Initialize the first row (row 0)
                    matrix[0][0] = new Block(BlockDimension);             // A(0) block (filled with 0s)
                    matrix[1][0] = new Block(BlockDimension, values, start:0);  // C(0) block
                    matrix[2][0] = new Block(BlockDimension, values, start:BlockDimension);  // B(0) block

                    // Initialize rows 1 to (BlockDimension - 2)
                    for (int i = 0; i < 3; ++i)  // For every diagonal
                    {
                        for (int j = 1; j < MatrixDimension - 1; ++j)  // For every non-end block in the diagonal
                        {
                            matrix[i][j] = new Block(BlockDimension, values,
                                                     start: (2 + 3 * (j - 1) + i) * BlockDimension);  // (First rows' offset + number of the diagonal)
                                                                                                 // times BlockDimension is the starting index for the
                                                                                                 // current block's parameters
                        }
                    }

                    // Initialize the last row
                    matrix[0][MatrixDimension - 1] = new Block(BlockDimension, values,
                                                               start: (3 * MatrixDimension - 4) * BlockDimension);  // A(MatrixDimension - 1) block
                    matrix[1][MatrixDimension - 1] = new Block(BlockDimension, values,
                                                               start: (3 * MatrixDimension - 3) * BlockDimension);  // C(MatrixDimension - 1) block
                    matrix[2][MatrixDimension - 1] = new Block(BlockDimension);                                     // B(MatrixDimension - 1) block (filled with 0s)
                }
            }
            else  // No form of block matrix creation can be applied; some parameters are missing/there are extra parameters
            {
                throw new ArgumentException("_3DiagMatricesCS.BlockMatrix.BlockMatrix: Can't derive values from this amount of parameters.");
            }
        }

        BlockMatrix(BlockMatrix other) // Make a deep copy of another BlockMatrix
        {
            MatrixDimension = other.MatrixDimension;  // No exceptions expected here
            BlockDimension = other.BlockDimension;

            matrix = new Block[3][];  // Allocate memory for the matrix
            for (int i = 0; i < 3; ++i)  // For each diagonal
            {
                matrix[i] = new Block[MatrixDimension];  // Allocate memory for the diagonal
                for (int j = 0; j < MatrixDimension; ++j)  // For each block of the diagonal
                {
                    matrix[i][j] = new Block(other.matrix[i][j]);  // Don't forget to make a copy
                }
            }
        }

        BlockMatrix(int matrixDimension, Block[] aDiag, Block[] cDiag, Block[] bDiag)
        {

        }

        BlockMatrix(int matrixDimension, int blockDimension, double[] aDiag, double[] cDiag, double[] bDiag)
        {

        }

        BlockMatrix(int matrixDimension, int blockDimension, double[][] values)
        {

        }

        //----------
        // Operators
        //----------
        public static double[] operator*(BlockMatrix mat, double[] vec)
        {
            int matDim = mat.MatrixDimension;  // Aliases for dimensions
            int bloDim = mat.BlockDimension;   // (first written without them, the code was too large)

            if (vec.Length != matDim * bloDim)
            {
                throw new ArgumentException("_3DiagMatricesCS.BlockMatrix.operator*: Dimensions do not match.");
            }

            double[] result = new double[matDim * bloDim];  // Initialized with zeroes, just to be safe
            double[][] buffers = new double[3][];  // These buffers can be omitted if summation of two double[] objects is defined (element-by-element)
            for (int i = 0; i < 3; ++i)
            {
                buffers[i] = new double[bloDim];
            }

            // First row has only two blocks
            mat.matrix[1][0].MultiplyWithVec(vec, start: 0, result: buffers[1]);  // C(0) * vec[0 : BlockDimension]
            mat.matrix[2][0].MultiplyWithVec(vec, start: bloDim, result: buffers[2]);  // B(0) * vec[BlockDimension : 2 * BlockDimension]
            for (int k = 0; k < bloDim; ++k)  // Sum buffers element-by-element
            {
                result[k] = buffers[1][k] + buffers[2][k];
            }


            // Compute rows that are not in corner blocks (neither first nor last block)
            for (int j = 1; j < matDim - j; ++j)  // For every non-end row
            {
                for (int i = 0; i < 3; ++i)  // For every diagonal (A, C, B)
                {
                    // Multiply one block with a subset of vec
                    mat.matrix[i][j].MultiplyWithVec(vec, start: (j + i - 1) * bloDim, result: buffers[i]);
                }

                // Now we have all three double[] objects; let's sum them 

                for (int k = 0; k < bloDim; ++k)
                {
                    // Summation is hardcoded (as opposed to using a loop) on purpose: code is a huge mess already, and this place
                    // is one of the anchors that can remind us where we are
                    result[j * bloDim + k] = buffers[0][k] + buffers[1][k] + buffers[2][k];  // Buffers' length in blocks is 1, result's length in is matDim 
                }
            }

            // Last row has only two blocks
            mat.matrix[0][matDim - 1].MultiplyWithVec(vec, start: (matDim - 2) * bloDim, result: buffers[0]);  // Last A multiplication
            mat.matrix[1][matDim - 1].MultiplyWithVec(vec, start: (matDim - 1) * bloDim, result: buffers[1]);  // Last C multiplication

            for (int k = 0; k < bloDim; ++k)  // Sum buffers element-by-element
            {
                result[(matDim - 1) * bloDim + k] = buffers[0][k] + buffers[1][k];
            }

            return result;
        }

        //--------
        // Methods
        //--------

        public void SaveToFile(double[] fVec, double[] xVec, string filename="./result_cpp.txt")
        {
            FileStream file = null;
            try
            {
                file = new FileStream(filename, FileMode.Append);
                StreamWriter writer = new StreamWriter(file);

                writer.WriteLine($"Equation solved at: {DateTime.Now}");
                writer.WriteLine();
                
                writer.WriteLine("Matrix:");
                writer.WriteLine(ToString());
                writer.WriteLine();

                writer.WriteLine("Right part (transposed):");
                writer.WriteLine($"[{fVec}]");
                writer.WriteLine();

                writer.WriteLine("Solution (transposed):");
                writer.WriteLine($"[{xVec}]");
                writer.WriteLine();
                
                writer.Close();
            }
            catch (Exception e)
            {
                throw new Exception("_3DiagMatricesCS.BlockMatrix.SaveToFile: Some I/O error occurred.", e);
            }
            finally
            {
                if (file != null)
                {
                    file.Close();
                }
            }
        }

        public override string ToString()
        {
            string zeroString = (new Block(BlockDimension)).RowToString(0);
            StringBuilder buffer = new StringBuilder();

            for (int j = 0; j < MatrixDimension; ++j)
            {
                for (int k = 0; k < BlockDimension; ++k)
                {
                    for (int p = 0; p < j; ++p)
                    {
                        buffer.Append(zeroString);
                        buffer.Append(" | ");
                    }

                    for (int i = (j == 0 ? 1 : 0); i < (j == (MatrixDimension - 1) ? 2 : 3); ++i)
                    {
                        buffer.Append(matrix[i][0].RowToString(k));
                        buffer.Append(" | ");
                    }

                    for (int p = j + 1; p < BlockDimension; ++p)
                    {
                        buffer.Append(zeroString);
                        buffer.Append(" | ");
                    }
                    buffer.Length -= " | ".Length;
                    buffer.AppendLine();
                }

                buffer.AppendLine("***");
            }

            buffer.Length -= "***".Length + 1;

            return buffer.ToString();
        }

        //---------------
        // Static methods
        //---------------
        public static double[] SolveSystem(BlockMatrix mat, double[] vec, double[] result=null)
        {
            int matDim = mat.MatrixDimension;  // Aliases for dimensions
            int bloDim = mat.BlockDimension;   // (first written without them, the code was too large)

            //------------------
            // Arguments' checks
            //------------------

            if (vec.Length != matDim * bloDim)  // The right part of the equation is a plain double[]...
                                                // This will give us a lot of pain later
            {
                throw new ArgumentException("_3DiagMatricesCS.BlockMatrix.SolveSystem: Dimensions do not match.");
            }

            if (result == null)  // Create new array if storage for the result was not provided
            {
                result = new double[matDim * bloDim];
            }

            if (vec.Length > result.Length)  // Check if there is enough space to store the result
            {
                throw new ArgumentException("_3DiagMatricesCS.BlockMatrix.SolveSystem: Not enough space to store the result (check vec and result parameters).");
            }

            //--------------------------------
            // Coefficients' memory allocation
            //--------------------------------
            
            Block[] alphas = new Block[matDim];  // Coefficients: alphas are Block,
            double[][] betas = new double[matDim + 1][];  // whereas betas are double[]
            alphas[0] = new Block(bloDim);
            betas[0] = new double[bloDim];  // We rely on auto zero-initialization

            //--------------------------
            // Coefficients' calculation
            //--------------------------
            
            for (int j = 0; j < matDim - 1; ++j)  // Calculate alphas
            {
                alphas[j + 1] = Block.Inverse(mat.matrix[1][j] - mat.matrix[0][j] * alphas[j]) * mat.matrix[2][j];  // Remember: matrix[0] - A diagonal,
                                                                                                                    // matrix[1] - C diagonal,
                                                                                                                    // matrix[2] - B diagonal
            }

            for (int j = 0; j < matDim; ++j)  // Calculate betas
            {
                betas[j + 1] = Block.Inverse(mat.matrix[1][j] - mat.matrix[0][j] * alphas[j])
                               * (new VectorDouble(vec, j * bloDim, bloDim) - mat.matrix[0][j] * betas[j]);  // Here we have to introduce a wrapper struct for double[],
                                                                                                             // VectorDouble, to implement subtraction of two arrays.
                                                                                                             // The result of the subtraction is double[] again
            }
            
            //---------------------------------------
            // Result calculation (result == X array)
            //---------------------------------------

            double[] buffer = new double[bloDim];  // We have to introduce a buffer: result is a plain double[], but it is filled in portions of size bloDim each

            for (int k = 0; k < bloDim; ++k)  // Fill in X(bloDim), the last element of the answer (it is a subset of double[] result) 
            {
                result[(matDim - 1) * bloDim + k] = betas[matDim][k];
            }

            for (int j = matDim - 2; j >= 0; --j)  // Fill in other X-s, thus completing the result
            {
                alphas[j + 1].MultiplyWithVec(vec, (j + 1) * bloDim, buffer);  // Here's why we need buffer
                buffer = new VectorDouble(betas[j + 1]) - buffer;  // And here

                for (int k = 0; k < bloDim; ++k)  // If we didn't have to iterate through the values to fill this block of result,
                                                  // we could get rid of buffer and do the calculations right here; but for now we just copy values
                {
                    result[j * bloDim + k] = buffer[k];
                }
            }

            //-----------------------
            // Saving results to file
            //-----------------------

            mat.SaveToFile(vec, result);

            return result;
        }

    }
}
