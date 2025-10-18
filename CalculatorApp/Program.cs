using System;

namespace CalculatorApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Simple .NET Calculator ===");
            Console.Write("Enter first number: ");
            double num1 = Convert.ToDouble(Console.ReadLine());

            Console.Write("Enter operator (+, -, *, /): ");
            char op = Console.ReadLine()[0];

            Console.Write("Enter second number: ");
            double num2 = Convert.ToDouble(Console.ReadLine());

            double result = op switch
            {
                '+' => num1 + num2,
                '-' => num1 - num2,
                '*' => num1 * num2,
                '/' => num2 != 0 ? num1 / num2 : double.NaN,
                _ => double.NaN
            };

            Console.WriteLine($"Result: {result}");
        }
    }
}
