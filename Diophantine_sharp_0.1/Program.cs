using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.IO;

namespace Diophantine_sharp
{
    public class Program
    {
        static void write_arr(int[] arr)
        {
            foreach (var a in arr)
            {
                Console.Write(a + ", ");
            }
            Console.WriteLine();
        }
        static T Reduce<T, U>(Func<U, T, T> func, IEnumerable<U> list, T acc)
        {
            foreach (var i in list)
                acc = func(i, acc);

            return acc;
        }

        static IEnumerable<TResult> Map<T, TResult>(Func<T, TResult> func, IEnumerable<T> list)
        {
            foreach (var i in list)
                yield return func(i);
        }

        static int[] plus(int[] arr1, int[] arr2)
        {
            int[] r = new int[arr1.Length];
            for (int i = 0; i < r.Length; i++)
            {
                r[i] = arr1[i] + arr2[i];
            }
            return r;
        }

        static int[] Min_m(List<int[]> arr)
        {
            List<int> m_arr = new List<int>();
            foreach (var vec in arr)
            {
                int k = 0;
                int l = 0;
                int r = 0;
                foreach (var a in vec)
                {
                    if (a > 0)
                        k++;
                    else if (a < 0)
                        l++;
                    else
                        r++;
                }
                m_arr.Add(k * l + r);
            }
            return arr[m_arr.IndexOf(m_arr.Min())];
        }

        static int GCD(int a, int b)
        {
            if (b == 0)
                return a;
            else
                return GCD(b, a % b);
        }

        static List<int[]> redundant(List<int[]> ar)
        {
            List<int[]> ar2 = new List<int[]>();
            UInt64[][] decar = new UInt64[ar.Count][];

            for (int y = 0; y<ar.Count; y++)
            {
                int lenght = Convert.ToInt32(Math.Ceiling(ar[0].Length / 64.0));
                List<string> binstr = new List<string>();
                for (int i=0; i < lenght; i ++)
                {
                    string stringToAdd = "";
                    for (int j=0; j<64; j++)
                    {
                        int index = i * 64 + j;
                        if (ar[y].Length > index &&  ar[y][index] != 0) { stringToAdd += "1"; }
                                            else { stringToAdd += "0"; }
                    }
                    binstr.Add(stringToAdd);
                }

                UInt64[] decarToAdd = new UInt64[lenght];
                for(int i=0; i<lenght; i++)
                {
                    decarToAdd[i] = Convert.ToUInt64(binstr[i], 2);
                }
                decar[y] = decarToAdd;
            }

            for (int j=0; j < ar.Count; j++)
            {
                bool r = true;
                for (int k = 0; k < ar.Count; k++)
                {
                    if (j != k)
                    {
                        for (int n = 0; n<decar[j].Length && r==true; n++)
                        {
                            UInt64 an = decar[j][n] & decar[k][n];
                            if (an == decar[k][n])
                            {
                                r = false;
                                break;
                            }
                        }
                    }
                }
                if (r)
                {
                    ar2.Add(ar[j]);
                }
            }
            return ar2;
        }

        static List<int[]> redundant2(List<int[]> ar, int len_input_arr)
        {
            List<int[]> ar2 = new List<int[]>();
            List<string> strings = new List<string>();
            for (int j = 0; j < ar.Count; j++)
            {
                int zcounter = 0;
                string s = "";
                for (int i = 0; i < ar[j].Length; i++)
                {
                    if (ar[j][i] == 0)
                    {
                        zcounter += 1;
                        s = s + "0";
                    }
                    else
                    {
                        s = s + "1";
                    }
                }
                if (zcounter > len_input_arr)
                {
                    ar2.Add(ar[j]);
                    strings.Add(s);
                }
            }
            List<int[]> ar3 = new List<int[]>();
            List<string> strings2 = new List<string>();
            for (int j = 0; j < ar2.Count; j++)
            {
                if (!strings2.Contains(strings[j]))
                {
                    ar3.Add(ar2[j]);
                    strings2.Add(strings[j]);
                }
            }
            return ar3;
        }

