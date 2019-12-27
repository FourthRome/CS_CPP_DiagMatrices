#include "pch.h"
#include <iostream>

extern "C" _declspec(dllexport)
void SolveEquationCpp(int matDim, int bloDim, double *matrix, double *rightSide, double *result, double &time)
{


	for (int i = 0; i < matDim * bloDim; ++i) {
		result[i] = 2.0;
	}

	time = 3.0;
}

int Main()
{
	std::cout << "Hello!" << std::endl;
	return 0;
}
