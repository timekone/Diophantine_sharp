﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.IO;

namespace Diophantine_sharp
{
    public class Program
    {
        static void write_arr(int[] arr)  //printing array to console with commas
        {
            foreach (var a in arr)
            {
                Console.Write(a + ", ");
            }
            Console.WriteLine();
        }
        static T Reduce<T, U>(Func<U, T, T> func, IEnumerable<U> list, T acc)  //applies function cumulatively to the items of list
        {
            foreach (var i in list)
                acc = func(i, acc);

            return acc;
        }

        static IEnumerable<TResult> Map<T, TResult>(Func<T, TResult> func, IEnumerable<T> list)  //return an iterator that applies function to every item of iterable, yielding the results
        {
            foreach (var i in list)
                yield return func(i);
        }

        static int[] Min_m(List<int[]> arr)  //Choose equation with min m=k*l+r where k - number of positive coefficients, l - negative, r - zeroes
        {
            List<int> m_arr = new List<int>();  //list with values of m for each equation
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
            return arr[m_arr.IndexOf(m_arr.Min())];  //index of min m equals to index of its equation
        }

        static int GCD(int a, int b)  //Greatest Common Divisor
        {
            if (b == 0)
                return a;
            else
                return GCD(b, a % b);
        }

        static List<int[]> redundant2(List<int[]> ar, int len_input_arr)  //delete redundant vectors
        {
            List<int[]> ar2 = new List<int[]>();  //for storing halfway solutions
            List<string> strings = new List<string>();
            for (int j = 0; j < ar.Count; j++)  //searching for vectors with number of zero coordinates greater then number of vectors
            {
                int zcounter = 0;  //number of zero coordinates in a single vector
                string s = "";  //binary representation of vector
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
                    strings.Add(s);  //saving binary representation for later
                }
            }
            List<int[]> ar3 = new List<int[]>();
            List<string> strings2 = new List<string>();
            for (int j = 0; j < ar2.Count; j++)
            {
                if (!strings2.Contains(strings[j]))  //eliminating vectors without unique bin. representation
                {
                    ar3.Add(ar2[j]);
                    strings2.Add(strings[j]);
                }
            }

            //redundant with bool optimisation(old redundant)
            UInt64[] decimal_ar = new UInt64[strings2.Count];
            for (int i = 0; i < strings2.Count; i++)
            {
                decimal_ar[i] = Convert.ToUInt64(strings2[i], 2);  //translating binary representation to decimal
            }
            ar2.Clear();  //reusing ar2 to save memory
            for (int j = 0; j < ar3.Count; j++)
            {
                bool r = true;  //do we need ar[j]
                for (int k = 0; k < ar3.Count; k++)
                {
                    if (j != k)  //are these different vectors
                    {
                        UInt64 an = decimal_ar[j] & decimal_ar[k];  //AND between binary representations of vec
                        if (an == decimal_ar[k]) //if [j] has 1's where [k] has 0's, e.g. [j] redundant to [k]
                        {
                            r = false;
                            break;
                        }
                    }
                }
                if (r)
                {
                    ar2.Add(ar3[j]);
                }
            }
            return ar2;
        }

        static List<int[]> simplify(List<int[]> ar)  //simplifying vectors of matrix
        {
            List<int[]> ar2 = new List<int[]>() { };  //for storing result
            foreach (int[] y in ar)
            {
                int[] y2 = y;
                int d = Reduce(GCD, y, 0);  //searching for greatest common divisor
                if (d!=0 && d != 1)
                {
                    y2 = Map<int, int>(z => z / d, y).ToArray();  //dividing by GCD
                }
                ar2.Add(y2);  //append to answer
            }
            return ar2;
        }