        static List<int[]> siplify(List<int[]> ar)
        {
            List<int[]> ar2 = new List<int[]>() { };
            foreach (int[] y in ar)
            {
                int[] y2 = y;
                int d = Reduce(GCD, y, 0);
                if (d!=0 && d != 1)
                {
                    y2 = Map<int, int>(z => z / d, y).ToArray();
                }
                ar2.Add(y2);
            }
            return ar2;
        }

        static int[][] set_basis(int[] arr)
        {
            int[][] basis = new int[arr.Length][];
            for (int p = 0; p < arr.Length; p++)
            {
                basis[p] = new int[arr.Length];
                for (int o = 0; o < arr.Length; o++)
                {
                    if (p == o)
                        basis[p][o] = 1;
                    else
                        basis[p][o] = 0;
                }
            }
            return basis;
        }

        public static void Diagonalize(List<int[]> ar)
        {
            for (int i = 0; i < ar.Count; i++)
            {
                if (ar[i][i] == 0)
                {
                    int t = 0;
                    while ((i + t + 1 < ar.Count) && (ar[i + t][i] == 0))  // search for line with not a zero in i column
                    {
                        t += 1;
                    }
                    int[] c = ar[i];
                    ar[i] = ar[i + t];
                    ar[i + t] = c;
                    if (ar[i][i] == 0)
                    {
                        break;
                    }
                }
                for (int j = 0; j < i; j++)
                {
                    if (ar[j][i] != 0)
                    {
                        int[] newj = new int[ar[j].Length];
                        for (int k = 0; k < ar[i].Length; k++)//no map this time
                        {
                            newj[k] = checked(ar[j][k] * ar[i][i] - ar[i][k] * ar[j][i]);
                        }
                        ar[j] = newj;
                    }
                }
                for (int j = i + 1; j < ar.Count; j++)
                {
                    if (ar[j][i] != 0)
                    {
                        int[] newj = new int[ar[j].Length];
                        for (int k = 0; k < ar[i].Length; k++)//no map this time
                        {
                            newj[k] = ar[j][k] * ar[i][i] - ar[i][k] * ar[j][i];
                        }
                        ar[j] = newj;
                    }
                }
                ar = siplify(ar);
            }
        }

        public static List<int[]> Read_input(string path)
        {
            string[] str_input = File.ReadAllLines(path);
            List<int[]> retlist = new List<int[]>();
            foreach (string line in str_input)
            {
                string[] arline = line.Split();
                int[] intline = new int[arline.Length];
                for (int i = 0; i < arline.Length; i++)
                {
                    intline[i] = int.Parse(arline[i]);
                }
                retlist.Add(intline);
            }
            return retlist;
        }

        static void WriteToFile(List<int[]> x)
        {
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(@"output.txt"))
            {
                foreach (int[] l in x)
                {
                    foreach (int n in l)
                    {
                        file.Write("{0} ", n);
                    }
                    file.WriteLine();
                }
            }
        }

