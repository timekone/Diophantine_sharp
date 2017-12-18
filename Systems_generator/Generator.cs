using System;
using System.IO;
using System.Text;

namespace Systems_generator
{
    class Program
    {
        static void Main(string[] args)
        {
            //int x = 30;
            //int y = 18;
            int x = 50;
            int y = 45;
            int[] coofs = { 1, 1, -1, 0, 0, 0, 0};
            //int[] coofs2 = { 1, 1, 0, 1, 1, 1, 1, 0, 0, 0, 1, 1, 0, 0, 1, 0, 0, 0, 0, 1, 1, 1, 0, 0, 1, 0, 1, 0, 0, 1 };
            int[] vector = { 1, 1, 0, 1, 1, 1, 1, 0, 0, 0,
                             1, 1, 0, 0, 1, 0, 0, 0, 0, 1,
                             1, 1, 0, 0, 1, 0, 1, 0, 0, 1,
                             0, 0, 0, 1, 1, 0, 0, 1, 1, 1,
                             0, 0, 1, 0, 1, 1, 0, 0, 0, 1};
            //int[] vector = new int[x + 1];
            //Random rnd1 = new Random();
            //for (int i = 0; i<x; i++)
            //{
            //    int index = rnd1.Next(0, coofs2.Length);
            //    vector[i] = coofs2[index];
            //}
            //vector[x] = 1;
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(new System.IO.FileStream("generated.txt", FileMode.Create)))
            {
                Random rnd = new Random();
                for(int i = 0; i<y; i++)
                {
                    int equation_sum = 0;
                    for(int j = 0; j<vector.Length-1; j++)
                    {
                        int index = rnd.Next(0, coofs.Length);
                        file.Write(coofs[index] + " ");
                        equation_sum += coofs[index] * vector[j];
                    }
                    file.Write( -1*equation_sum);
                    file.WriteLine();
                }

            }
        }
    }
}
