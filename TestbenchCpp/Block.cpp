#include <iostream>
#include "Block.h"

namespace _3DiagMatricesCPP
{
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

		Block::Block(int _dimension, double* values, int start)  // Construct Block from double[]
		{
			dimension = _dimension;
			matrix = new double[dimension];  // Arguments are acceptable, so we can allocate memory

			if (values) {
				for (int i = 0; i < dimension; ++i)  // Initialize matrix with values from double[], starting from the "start" index
				{
					matrix[i] = values[start + i];
				}
			}
			else
			{
				for (int i = 0; i < dimension; ++i)
				{
					matrix[i] = 0;
				}
			}
		}

		Block::Block(const Block &other)  // Construct Block from double[]
		{
			dimension = other.dimension;
			matrix = new double[dimension];  // Arguments are acceptable, so we can allocate memory

			for (int i = 0; i < dimension; ++i) {
				matrix[i] = other.matrix[i];
			}
		}

		Block::Block(Block&& other) noexcept :
			matrix(std::move(other.matrix)),       // explicit move of a member of class type
			dimension(std::exchange(other.dimension, 0)) // explicit move of a member of non-class type
		{ }

		//-----------
		// Destructor
		//-----------
		Block::~Block()
		{
			delete[] matrix;
		}


		////----------
		//// Operators
		////----------
		Block& Block::operator=(const Block& other) // copy assignment
		{
			if (this != &other) { // self-assignment check expected
				if (other.dimension != dimension) {         // storage cannot be reused
					delete[] matrix;              // destroy storage in this
					matrix = nullptr;             // preserve invariants in case next line throws
					matrix = new double[other.dimension]; // create storage in this
					dimension = other.dimension;
				}
				std::copy(other.matrix, other.matrix + other.dimension, matrix);
			}
			return *this;
		}

		Block& Block::operator=(Block&& other) noexcept // move assignment
		{
			if (this != &other) { // no-op on self-move-assignment (delete[]/size=0 also ok)
				delete[] matrix;                               // delete this storage
				matrix = std::exchange(other.matrix, nullptr); // leave moved-from in valid state
				dimension = std::exchange(other.dimension, 0);
			}
			return *this;
		}

		Block& operator+(const Block &a, const Block &b)  // Block addition
		{
			Block result = Block(a.dimension);
			for (int i = 0; i < a.dimension; ++i)
			{
				result.matrix[i] = a.matrix[i] + b.matrix[i];
			}
			return result;
		}

		Block& operator-(const Block& a, const Block& b)  // Block subtraction
		{
			Block result = Block(a.dimension);
			for (int i = 0; i < a.dimension; ++i)
			{
				result.matrix[i] = a.matrix[i] - b.matrix[i];
			}
			return result;
		}

		Block& operator*(const Block& a, const Block& b)  // Block multiplication
		{
			Block result = Block(a.dimension);
			for (int i = 0; i < a.dimension; ++i)
			{
				result.matrix[i] = a.matrix[i] * b.matrix[i];
			}
			return result;
		}

		Block& operator-(const Block& a)  // Block negation
		{
			Block result = Block(a.dimension);
			for (int i = 0; i < a.dimension; ++i)
			{
				result.matrix[i] = -a.matrix[i];
			}
			return result;
		}

		double* operator*(const Block &block, double *vec)  // Block-vector multiplication
		{
			double *result = new double[block.dimension];
			for (int i = 0; i < block.dimension; ++i)
			{
				result[i] = block.matrix[i] * vec[i];
			}
			return result;
		}

		////--------
		//// Methods
		////--------
		double[] Block::MultiplyWithVec(double[] vec, int start = 0,
			double[] result = null, int resultStart = 0)  // Block-vector multiplication (version with array of bigger size)
		{
			if (start + Dimension > vec.Length)  // Check if start and len are specified correctly (vector can be longer than block)
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
