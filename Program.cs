using System;
using System.Collections.Generic;
using System.Linq;

namespace _3_kron_
{
    class Program
    {
        static void Main(string[] args)
        {
            Random rand = new Random();
            int M = 0, N = 0, choise;
            List<int> tasks = new List<int>();
            List<int> sums = new List<int>();
            List<List<int>> table = new List<List<int>>();
            Console.Write("processors: "); N = Convert.ToInt32(Console.ReadLine());
            Console.Write("task: "); M = Convert.ToInt32(Console.ReadLine());
            Console.Write("min: "); int min = Convert.ToInt32(Console.ReadLine());
            Console.Write("max: "); int max = Convert.ToInt32(Console.ReadLine());

            for (int i = 0; i < M; i++) { tasks.Add(rand.Next(min, max)); }
            for (int i = 0; i < N; i++) { table.Add(new List<int>()); }
            for (int i = 0; i < M; i++) { table[rand.Next(0, N)].Add(tasks[i]); }

            //table[0] = new List<int>(new int[] { 15, 13, 17 });
            //table[1] = new List<int>(new int[] { 11, 12, 19 });
            //table[2] = new List<int>(new int[] { 20, 11, 15,12 });
            PrintTable(table);
            


            
            while(true)
            {
                Console.WriteLine("\n\n\n1-no sort\n2-decrise\n3-CMP increase\n4-CMP decrise");
                Console.Write("Choise: ");  choise = Convert.ToInt32(Console.ReadLine());
                switch (choise)
                {
                    case 1:
                        {
                            output = "NO SORT\r\n";
                            Console.Write(output); 
                            Console.Write("Tasks: "); foreach (int x in tasks) Console.Write(x.ToString() + " ");
                            Console.WriteLine();
                            List<List<int>> matrix = Copy(table);
                            Kron(matrix, N, M);
                        }
                        break;
                    case 2:
                        {
                            output = "SORT\r\n";
                            Console.Write(output);
                            Console.Write("Tasks: "); foreach (int x in tasks) Console.Write(x.ToString() + " ");
                            Console.WriteLine();
                            List<List<int>> matrix = Copy(table);
                            Kron(matrix, N, M, true);
                        }
                        break;
                    case 3:
                        {
                            output = "CMP INCREASE\r\n";
                            Console.Write(output);
                            List<List<int>> matrix = CMP(tasks, N, M, false);
                            PrintTable(matrix);
                            Kron(matrix, N, M);
                        }
                        break;
                    case 4:
                        {
                            output = "CMP DECREASE\r\n";
                            Console.Write(output);
                            List<List<int>> matrix = CMP(tasks, N, M, true);
                            PrintTable(matrix);
                            Kron(matrix, N, M);
                        }
                        break;

                }
            }


            


        }

        static void Kron(List<List<int>> table,int N, int M, bool sort=false)
        {
            List<int> sums = new List<int>();
            if (sort == true) RowSort(ref table);
            PrintTable(table);
           


            while (true)
            {
                bool flag = false;
                sums.Clear();
                for (int i = 0; i < N; i++) { sums.Add(table[i].Sum()); }
                int delta = sums.Max() - sums.Min();
                Console.WriteLine($"\ndelta = {sums.Max()} - {sums.Min()} = {delta}");
                output += $"delta = {sums.Max()} - {sums.Min()} = {delta}\r\n\r\n";
                int minCol = Index(sums, sums.Min()), maxCol = Index(sums, sums.Max());

                for (int i = 0; i < table[maxCol].Count; i++)
                {
                    if (table[maxCol][i] < delta)
                    {
                        table[minCol].Add(table[maxCol][i]);
                        table[maxCol].RemoveAt(i);
                        flag = true;
                        break;
                    }
                }
                if (sort == true) RowSort(ref table);
                if (flag == false) break; else PrintTable(table);
            }

            Console.WriteLine("\nSecond step");
            output += "SecondStep\r\n";
            PrintTable(table);

            while (true)
            {
                bool flag = false;
                sums.Clear();
                for (int i = 0; i < N; i++) { sums.Add(table[i].Sum()); }
                int delta = sums.Max() - sums.Min();
                Console.WriteLine($"\ndelta = {sums.Max()} - {sums.Min()} = {delta}");
                output += $"delta = {sums.Max()} - {sums.Min()} = {delta}\r\n\r\n";
                int minCol = Index(sums, sums.Min()), maxCol = Index(sums, sums.Max());

                for (int i = 0; i < table[maxCol].Count; i++)
                {
                    for (int j = 0; j < table[minCol].Count; j++)
                    {
                        if (table[maxCol][i] > table[minCol][j] && (table[maxCol][i] - table[minCol][j])<delta )
                        {
                            flag = true;
                            int tmp = table[maxCol][i];
                            table[maxCol][i] = table[minCol][j];
                            table[minCol][j] = tmp;
                            goto exit;
                        }
                    }
                }
                exit:
                if (sort == true) RowSort(ref table);
                if (flag == false) break; else PrintTable(table);
            }
        }


