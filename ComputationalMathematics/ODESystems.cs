using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ComputationalMaths
{
    static class ODESystems
    {
        public static void ImplicitEuler(double start, double end, double h, double error, NonlinearSystems.operation System, ref Matrix RKIteration, NonlinearSystems.operation Jacoby, bool isJacobyInversed)
        {
            string format = "0.000000 ";
            List<string> points = new List<string>();
            for (double i = start; end >= i; i += h)
            {
                RKIteration = NonlinearSystems.Newton(System, Jacoby, RKIteration.Height, RKIteration, error, isJacobyInversed);
                points.Add(i.ToString(format) + RKIteration.Values[0, 0].ToString(format) + RKIteration.Values[1, 0].ToString(format));
                Console.WriteLine("{0,12} {1,12} {2,12}", RKIteration.Values[0, 0].ToString(format), RKIteration.Values[1, 0].ToString(format), i.ToString(format));
            }
            File.WriteAllLines("T.txt", points);
            Console.WriteLine("Result:");
            RKIteration.Print();
        }


        public static void Rosenbrock(double start, double end, double h, NonlinearSystems.operation J, NonlinearSystems.operation f, Matrix input)
        {
            string format = "0.000000 ";

            for (double i = start; end >= i; i += h)
            {
                var D = Matrix.IdentityMatrix(2) - 0.5 * h * J(input);
                var F = f(input);

                var k1 = F.Values[0, 0] / (D.Values[0, 0] + D.Values[1, 0]);
                var k2 = F.Values[1, 0] / (D.Values[0, 1] + D.Values[1, 1]);
                Console.WriteLine("{0,12} {1,12} {2,12}", input.Values[0, 0].ToString(format), input.Values[1, 0].ToString(format), i.ToString(format));
                input.Values[0, 0] += k1 * h;
                input.Values[1, 0] += k2 * h;

            }
        }

        public static void Rosenbrock(double start, double end, double h, NonlinearSystems.operation J, NonlinearSystems.operation f, Matrix input, out Matrix predLastVector)
        {
            string format = "0.000000 ";
            predLastVector = new Matrix(input.Values);
            for (double i = start; end >= i; i += h)
            {
                var D = Matrix.IdentityMatrix(2) - 0.5 * h * J(input);
                var F = f(input);

                var k1 = F.Values[0, 0] / (D.Values[0, 0] + D.Values[1, 0]);
                var k2 = F.Values[1, 0] / (D.Values[0, 1] + D.Values[1, 1]);
                if (end - h == i)
                {
                    predLastVector.Values[0, 0] = input.Values[0, 0];
                    predLastVector.Values[1, 0] = input.Values[1, 0];
                }
                input.Values[0, 0] += k1 * h;
                input.Values[1, 0] += k2 * h;

            }
        }
    }
}
