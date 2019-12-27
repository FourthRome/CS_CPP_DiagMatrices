#pragma once
#include <math.h>


namespace _3DiagMatricesCPP
{
	class Block
	{
	public:
		const double EPSILON = 1e-10;
		int dimension;
		double* matrix;
		
	
		Block(int _dimension, double* values=nullptr, int start = 0);
		Block(const Block& other);
		Block(Block&& other) noexcept;
		~Block();


		////----------
		//// Operators
		////----------
		Block& operator=(const Block& other); // copy assignment
		Block& operator=(Block&& other) noexcept; // move assignment

		friend Block& operator+(const Block &a, const Block &b);
		friend Block& operator-(const Block& a, const Block& b);
		friend Block& operator*(const Block& a, const Block& b);
		friend Block& operator-(const Block& a);

		friend double* operator*(const Block& block, const double *vec);
	
		////--------
		//// Methods
		////--------
		double* MultiplyWithVec(double *vec, int start = 0,
			double *result = null, int resultStart = 0)  // Block-vector multiplication (version with array of bigger size)
		{
			if (start + dimension > vec.Length)  // Check if start and len are specified correctly (vector can be longer than block)
			{
				throw new ArgumentException("_3DiagMatricesCS.Block.MultiplyWithVec: Vector doesn't have enough values in it (check vec and start parameters).");
			}

			if (result == null)  // Create new array if storage for the result was not provided
			{
				result = new double[Dimension];
			}

			if (resultStart + Dimension > result.Length)  // Check if there is enough space to store the result
			{
				throw new ArgumentException("_3DiagMatricesCS.Block.MultiplyWithVec: Not enough space to store the result (check result and resultStart parameters).");
			}

			for (int i = 0; i < Dimension; ++i)
			{
				result[resultStart + i] = matrix[i] * vec[start + i];
			}
			return result;  // Note: return value can be an existing array passed to the function as a parameter.
							// Does this have any effect on GC? Should we consider returning null instead?
		}


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
};