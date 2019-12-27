#include "pch.h"
#include <iostream>
#include "Block.h"

namespace _3DiagMatricesCPP
{
	class Block  // Class to represent a block in a block matrix.
				 // Each block here is a diagonal matrix of an arbitrary dimension. 
	{
		//----------
		// Constants
		//----------
		const double EPSILON = 1e-10;

		//---------------
		// Private fields
		//---------------
		int dimension;
		double *matrix;

	public:
		//-------------
		// Constructors
		//-------------
		//Block(int _dimension, double *values)  // Most default constructor
		//{
		//	dimension = _dimension;

		//	matrix = new double[dimension];  // Number of arguments is acceptable, so we can allocate memory

		//	for (int i = 0; i < dimension; ++i)
		//	{
		//		matrix[i] = values[i];
		//	}
		//}

		//public Block(Block other)  // Make a deep copy of another Block
		//{
		//	Dimension = other.Dimension;  // No exception expected
		//	matrix = new double[Dimension];
		//	for (int i = 0; i < Dimension; ++i)
		//	{
		//		matrix[i] = other.matrix[i];
		//	}
		//}

		Block(int _dimension, double *values, int start = 0)  // Construct Block from double[]
		{
			dimension = _dimension;
			matrix = new double[dimension];  // Arguments are acceptable, so we can allocate memory

			for (int i = 0; i < dimension; ++i)  // Initialize matrix with values from double[], starting from the "start" index
			{
				matrix[i] = values[start + i];
			}
		}

		//-----------
		// Destructor
		//-----------
		~Block()
		{
			delete[] matrix;
		}


		////----------
		//// Operators
		////----------
		//static Block& operator+(Block a, Block b)  // Block addition
		//{
		//	Block result = new Block(a.dimension);
		//	for (int i = 0; i < a.Dimension; ++i)
		//	{
		//		result.matrix[i] = a.matrix[i] + b.matrix[i];
		//	}
		//	return result;
		//}

		//static Block& operator-(Block a)  // Block negation
		//{
		//	Block result = new Block(a.Dimension);
		//	for (int i = 0; i < a.Dimension; ++i)
		//	{
		//		result.matrix[i] = -a.matrix[i];
		//	}
		//	return result;
		//}

		//static Block& operator-(Block a, Block b)  // Block subtraction
		//{
		//	Block result = new Block(a.Dimension);
		//	for (int i = 0; i < a.Dimension; ++i)
		//	{
		//		result.matrix[i] = a.matrix[i] - b.matrix[i];
		//	}
		//	return result;
		//}

		//static Block& operator*(Block a, Block b)  // Block multiplication
		//{
		//	Block result = new Block(a.Dimension);
		//	for (int i = 0; i < a.Dimension; ++i)
		//	{
		//		result.matrix[i] = a.matrix[i] * b.matrix[i];
		//	}
		//	return result;
		//}


		//static double[] operator*(Block block, double[] vec)  // Block-vector multiplication
		//{
		//	if (vec.Length != block.Dimension)
		//	{
		//		throw new ArgumentException("_3DiagMatricesCS.Block.operator*: Dimensions do not match.");
		//	}

		//	double[] result = new double[block.Dimension];
		//	for (int i = 0; i < block.Dimension; ++i)
		//	{
		//		result[i] = block.matrix[i] * vec[i];
		//	}
		//	return result;
		//}

		////--------
		//// Methods
		////--------
		//double[] MultiplyWithVec(double[] vec, int start = 0,
		//	double[] result = null, int resultStart = 0)  // Block-vector multiplication (version with array of bigger size)
		//{
		//	if (start + Dimension > vec.Length)  // Check if start and len are specified correctly (vector can be longer than block)
		//	{
		//		throw new ArgumentException("_3DiagMatricesCS.Block.MultiplyWithVec: Vector doesn't have enough values in it (check vec and start parameters).");
		//	}

		//	if (result == null)  // Create new array if storage for the result was not provided
		//	{
		//		result = new double[Dimension];
		//	}

		//	if (resultStart + Dimension > result.Length)  // Check if there is enough space to store the result
		//	{
		//		throw new ArgumentException("_3DiagMatricesCS.Block.MultiplyWithVec: Not enough space to store the result (check result and resultStart parameters).");
		//	}

		//	for (int i = 0; i < Dimension; ++i)
		//	{
		//		result[resultStart + i] = matrix[i] * vec[start + i];
		//	}
		//	return result;  // Note: return value can be an existing array passed to the function as a parameter.
		//					// Does this have any effect on GC? Should we consider returning null instead?
		//}


		//bool IsSingular()  // Needed in Block.Inverse() method
		//{
		//	for (int i = 0; i < dimension; ++i)
		//	{
		//		if (Math.Abs(matrix[i]) < EPSILON)
		//		{
		//			return true;
		//		}
		//	}
		//	return false;
		//}

		////string RowToString(int row)
		////{
		////	if (row < 0 || row >= Dimension)
		////	{
		////		throw new IndexOutOfRangeException("_3DiagMatricesCS.Block.RowToString: Matrix has no row with that index.");
		////	}

		////	StringBuilder result = new StringBuilder();

		////	result.Append(String.Concat(Enumerable.Repeat("0 ", row)));
		////	result.Append($"{matrix[row]} ");
		////	result.Append(String.Concat(Enumerable.Repeat("0 ", Dimension - 1 - row)));

		////	return result.ToString().TrimEnd();
		////}

		////string ToString()
		////{
		////	StringBuilder str = new StringBuilder();

		////	for (int i = 0; i < Dimension; ++i)
		////	{
		////		str.Append(RowToString(i));
		////		str.AppendLine();
		////	}

		////	return str.ToString().TrimEnd();
		////}

		////---------------
		//// Static methods
		////---------------

		//static Block Inverse(const Block &block)  // Calculate inverse matrix
		//{
		//	if (block.IsSingular())  // Check if inverse matrix exists
		//	{
		//		throw new ArithmeticException("_3DiagMatricesCS.Block.inverse: Can't compute inverse of a singular matrix.");
		//	}

		//	Block result = new Block(block.Dimension);
		//	for (int i = 0; i < block.Dimension; ++i)  // Luckily, our matrix is diagonal, so 
		//	{
		//		result.matrix[i] = 1 / block.matrix[i];
		//	}
		//	return result;
		//}


	};
}
