using System;
using System.Collections.Generic;
using System.IO;

namespace Polinom3
{
    class Element
    {
        public int Coef { get; set; }
        public int Deg1 { get; set; }
        public int Deg2 { get; set; }
        public int Deg3 { get; set; }
        public Element Next { get; set; }

        public override string ToString()
        {
            return $"{Coef} * x^{Deg1} * y^{Deg2} * z^{Deg3}";
        }

        public int Compare(Element el) //Приводим полиномы к виду Deg1Deg2Deg3 и сравниваем лексикографически
        {
            var elStr = "" + el.Deg1 + el.Deg2 + el.Deg3;
            var thisStr = "" + Deg1 + Deg2 + Deg3;
            return thisStr.CompareTo(elStr);
        }
    }

    class Polinom3
    {
        public Element Head { get; set; }
        public Element Tail { get; set; }

        public Polinom3() { }
        public Polinom3(string filename)
        {
            var a = File.ReadAllLines(filename);
            foreach (var e in a)
            {
                if (Head == null)
                    Head = Tail = StrToElem(e);
                else
                {
                    Tail.Next = StrToElem(e);
                    Tail = Tail.Next;
                }
            }
        }

        public Element StrToElem(string str) //Преобразуем строку вида "1 2 3 4" в полином
        {
            var temp = str.Split(' ');
            return new Element
            {
                Coef = int.Parse(temp[0]),
                Deg1 = int.Parse(temp[1]),
                Deg2 = int.Parse(temp[2]),
                Deg3 = int.Parse(temp[3])
            };
        }

        public void Insert(int coef, int deg1, int deg2, int deg3)
        {
            var elem = new Element { Coef = coef, Deg1 = deg1, Deg2 = deg2, Deg3 = deg3, Next = null };
            var temp = Head;
            if (elem.Compare(temp) < 0) //Проверяем не является ли он самым младшим лексикографически
            {
                elem.Next = temp;
                Head = elem;
                return;
            }
            while (temp != null)
            {
                if (elem.Compare(temp) == 0 && temp.Coef == coef)//Проверяем, не является ли текущий элемент копией нашего
                {
                    temp = elem;
                    return;
                }
                if (temp.Next == null)//Проверяем, не дошли ли мы до конца
                {
                    temp.Next = elem;
                    Tail = temp.Next;
                    return;
                }
                if (elem.Compare(temp) >= 0 && elem.Compare(temp.Next) < 0)
                {
                    elem.Next = temp.Next;
                    temp.Next = elem;
                    return;
                }
                temp = temp.Next;
            }
        }

        public void Delete(int deg1, int deg2, int deg3)
        {
            var temp = Head;
            if (temp.Deg1 == deg1 && temp.Deg2 == deg2 && temp.Deg3 == deg3)//Проверяем, не является ли первый эл-т нужным
            {
                Head = Head.Next;
                return;
            }
            while (temp != null)
            {
                if (temp.Next.Deg1 == deg1 && temp.Next.Deg2 == deg2 && temp.Next.Deg3 == deg3)
                {
                    temp.Next = temp.Next.Next;
                    return;
                }
                temp = temp.Next;
            }
            throw new Exception("The list does not contain that element");
        }

        public void Add(Polinom3 p)//Обычное слияние списков, проще было бы просто вставить всё из второго списка в первый при помощи Insert
        {
            var result = new Polinom3();
            var tempP = p.Head;
            var temp = Head;
            if (temp.Compare(tempP) < 0)
            {
                result.Head = result.Tail = new Element { Coef = temp.Coef, Deg1 = temp.Deg1, Deg2 = temp.Deg2, Deg3 = temp.Deg3 };
                temp = temp.Next;
            }
            else
            {
                result.Head = result.Tail = new Element { Coef = tempP.Coef, Deg1 = tempP.Deg1, Deg2 = tempP.Deg2, Deg3 = tempP.Deg3 };
                tempP = tempP.Next;
            }
            while (temp != null || tempP != null)
            {
                if (tempP == null || temp != null && temp.Compare(tempP) < 0)
                {
                    result.Tail.Next = new Element { Coef = temp.Coef, Deg1 = temp.Deg1, Deg2 = temp.Deg2, Deg3 = temp.Deg3 };
                    result.Tail = result.Tail.Next;
                    temp = temp.Next;
                }
                else
                {
                    result.Tail.Next = new Element { Coef = tempP.Coef, Deg1 = tempP.Deg1, Deg2 = tempP.Deg2, Deg3 = tempP.Deg3 }; ;
                    result.Tail = result.Tail.Next;
                    tempP = tempP.Next;
                }
            }
            Head = result.Head;
            Tail = result.Tail;
        }

        public void Derivate(int i)
        {
            var temp = Head;
            while (temp != null)
            {
                switch (i)
                {
                    case 1:
                        if (temp.Deg1 == 0)
                        {
                            Delete(temp.Deg1, temp.Deg2, temp.Deg3);
                            continue;
                        }
                        temp.Coef *= temp.Deg1;
                        temp.Deg1--;
                        break;
                    case 2:
                        if (temp.Deg2 == 0)
                        {
                            Delete(temp.Deg1, temp.Deg2, temp.Deg3);
                            continue;
                        }
                        temp.Coef *= temp.Deg2;
                        temp.Deg2--;
                        break;
                    case 3:
                        if (temp.Deg3 == 0)
                        {
                            Delete(temp.Deg1, temp.Deg2, temp.Deg3);
                            continue;
                        }
                        temp.Coef *= temp.Deg3;
                        temp.Deg3--;
                        break;
                    default:
                        throw new ArgumentException();
                }
                temp = temp.Next;
            }
        }

        public int Value(int x, int y, int z)
        {
            var temp = Head;
            var result = 0;
            while (temp != null)
            {
                result += temp.Coef * (int)Math.Pow(x, temp.Deg1) * (int)Math.Pow(y, temp.Deg2) * (int)Math.Pow(z, temp.Deg3);
                temp = temp.Next;
            }
            return result;
        }

        public int[] MinCoef()
        {
            int min = int.MaxValue;
            var temp = Head;
            var result = new int[3];
            while (temp != null)
            {
                if (temp.Coef < min)
                {
                    min = temp.Coef;
                    result = new int[] { temp.Deg1, temp.Deg2, temp.Deg3 };
                }
                temp = temp.Next;
            }
            return result;
        }

        public int Count()
        {
            var temp = Head;
            int result = 0;
            while (temp != null)
            {
                result++;
                temp = temp.Next;
            }
            return result;
        }

        public override string ToString()
        {
            var el = Head;
            var list = new List<string>();
            while (el != null)
            {
                list.Add(el.ToString());
                el = el.Next;
            }
            return String.Join(" + ", list);
        }
    }
}