        static int[][] set_basis(int[] arr)  //creating canonical basis matrix
        {
            int[][] basis = new int[arr.Length][];
            for (int p = 0; p < arr.Length; p++)
            {
                basis[p] = new int[arr.Length];
                for (int o = 0; o < arr.Length; o++)  //putting 1's on diagonal
                {
                    if (p == o)
                        basis[p][o] = 1;
                    else
                        basis[p][o] = 0;
                }
            }
            return basis;
        }

        public static void Diagonalize(List<int[]> ar)  //diagonalize input matrix
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
                    int[] c = ar[i];  //switch places
                    ar[i] = ar[i + t];
                    ar[i + t] = c;
                    if (ar[i][i] == 0)
                    {
                        break;
                    }
                }
                //subtract vectors
                for (int j = 0; j < i; j++)
                {
                    if (ar[j][i] != 0)
                    {
                        int[] newj = new int[ar[j].Length];
                        for (int k = 0; k < ar[i].Length; k++)
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
                ar = simplify(ar);
            }
        }

        public static List<int[]> Read_input(string path)  //read input matrix
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

        static void WriteToFile(List<int[]> x)  //write answer
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

        public static List<int[]> Work(string path)  //primary function
        {
            List<int[]> input_arr = Read_input(path);
            Diagonalize(input_arr);  //diagonalize input matrix
            int[] f_arr = Min_m(input_arr);  //choosing equation with min m
            input_arr.Remove(f_arr);
            int[][] basis = set_basis(f_arr);  //setting canon basis for given equation
             List <int[]> x = new List<int[]>() { };  //for storing answer


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
                            int[] y = arr1.Zip(arr2, (o, p) => o + p).ToArray();
                            int d = Reduce<int, int>(GCD, y, 0);  //find greatest common divider of vector..
                            if (d != 1)  //# and if it is not 1..
                                y = Map<int, int>(z => z / d, y).ToArray();  //simplify vector
                            x.Add(y);
                        }
                    }
                }
            }
            
            while (input_arr.Count > 0)
            {
                List<int[]> f_arr2 = new List<int[]>() { };
                foreach (int[] e in input_arr)  //for each equation left..
                {
                    List<int> fn = new List<int>() { };
                    foreach (int[] xn in x)  //substitute every answer vector
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
                int[] cur_eq2 = Min_m(f_arr2);  //choose equation with min m=k*l+r on f_arr2 values
                input_arr.RemoveAt(f_arr2.IndexOf(cur_eq2));
                Console.WriteLine("{0} {1}", cur_eq2.Length, input_arr.Count);
                List<int[]> x2 = new List<int[]>() { };
                for (int i = 0; i < cur_eq2.Length; i++) //searching for TSS - solutions
                {
                    if (cur_eq2[i] == 0)  //if zero just add to solutions
                        x2.Add(x[i]);
                    else if (cur_eq2[i] < 0)  //otherwise combining positive with negative
                    {
                        for (int j = 0; j < cur_eq2.Length; j++)
                        {
                            if (cur_eq2[j] > 0)
                            {

                                int[] arr1 = Map<int, int>(t => -cur_eq2[i] * t, x[j]).ToArray();
                                int[] arr2 = Map<int, int>(t => cur_eq2[j] * t, x[i]).ToArray();
                                int[] y = arr1.Zip(arr2, (o, p) => o + p).ToArray();
                                x2.Add(y);
                            }
                        }
                    }
                }
                
                //test
                Console.WriteLine(x2.Count); //length before elimination of redundant vectors
                Console.WriteLine("start r2"); 
                x2 = redundant2(x2, input_arr.Count);
                Console.WriteLine("finish r2");
                Console.WriteLine(x2.Count);  //after elimination
                x2 = simplify(x2);
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

            List<int[]> x = Work(project_root + "\\" + "input(old).txt");

            sw.Stop();
            WriteToFile(x);
            x.ForEach(i => write_arr(i));  //writing each TSS-solution to console
            Console.WriteLine();
            Console.WriteLine((sw.ElapsedMilliseconds / 100.0).ToString());  //write time
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