        static List<List<int>> CMP(List<int> arr, int N, int M, bool isDecrise)
        {
            List<int> tasks = new List<int>();
            foreach (int x in arr) tasks.Add(x);
            QuickSort(tasks, 0, tasks.Count - 1, isDecrise);
            Console.Write("Tasks: "); foreach (int x in tasks) Console.Write(x.ToString() + " ");
            Console.WriteLine();
            List<List<int>> result = new List<List<int>>();
            List<int> sums = new List<int>();
            for (int i = 0; i < N; i++)
            {
                result.Add(new List<int>());
                sums.Add(0);
            }

            for (int i = 0; i < M; i++)
            {
                int min = SelectMin(sums);
                result[min].Add(tasks[i]);
                sums[min] += tasks[i];
            }

            return result;
        }

        static int SelectMin(List<int> arr)
        {
            int min = 0;
            for (int i = 1; i < arr.Count; i++)
            {
                if (arr[i] < arr[min]) min = i;
            }
            return min;
        }
        static void RowSort(ref List<List<int>> matrix)
        {
            for(int i =0; i<matrix.Count; i++)
            {
                QuickSort(matrix[i], 0, matrix[i].Count - 1, true);
            }
        }
        static void QuickSort( List<int> arr, int begin, int end, bool decr = false)
        {
            int left = begin, right = end,
                piv = arr[(left + right) / 2];
            while (left <= right)
            {
                if (decr == true)
                {
                    while (arr[left] > piv) { left++; }
                    while (arr[right] < piv) { right--; }
                }
                else
                {
                    while (arr[left] < piv) { left++; }
                    while (arr[right] > piv) { right--; }
                }

                if (left <= right)
                {
                    int tmp = arr[left];
                    arr[left] = arr[right];
                    arr[right] = tmp;
                    ++left;
                    --right;
                }
                if (begin < right)
                {
                    QuickSort(arr, begin, right, decr);

                }
                if (end > left)
                {
                    QuickSort(arr, left, end, decr);
                }
            }
        }
        static int Index(List<int> lst, int value)
        {
            for (int i = 0; i < lst.Count; i++)
                if (lst[i] == value) return i;
            throw new Exception("karaool");
        }

        static void PrintTable(List<List<int>> table)
        {
            Console.WriteLine("\n");
            int elt = 0;
            while (true)
            {
                int check = 0;
                for (int i = 0; i < table.Count; i++)
                {
                    try
                    {
                        Console.Write(table[i][elt].ToString().PadRight(3) + " ");
                        output += table[i][elt].ToString().PadRight(3) + " ";
                    }
                    catch (Exception e)
                    {
                        Console.Write("    ");
                        output += "      ";
                        check++;
                    }
                }
                elt++;
                if (check == table.Count)
                {
                    break;
                }
                Console.WriteLine();
                output += "\r\n";
            }
            List<int> sums = new List<int>();
            foreach (List<int> lst in table) sums.Add(lst.Sum());
            Console.WriteLine();
            foreach (int x in sums)
            {
                Console.Write(x.ToString().PadRight(3) + " ");
                output += x.ToString().PadRight(3) + " ";
            }
            output +="<-Sums"+ "\r\n";
        }

        static List<List<int>> Copy(List<List<int>> table)
        {
            List<List<int>> result = new List<List<int>>();
            foreach(List<int> lst in table)
            {
                result.Add(new List<int>(lst.ToArray()));
            }
            return result;
        }

        static string output = "";
    }
}
