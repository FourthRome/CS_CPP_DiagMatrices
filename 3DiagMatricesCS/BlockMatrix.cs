﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DiagMatricesCS
{
    class BlockMatrix  // Class to represent 3-diagonal matrix consisting of diagonal blocks
    {
        // Constants

        // Private fields
        uint blockDimension;
        uint matrixDimension;
        Block[][] matrix;  // First index is diagonal number: 0=A, 1=C, 2=B; second is the number of element in this diagonal

        // Public properties
        public uint MatrixDimension
        {
            get { return matrixDimension; }
            private set
            {
                if (matrixDimension <= 0)
                {
                    throw new ArgumentException("_3DiagMatricesCS.BlockMatrix.MatrixDimension: matrix dimension should be a positive number.");
                }
                matrixDimension = value;
            }
        }

        public uint BlockDimension
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

        // Constructors
        BlockMatrix(uint matrixDimension, uint blockDimension, params double[] values)  // Most default constructor
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
                for (uint i = 0; i < 3; ++i)
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
                    for (uint j = 0; j < MatrixDimension; ++j)  // For every block in each diagonal
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
                    for (uint i = 0; i < 3; ++i)  // For every diagonal
                    {
                        for (uint j = 0; j < MatrixDimension - 2; ++j)  // For every non-end block in the diagonal
                        {
                            matrix[i][j] = new Block(BlockDimension, values[2 + 3 * j + i]);  // Offset + number of the row + number of the diagonal
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
                    for (uint i = 0; i < 3; ++i)  // For every diagonal
                    {
                        for (uint j = 0; j < MatrixDimension - 2; ++j)  // For every non-end block in the diagonal
                        {
                            matrix[i][j] = new Block(BlockDimension, values,
                                                     start: (2 + 3 * j + i) * BlockDimension);   // (Offset + number of the row + number of the diagonal)
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
            for (uint i = 0; i < 3; ++i)  // For each diagonal
            {
                matrix[i] = new Block[MatrixDimension];  // Allocate memory for the diagonal
                for (uint j = 0; j < MatrixDimension; ++j)  // For each block of the diagonal
                {
                    matrix[i][j] = new Block(other.matrix[i][j]);  // Don't forget to make a copy
                }
            }
        }

        BlockMatrix(uint matrixDimension, Block[] aDiag, Block[] cDiag, Block[] bDiag)
        {

        }

        BlockMatrix(uint matrixDimension, uint blockDimension, double[] aDiag, double[] cDiag, double[] bDiag)
        {

        }

        BlockMatrix(uint matrixDimension, uint blockDimension, double[][] values)
        {

        }

        // Operators
        public static double[] operator*(BlockMatrix block, double[] vec)
        {
            double[] result = 
        }

    }
}