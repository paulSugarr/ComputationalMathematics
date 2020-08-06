using System;
using System.Collections.Generic;
using System.Text;

namespace ComputationalMaths
{
    static class LinearSystems
    {
        public static Matrix Gauss(Matrix A, Matrix b, int n, double eps)
        {
            if (b.Width > 1)
            {
                throw new ArgumentException();
            }
            double[] y = new double[b.Height];
            for (int i = 0; i < b.Height; i++)
            {
                y[i] = b.Values[i, 0];
            }

            double[,] a = new double[A.Height, A.Width];
            for (int i = 0; i < A.Height; i++)
            {
                for (int j = 0; j < A.Width; j++)
                {
                    a[i, j] = A.Values[i, j];
                }
            }
            double[] x = new double[n];
            double max;
            int k = 0;
            int index;
            while (k < n)
            {
                // Поиск строки с максимальным a[i,k]
                max = Math.Abs(a[k, k]);
                index = k;
                for (int i = k + 1; i < n; i++)
                {
                    if (Math.Abs(a[i, k]) > max)
                    {
                        max = Math.Abs(a[i, k]);
                        index = i;
                    }
                }
                // Перестановка строк
                for (int j = 0; j < n; j++)
                {
                    double temp = a[k, j];
                    a[k, j] = a[index, j];
                    a[index, j] = temp;
                }
                double buffer = y[k];
                y[k] = y[index];
                y[index] = buffer;
                // Нормализация уравнений
                for (int i = k; i < n; i++)
                {
                    double temp = a[i, k];
                    if (Math.Abs(temp) < eps) { continue; } // для нулевого коэффициента пропустить
                    for (int j = 0; j < n; j++)
                    {
                        a[i, j] = a[i, j] / temp;
                    }
                    y[i] = y[i] / temp;
                    if (i == k) { continue; } // уравнение не вычитать само из себя
                    for (int j = 0; j < n; j++)
                    {
                        a[i, j] = a[i, j] - a[k, j];
                    }
                    y[i] = y[i] - y[k];
                }
                k++;
            }
            // обратная подстановка
            for (k = n - 1; k >= 0; k--)
            {
                x[k] = y[k];
                for (int i = 0; i < k; i++)
                {
                    y[i] = y[i] - a[i, k] * x[k];
                }
            }

            double[,] valuesX = new double[x.GetLength(0), 1];
            for (int i = 0; i < x.GetLength(0); i++)
            {
                valuesX[i, 0] = y[i];
            }
            Matrix result = new Matrix(valuesX);
            return result;
        }

        public static Matrix Zaidel(Matrix A, Matrix B, int n, double eps)
        {

            double[] prevX;  // неизвестная на предыдущем шаге
            double[] curX;   // неизвестные на этом шаге
            double error; // наша ошибка на текущей итерации

            prevX = new double[n];
            curX = new double[n];

            for (int i = 0; i < n; i++)
            {
                prevX[i] = 0;
            }

            do
            {
                for (int i = 0; i < n; i++)
                {
                    curX[i] = B.Values[i, 0];
                    for (int j = 0; j < n; j++)
                    {
                        // При j < i можем использовать уже посчитанные
                        // на этой итерации значения неизвестных
                        // При j > i используем значения с прошлой итерации
                        if (j < i)
                        {
                            curX[i] -= A.Values[i, j] * curX[j];
                        }
                        if (j > i)
                        {
                            curX[i] -= A.Values[i, j] * prevX[j];
                        }
                    }
                    curX[i] = curX[i] / A.Values[i, i];
                }
                error = 0.0;
                for (int i = 0; i < n; i++)
                {
                    error += Math.Abs(curX[i] - prevX[i]);
                }

                for (int i = 0; i < n; i++)
                {
                    prevX[i] = curX[i];
                }
                //prevX = curX;

                // Посчитаем текущую погрешность относительно предыдущей итерации
                

            } while (error > eps);
            double[,] resultValues = new double[n, 1];
            for (int i = 0; i < n; i++)
            {
                resultValues[i, 0] = curX[i];
            }
            return new Matrix(resultValues);
        }
    }
}