        public static List<int[]> Work(string path)
        {
            List<int[]> input_arr = Read_input(path);
            //Console.WriteLine(input_arr);
            //List<int[]> input_arr = new List<int[]>()
            //{
            //    new int[]{2, -1, 3, 1, -4, 2},
            //    new int[]{1, 0, -2, -3, 2, 1},
            //    new int[]{-3, 1, 1, 0, -1, 2},
            //    new int[]{4, 1, -1, -2, 0, 1}
            //};

            Diagonalize(input_arr);

            int[] f_arr = Min_m(input_arr);
            input_arr.Remove(f_arr);
            int[][] basis = set_basis(f_arr);

            //#region Maybe_it_doesn't_need
            //int[] f_arr = new int[basis.GetLength(0)];
            //for (int e = 0; e < basis.GetLength(0); e++) //f_arr=cur_eq;
            //{
            //    f_arr[e] = basis[e][e] * cur_eq[e];
            //}
            //#endregion
            List<int[]> x = new List<int[]>() { };
            //write_arr(f_arr);
            //f_arr.ToList<int>().ForEach(i => Console.Write(i + " "));
            //Console.WriteLine();


            /*if value of eq is zero, add corresponding basis vector to answer
            otherwise for every pair of ei and ej, add y = -L(ei)*ej + L(ej)*ei, where ej - basis for which L>0, ei - for which L<0 */
            for (int i = 0; i < f_arr.Length; i++)
            {
                if (f_arr[i] == 0)
                {
                    x.Add(basis[i]);
                }
                else if (f_arr[i] < 0)
                {
                    for (int j = 0; j < f_arr.Length; j++)
                    {
                        if (f_arr[j] > 0)
                        {
                            int[] arr1 = Map<int, int>(z => -f_arr[i] * z, basis[j]).ToArray();
                            int[] arr2 = Map<int, int>(z => f_arr[j] * z, basis[i]).ToArray();
                            int[] y = plus(arr1, arr2);
                            int d = Reduce<int, int>(GCD, y, 0);
                            if (d != 1)
                                y = Map<int, int>(z => z / d, y).ToArray();
                            x.Add(y);
                        }
                    }
                }
            }

            //x.ForEach(i => write_arr(i));

            while (input_arr.Count > 0)
            {
                List<int[]> f_arr2 = new List<int[]>() { };
                foreach (int[] e in input_arr)
                {
                    List<int> fn = new List<int>() { };
                    foreach (int[] xn in x)
                    {
                        int fx = 0;
                        for (int a = 0; a < xn.Length; a++)
                        {
                            fx += xn[a] * e[a];
                        }
                        fn.Add(fx);
                    }
                    f_arr2.Add(fn.ToArray());
                }
                int[] cur_eq2 = Min_m(f_arr2);
                input_arr.RemoveAt(f_arr2.IndexOf(cur_eq2));
                Console.WriteLine("{0} {1}", cur_eq2.Length, input_arr.Count);
                List<int[]> x2 = new List<int[]>() { };
                for (int i = 0; i < cur_eq2.Length; i++)
                {
                    if (cur_eq2[i] == 0)
                        x2.Add(x[i]);
                    else if (cur_eq2[i] < 0)
                    {
                        for (int j = 0; j < cur_eq2.Length; j++)
                        {
                            if (cur_eq2[j] > 0)
                            {

                                int[] arr1 = Map<int, int>(t => -cur_eq2[i] * t, x[j]).ToArray();
                                int[] arr2 = Map<int, int>(t => cur_eq2[j] * t, x[i]).ToArray();
                                int[] y = plus(arr1, arr2);
                                x2.Add(y);
                            }
                        }
                    }
                }
                
                //test
                Console.WriteLine(x2.Count); 
                Console.WriteLine("start r2");
                x2 = redundant2(x2, input_arr.Count);
                Console.WriteLine(x2.Count);
                Console.WriteLine("start r");
                x2 = redundant(x2);
                Console.WriteLine("finish r");
                Console.WriteLine(x2.Count);
                x2 = siplify(x2);

                //x2.ForEach(i => write_arr(i));
                //Console.WriteLine();
                x = x2;
            }
            return x;
        }

        public static void Main(string[] args)
        {
            string project_root = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
            string path_old = @"input(old).txt";
            string path_small = @"input.txt";
            string path_medium = @"input25x41.txt";
            string path_big = @"input40x61(10x30).txt";

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            List<int[]> x = Work(project_root + "\\" + path_medium);

            sw.Stop();
            WriteToFile(x);
            x.ForEach(i => write_arr(i));
            Console.WriteLine();
            Console.WriteLine((sw.ElapsedMilliseconds / 100.0).ToString());
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(project_root + @"\output.txt", true)) // writes only elapsed wtime, not a solution
            {
                foreach (var i in x)
                {
                    foreach (var a in i)
                    {
                        file.Write(a + ", ");
                    }
                    file.WriteLine();
                }
                file.WriteLine((sw.ElapsedMilliseconds / 100.0).ToString());
            }
            Console.ReadKey();
        }
    }
}
