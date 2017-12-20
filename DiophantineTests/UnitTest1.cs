using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Diophantine_sharp;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DiophantineTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestDiagonalize()
        {
            List<int[]> test_arr = new List<int[]>()
            {
                new int[]{2, -1, 3, 1, -4, 2},
                new int[]{1, 0, -2, -3, 2, 1},
                new int[]{-3, 1, 1, 0, -1, 2},
                new int[]{4, 1, -1, -2, 0, 1}
            };

            Program.Diagonalize(test_arr);
            foreach (var a in test_arr)
            {
                Console.Write(a + ", ");
            }
            Console.WriteLine();

            bool isDiagonalized = true;
            for (int i=0; i<test_arr.Count; i++)
            {
                for (int j = 0; j<test_arr.Count; j++)
                {
                    if ((i != j) && (test_arr[i][j] != 0))
                    { isDiagonalized = false; }
                    if ((i == j) && (test_arr[i][j] == 0))
                    { isDiagonalized = false; }

                }
            }

            Assert.IsTrue(isDiagonalized, "matrix was not diagonalized");
        }

        [TestMethod]
        public void TestOldInput()
        {
            string path_old = @"C:\Users\dmitr\Source\Repos\Diophantine_sharp\Diophantine_sharp_0.1\input(old).txt";

            List<int[]> x = Program.Work(path_old);

            List<int[]> answer =  new List<int[]>()
            {
                new int[]{2, 0, 3, 5, 7, 5},
                new int[]{16, 15, 89, 0, 76, 10}
            };

            if (x.Count != 2)
            {
                Assert.Fail("wrong answer");
            }
            else
            {
                Assert.IsTrue(((Enumerable.SequenceEqual(x[0], answer[0]) && Enumerable.SequenceEqual(x[1], answer[1])) || (Enumerable.SequenceEqual(x[0], answer[1]) && Enumerable.SequenceEqual(x[1], answer[0]))), "wrong answwer");
            }
        }

        [TestMethod]
        public void TestSmallInput()
        {
            string path_small = @"C:\Users\dmitr\Source\Repos\Diophantine_sharp\Diophantine_sharp_0.1\input.txt";

            List<int[]> x = Program.Work(path_small);

            Assert.AreEqual(104, x.Count, "wrong answer");
        }
    }
}
