using System;
using System.Collections.Generic;
using System.Text;

namespace ComputationalMaths
{
    class Matrix
    {
        public double [,] Values { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public static Matrix IdentityMatrix (int size)
        {
            double[,] values = new double[size, size];
            for (int i = 0; i < values.GetLength(0); i++)
            {
                for (int j = 0; j < values.GetLength(1); j++)
                {
                    if (i == j) { values[i, j] = 1; }
                    else { values[i, j] = 0; }
                }
            }
            return new Matrix(values);
        }

        public Matrix(double [,] values)
        {
            Values = values;
            Height = values.GetLength(0);
            Width = values.GetLength(1);
        }
        public Matrix(int length, int width, double value)
        {
            Values = new double[length, width];
            for (int i = 0; i < Values.GetLength(0); i++)
            {
                for (int j = 0; j < Values.GetLength(1); j++)
                {
                    Values[i, j] = value;
                }
            }
            Height = Values.GetLength(0);
            Width = Values.GetLength(1);
        }
        public void Print()
        {
            for (int i = 0; i < Values.GetLength(0); i++)
            {
                for (int j = 0; j < Values.GetLength(1); j++)
                {
                    Console.Write("{0} ", Values[i, j]);
                }
                Console.WriteLine();
            }
        }
        public bool IsSquare()
        {
            return Height == Width;
        }
        public static Matrix Copy(Matrix matrix)
        {
            double[,] values = new double[matrix.Height, matrix.Width];
            for (int i = 0; i < matrix.Height; i++)
            {
                for (int j = 0; j < matrix.Width; j++)
                {
                    values[i, j] = matrix.Values[i, j];
                }
            }
            return new Matrix(values);
        }
        public double MaxMagnitude()
        {
            double max = 0;
            foreach (var value in Values)
            {
                if (Math.Abs(value) > max)
                {
                    max = Math.Abs(value);
                }
            }
            return max;
        }
        public double SumMagnitude()
        {
            double result = 0;
            foreach (var value in Values)
            {
                result += Math.Abs(value);
            }
            return result;
        }
        public static Matrix Inversion(Matrix A)
        {
            if (A.Height != A.Width)
            {
                throw new ArgumentNullException();
            }
            int size = A.Width;
	        double temp;
            Matrix E = IdentityMatrix(size);
            double [,] values = new double[A.Height, A.Width];
            for (int i = 0; i < A.Height; i++)
            {
                for (int j = 0; j < A.Width; j++)
                {
                    values[i, j] = A.Values[i, j];
                }
            }
            Matrix tempMatr = new Matrix (values);

	        for (int k = 0; k < size; k++)
            {
		        temp = tempMatr.Values[k,k];
		        for (int j = 0; j < size; j++)
                {
			        tempMatr.Values[k,j] /= temp;
			        E.Values[k,j] /= temp;
		        }

		        for (int i = k + 1; i < size; i++)
                {
			        temp = tempMatr.Values[i,k];
			        for (int j = 0; j < size; j++)
                    {
				        tempMatr.Values[i,j] -= tempMatr.Values[k,j] * temp;
                        E.Values[i,j] -= E.Values[k,j] * temp;
			        }
		        }
	        }

	        for (int k = size - 1; k > 0; k--)
            {
		        for (int i = k - 1; i >= 0; i--)
                {
			        temp = tempMatr.Values[i,k];
			        for (int j = 0; j < size; j++)
                    {
				        tempMatr.Values[i,j] -= tempMatr.Values[k,j] * temp;
                        E.Values[i,j] -= E.Values[k,j] * temp;
			        }
		        }
	        }

	        for (int i = 0; i < size; i++) 
            {
		        for (int j = 0; j < size; j++) 
                {
			        tempMatr.Values[i,j] = E.Values[i,j];
		        }
	        }

	        return tempMatr;
        }
        public static double Norm(Matrix a, int n)
        {
            double sum = 0;
            double maxSum = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    sum += a.Values[i, j];
                }
                if (sum > maxSum)
                {
                    maxSum = sum;
                }
            }
            return maxSum;
        }

        public static Matrix operator+ (Matrix A, Matrix B)
        {
            if ((A.Width != B.Width) | (A.Height != B.Height))
            {
                throw new ArgumentException();
            }

            double[,] array = new double[A.Height, A.Width];
            for (int i = 0; i < A.Height; i++)
            {
                for (int j = 0; j < A.Width; j++)
                {
                    array[i, j] = A.Values[i, j] + B.Values[i, j];
                }
            }
            return new Matrix(array);
        }
        public static Matrix operator- (Matrix A, Matrix B)
        {
            return A + (-1) * B;
        }
        public static Matrix operator* (Matrix A, double a)
        {
            double[,] array = new double[A.Height, A.Width];
            for (int i = 0; i < A.Height; i++)
            {
                for (int j = 0; j < A.Width; j++)
                {
                    array[i, j] = A.Values[i, j] * a;
                }
            }
            return new Matrix(array);
        }
        public static Matrix operator* (double a, Matrix A)
        {
            return A * a;
        }
        public static Matrix operator* (Matrix A, Matrix B)
        {
            if (A.Width != B.Height)
            {
                throw new ArgumentException();
            }
            double[,] array = new double[A.Height, B.Width];
            for (int i = 0; i < A.Height; i++)
            {
                for (int j = 0; j < B.Width; j++)
                {
                    array[i, j] = 0;
                    for (int k = 0; k < B.Height; k++)
                    {
                        array[i, j] += A.Values[i, k] * B.Values[k, j];
                    }
                }
            }

            return new Matrix(array);
        }

    }
}
