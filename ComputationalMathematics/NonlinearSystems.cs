using System;
using System.Collections.Generic;
using System.Text;

namespace ComputationalMaths
{
    class NonlinearSystems
    {
        public delegate Matrix operation(Matrix matrix);

        
        public static Matrix Newton(operation F, operation J, int size, Matrix start, double eps)
        {

            double[,] values = new double[size, 1];
            for (int i = 0; i < size; i++)
            {
                values[i, 0] = 1;
            }
            Matrix previous;
            Matrix solution = Matrix.Copy(start);
            double error = 1;
            if (!J(solution).IsSquare() | F(solution).Height != J(solution).Height)
            {
                throw new ArgumentException();
            }

            while (error > eps)
            {

                previous = Matrix.Copy(solution);
                Matrix inversedJ = Matrix.Inversion(J(solution));
                solution -= inversedJ * F(solution);
                error = (solution - previous).MaxMagnitude();
            }

            return solution;
        }
        /*      
               Более быстрый вариант, когда уже посчитана обратная матрица Якоби
               (предыдущий алгоритм считает её на каждом шаге)
         */
        public static Matrix Newton(operation F, operation J, int size, Matrix start, double eps, bool isJInversed)
        {
            if (!isJInversed) { return Newton(F, J, size, start, eps); }
            double[,] values = new double[size, 1];
            for (int i = 0; i < size; i++)
            {
                values[i, 0] = 1;
            }
            Matrix previous;
            Matrix solution = Matrix.Copy(start);
            double error = 1;
            if (!J(solution).IsSquare() | F(solution).Height != J(solution).Height)
            {
                throw new ArgumentException();
            }

            while (error > eps)
            {

                previous = Matrix.Copy(solution);
                solution -= J(solution) * F(solution);
                error = (solution - previous).MaxMagnitude();
            }

            return solution;
        }
    }
}
