using System;

namespace Polinom3
{
    class Program
    {
        static void Main(string[] args)
        {
            //-----Создание списков из файла-----
            var a = new Polinom3("In1.txt");
            var b = new Polinom3("In2.txt");

            //-----Строковое представление-----
            Console.WriteLine(a);

            //-----Вставка эл-та с коэфицентом coef и степенями deg1, deg2, deg3-----
            a.Insert(5, 6, 7, 8);

            //-----Удаление эл-та со степенями deg1, deg2, deg3-----
            a.Delete(6, 7, 8);

            //-----Слияние списков с полиномами-----
            a.Add(b);

            //-----Не готовая производная-----

            //a.Derivate();

            //-----Значения полиномов в точке x, y, z-----
            var value = a.Value(1, 2, 3);

            //-----Показатели степени с минимальным коэф-ом-----
            var result = a.MinCoef();

            Console.ReadKey();
        }
    }
}
