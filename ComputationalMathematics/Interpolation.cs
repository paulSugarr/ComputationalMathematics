using System;
using System.Collections.Generic;
using System.Text;

namespace ComputationalMaths
{
    class Interpolation
    {
        public static double QuadInterpolation(double [,] tableFunction, double x)
        {
            if (tableFunction.GetLength(1) != 2)
            {
                throw new ArgumentException();
            }

            double nearest = tableFunction[0, 0];
            double temp = Math.Abs(x - nearest);
            int index = 0;

            int size = tableFunction.GetLength(0);
            for (int i = 0; i < size; i++)
            {
                if (Math.Abs(x - tableFunction[i, 0]) < temp)
                {
                    temp = Math.Abs(x - tableFunction[i, 0]);
                    nearest = tableFunction[i, 0];
                    index = i;
                }
            }
            if (index - 1 >= 0 & index + 1 < size)
            {
                double[,] values =
                {
                    {tableFunction[index - 1, 0] *  tableFunction[index - 1, 0], tableFunction[index - 1, 0], 1 },
                    {tableFunction[index, 0] *  tableFunction[index, 0], tableFunction[index, 0], 1 },
                    {tableFunction[index + 1, 0] *  tableFunction[index + 1, 0], tableFunction[index + 1, 0], 1 },
                };
                Matrix system = new Matrix(values);

                double[,] vectorValues =
                {
                    { tableFunction[index - 1, 1]},
                    { tableFunction[index, 1]},
                    { tableFunction[index + 1, 1]}
                };
                Matrix vector = new Matrix(vectorValues);

                Matrix coefficients = LinearSystems.Gauss(system, vector, 3, 0.000001);
                double result = coefficients.Values[0, 0] * x * x + coefficients.Values[1, 0] * x + coefficients.Values[2, 0];
                return result;
            }
            else
            {
                throw new ArgumentException();
            }
        }
    }
}
