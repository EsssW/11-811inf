using System;

namespace Polinom3
{
    class Program
    {
        static void Main(string[] args)
        {
            //-----Создание списков из файла-----
            var a = new Polinom3("In1.txt");
            Console.WriteLine(a);
            Console.WriteLine();
            var b = new Polinom3("In2.txt");
            Console.WriteLine(b);
            Console.WriteLine();

            //-----Строковое представление-----
            Console.WriteLine(a);

            //-----Вставка эл-та с коэфицентом coef и степенями deg1, deg2, deg3-----
            a.Insert(5, 6, 7, 8);
            Console.WriteLine(a);
            Console.WriteLine();

            //-----Удаление эл-та со степенями deg1, deg2, deg3-----
            a.Delete(6, 7, 8);
            Console.WriteLine(a);
            Console.WriteLine();

            //-----Слияние списков с полиномами-----
            a.Add(b);
            Console.WriteLine(a);
            Console.WriteLine();

            //-----Нахождение производной по i-ой переменной-----
            a.Derivate(1);
            Console.WriteLine();

            //-----Значения полиномов в точке x, y, z-----
            var value = b.Value(1, 2, 3);
            Console.WriteLine(value);
            Console.WriteLine();

            //-----Показатели степени с минимальным коэф-ом-----
            var result = a.MinCoef();
            Console.WriteLine(string.Join(" " , result));
            Console.WriteLine();

            Console.ReadKey();
        }
    }
}
