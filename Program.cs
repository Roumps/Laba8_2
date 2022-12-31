using System;
using System.IO;

namespace Program
{
    class Program
    {
        static string[] symbols = new string[] { "-", " ", ":" };

        public static void Main()
        {
            string[] allLines = File.ReadAllLines("Laba8_2.txt");
            string[][] sortedLines = SortLines(allLines);
            int balance = int.Parse(allLines[0]);

            if (GetBalanceStatus(sortedLines, balance) > 0)
                FindBalanceByDate(sortedLines, balance);
            else
                Console.WriteLine("Incorect file!!!");
        }

        public static void FindBalanceByDate(string[][] array, int balance)
        {
            Console.Write("Введите дату и время в формате ( YYYY-MM-DD HH:mm ) : ");
            string date = Console.ReadLine();
            Console.WriteLine(GetBalanceStatus(array, balance, date));
        }

        public static string[][] SortLines(string[] allLines)
        {
            string[][] result = new string[allLines.Length - 1][];

            // Разделяем строки по строке " | "
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = allLines[i + 1].Split(" | ");

                foreach (string symbol in symbols)
                {
                    result[i][0] = result[i][0].Replace(symbol, "");
                }
            }

            // Представляем даты в виде чисел(long) и сортируем по возрастанию
            for (int i = 1; i < result.Length; i++)
            {
                for (int j = 1; j < result.Length; j++)
                {
                    long firstDate = long.Parse(result[j - 1][0]);
                    long secondDate = long.Parse(result[j][0]);

                    if (firstDate > secondDate)
                    {
                        string[] minDate = result[j];
                        result[j] = result[j - 1];
                        result[j - 1] = minDate;
                    }
                }
            }

            return result;
        }

        public static int GetBalanceStatus(string[][] arrays, int balance, string date = "")
        {
            foreach (string symbol in symbols)
            {
                date = date.Replace(symbol, "");
            }

            for (int i = 0; i < arrays.Length; i++)
            {
                string[] line = arrays[i];

                if (line.Length > 2)
                    balance = UpdateBalance(balance, int.Parse(line[1]), line[2]);
                else
                {
                    line = arrays[i - 1];
                    balance = UpdateBalance(balance, int.Parse(line[1]), line[2] + "_REVERT");
                }

                if (balance < 0 || (date == line[0] && date != arrays[i + 1][0])) return balance;
            }
            return balance;
        }

        public static int UpdateBalance(int balance, int num, string attr)
        {
            switch (attr)
            {
                case "in":
                    balance += num;
                    break;
                case "out":
                    balance -= num;
                    break;
                case "in_REVERT":
                    balance -= num;
                    break;
                case "out_REVERT":
                    balance += num;
                    break;
            }
            return balance;
        }
    }
}