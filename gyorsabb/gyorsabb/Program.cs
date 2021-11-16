using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace gyorsabb
{
    class Program
    {
        public static Stopwatch timer = new Stopwatch();
        public static int jatekhossz = 5;

        public class Pair<U, T>
        {
            public Pair()
            {

            }
            public Pair(Pair<U, T> item)
            {
                this.First = item.First;
                this.Second = item.Second;
            }
            public Pair(U first, T second)
            {
                this.First = first;
                this.Second = second;
            }
            public U First { get; set; }
            public T Second { get; set; }
        }

        public class Tripple<U, T, K>
        {
            public Tripple()
            {

            }
            public Tripple(Tripple<U, T, K> item)
            {
                this.First = item.First;
                this.Second = item.Second;
                this.Third = item.Third;
            }
            public Tripple(U first, T second, K third)
            {
                this.First = first;
                this.Second = second;
                this.Third = third;
            }
            public U First { get; set; }
            public T Second { get; set; }
            public K Third { get; set; }
        }

        public static List<Tripple<int, int, int>> kordinatak = new List<Tripple<int, int, int>>();
        public static List<Tripple<int, int, int>> megoldaslista = new List<Tripple<int, int, int>>();
        static void Main(string[] args)
        {
            timer.Start();
            using (System.IO.StreamReader f = new System.IO.StreamReader("kordinatak.txt"))
            {
                while (!f.EndOfStream)
                {
                    string[] s = f.ReadLine().Split(' ');
                    Tripple<int, int, int> temp = new Tripple<int, int, int>(int.Parse(s[0]), int.Parse(s[1]), int.Parse(s[2])); //X, Y , XvagyO (0 VAGY 1)
                    kordinatak.Add(temp);
                }
            }

            string tovabb = "y";
            using (System.IO.StreamWriter f = new System.IO.StreamWriter("result.txt"))
            {
                foreach (var item in kordinatak)
                {
                    f.WriteLine(item.First + " " + item.Second + " " + item.Third);
                }
                while (tovabb == "y")
                {
                    timer.Restart();
                    foreach (var item in validsteppek())
                    {
                        //O val játszik a bot mindig
                        kordinatak.Add(new Tripple<int, int, int>(item.First, item.Second, 1));
                        rekurzio(3, item);
                        kordinatak.RemoveAt(kordinatak.Count - 1);
                    }
                    Console.WriteLine(timer.Elapsed + " másodpercbe telt.");
                    timer.Stop();
                    //megoldaslistában amxot keresünk
                    Tripple<int, int, int> max = megoldaslista[0]; // azt semmi nem fogja elérni
                    foreach (var item in megoldaslista)
                    {
                        if (item.Third > max.Third)
                            max = item;
                    }
                    Console.WriteLine(max.First + "\t" + max.Second + "\t" + max.Third);
                    kordinatak.Add(new Tripple<int, int, int>(max.First, max.Second, 1));//mivel kört rakott le
                    f.WriteLine(max.First + " " + max.Second + " 1");
                    Console.WriteLine("Do you want to continue ? ('y' or 'n')");
                    tovabb = Console.ReadLine();
                    if (tovabb == "y")
                    {
                        Console.WriteLine("Where do you want to place your X? (example.: '1 1')");
                        string[] s = Console.ReadLine().Split(' ');
                        kordinatak.Add(new Tripple<int, int, int>(int.Parse(s[0]), int.Parse(s[1]), 0));
                        f.WriteLine(s[0] + " " + s[1] + " 0");
                    }
                    megoldaslista.Clear();
                }
            }
        }

        public static void rekurzio(int milyenmélyre, Pair<int, int> eredetiKordinatak)
        {
            if (milyenmélyre == 0)
            {
                megoldaslista.Add(new Tripple<int, int, int>(eredetiKordinatak.First, eredetiKordinatak.Second, ValueBoard()));
            }
            else
            {
                if (milyenmélyre % 2 == 0)
                    foreach (var item in validsteppek())
                    {
                        kordinatak.Add(new Tripple<int, int, int>(item.First, item.Second, 1));
                        rekurzio(milyenmélyre - 1, eredetiKordinatak);
                        kordinatak.RemoveAt(kordinatak.Count - 1);
                    }
                else
                    foreach (var item in validsteppek())
                    {
                        kordinatak.Add(new Tripple<int, int, int>(item.First, item.Second, 0));
                        rekurzio(milyenmélyre - 1, eredetiKordinatak);
                        kordinatak.RemoveAt(kordinatak.Count - 1);
                    }
            }
        }

        public static List<Pair<int, int>> validsteppek()
        {
            List<Pair<int, int>> solution = new List<Pair<int, int>>();
            foreach (var item in kordinatak)
            {
                if (!ContainsValidsteppekhez(solution, item.First + 1, item.Second)) //jobbra
                    solution.Add(new Pair<int, int>(item.First + 1, item.Second));
                if (!ContainsValidsteppekhez(solution, item.First - 1, item.Second)) //balra
                    solution.Add(new Pair<int, int>(item.First - 1, item.Second));
                if (!ContainsValidsteppekhez(solution, item.First, item.Second + 1)) //fel
                    solution.Add(new Pair<int, int>(item.First, item.Second + 1));
                if (!ContainsValidsteppekhez(solution, item.First, item.Second - 1)) //le
                    solution.Add(new Pair<int, int>(item.First, item.Second - 1));
                if (!ContainsValidsteppekhez(solution, item.First + 1, item.Second + 1)) //jobbrafel
                    solution.Add(new Pair<int, int>(item.First + 1, item.Second + 1));
                if (!ContainsValidsteppekhez(solution, item.First - 1, item.Second + 1)) //balrafel
                    solution.Add(new Pair<int, int>(item.First - 1, item.Second + 1));
                if (!ContainsValidsteppekhez(solution, item.First - 1, item.Second - 1)) //balrale
                    solution.Add(new Pair<int, int>(item.First - 1, item.Second - 1));
                if (!ContainsValidsteppekhez(solution, item.First + 1, item.Second - 1)) //jobbrale
                    solution.Add(new Pair<int, int>(item.First + 1, item.Second - 1));
            }
            return solution;
        }

        //megnézi a solutio és a kordináta listában is
        public static bool ContainsValidsteppekhez(List<Pair<int, int>> lista, int a, int b)
        {
            bool solution = false;
            foreach (var item in kordinatak)
            {
                if (item.First == a && item.Second == b)
                {
                    solution = true;
                    break;
                }
            }
            if (!solution)
            {
                foreach (var item in lista)
                {
                    if (item.First == a && item.Second == b)
                    {
                        solution = true;
                        break;
                    }
                }
            }
            return solution;
        }

        public static bool ContainsValueBoardhoz(int a, int b, int c)
        {
            bool solution = false;
            foreach (var item in kordinatak)
            {
                if (item.First == a && item.Second == b && item.Third == c)
                {
                    solution = true;
                    break;
                }
            }
            return solution;
        }

        public static int ValueBoard()
        {
            int solution = 0;
            foreach (var item in kordinatak)
            {
                #region horizontal
                int szamlalo = 1; //önmaga
                                  //jobbra
                for (int i = 1; i < jatekhossz; i++)
                {
                    if (ContainsValueBoardhoz(item.First + i, item.Second, item.Third))
                        ++szamlalo;
                    else
                        break;
                }
                //balra
                for (int i = 1; i < jatekhossz; i++)
                {
                    if (ContainsValueBoardhoz(item.First - i, item.Second, item.Third))
                        ++szamlalo;
                    else
                        break;
                }
                //ertekfeldolgozas
                if (item.Third == 1) // ha O
                {
                    if (szamlalo >= jatekhossz)
                        solution += 100;
                    else
                        solution += szamlalo;
                }
                else // Ha X
                {
                    if (szamlalo >= jatekhossz)
                        solution -= 100;
                    else
                        solution -= szamlalo;
                }
                #endregion

                #region vertical
                szamlalo = 1; //önmaga
                              //fel
                for (int i = 1; i < jatekhossz; i++)
                {
                    if (ContainsValueBoardhoz(item.First, item.Second + i, item.Third))
                        ++szamlalo;
                    else
                        break;
                }
                //le
                for (int i = 1; i < jatekhossz; i++)
                {
                    if (ContainsValueBoardhoz(item.First, item.Second - i, item.Third))
                        ++szamlalo;
                    else
                        break;
                }
                //ertekfeldolgozas
                if (item.Third == 1) // ha O
                {
                    if (szamlalo >= jatekhossz)
                        solution += 100;
                    else
                        solution += szamlalo;
                }
                else // Ha X
                {
                    if (szamlalo >= jatekhossz)
                        solution -= 100;
                    else
                        solution -= szamlalo;
                }
                #endregion

                #region rightDiagonal
                szamlalo = 1; //önmaga
                              //jobbrafel
                for (int i = 1; i < jatekhossz; i++)
                {
                    if (ContainsValueBoardhoz(item.First + i, item.Second + i, item.Third))
                        ++szamlalo;
                    else
                        break;
                }
                //balrale
                for (int i = 1; i < jatekhossz; i++)
                {
                    if (ContainsValueBoardhoz(item.First - i, item.Second - i, item.Third))
                        ++szamlalo;
                    else
                        break;
                }
                //ertekfeldolgozas
                if (item.Third == 1) // ha O
                {
                    if (szamlalo >= jatekhossz)
                        solution += 100;
                    else
                        solution += szamlalo;
                }
                else // Ha X
                {
                    if (szamlalo >= jatekhossz)
                        solution -= 100;
                    else
                        solution -= szamlalo;
                }
                #endregion

                #region leftDiagonal
                szamlalo = 1; //önmaga
                              //balrafel
                for (int i = 1; i < jatekhossz; i++)
                {
                    if (ContainsValueBoardhoz(item.First - i, item.Second + i, item.Third))
                        ++szamlalo;
                    else
                        break;
                }
                //jobbrale
                for (int i = 1; i < jatekhossz; i++)
                {
                    if (ContainsValueBoardhoz(item.First + i, item.Second - i, item.Third))
                        ++szamlalo;
                    else
                        break;
                }
                //ertekfeldolgozas
                if (item.Third == 1) // ha O
                {
                    if (szamlalo >= jatekhossz)
                        solution += 100;
                    else
                        solution += szamlalo;
                }
                else // Ha X
                {
                    if (szamlalo >= jatekhossz)
                        solution -= 100;
                    else
                        solution -= szamlalo;
                }
                #endregion
            }
            return solution;
        }
    }
}
