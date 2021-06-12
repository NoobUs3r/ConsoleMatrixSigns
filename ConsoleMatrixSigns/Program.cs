using System;
using System.Threading;

namespace GreenMatrixSigns
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;

            int originalScreenWidth = -1;
            int originalScreenHeigh = -1;
            int maxSequentialChars = -1;
            int[] topRow = { -1 };
            int[] topRowSequentialCount = { -1 }; // Counts how many sequential chars there are in one column 

            string output = string.Empty;
            string[] chars = { "$", "&", "#", "^", "У" };

            while (true)
            {       
                // Clearing everything in case of console's hight and width change
                if (Console.WindowHeight != originalScreenHeigh || Console.WindowWidth != originalScreenWidth)
                {
                    try
                    {
                        Console.Clear();
                    }
                    catch (Exception)
                    {
                        continue; // In case there is no visible console window
                    }

                    originalScreenWidth = Console.WindowWidth;
                    originalScreenHeigh = Console.WindowHeight;
                    maxSequentialChars = originalScreenHeigh / 2;

                    topRow = new int[originalScreenWidth];
                    topRowSequentialCount = new int[originalScreenWidth];
                    FillArrayWithRandomValues(ref topRow, 1, 10); // 1/10 chance of non-empty column

                    for (int i = 0; i < originalScreenWidth; i++)
                    {
                        if (topRow[i] == 1)
                            topRowSequentialCount[i] = 1;
                    }

                    output = string.Empty;

                    for (int j = 0; j < originalScreenHeigh - 1; j++)
                    {
                        output += "\n";
                    }
                }

                // Adding random characters from chars to the top row
                string topRowString = string.Empty;

                for (int k = 0; k < topRow.Length; k++)
                {
                    if (topRow[k] == 1)
                    {
                        Random r = new Random();
                        int rInt = r.Next(0, chars.Length); // Select a random char index from chars list

                        topRowString += chars[rInt];
                    }
                    else
                        topRowString += " ";
                }

                // Keeping count of sequential characters
                for (int m = 0; m < originalScreenWidth; m++)
                {
                    Random r = new Random();

                    if (topRowSequentialCount[m] == 0) // If there are no sequential chars
                    {
                        int rInt = r.Next(1, 30); // 1/30 chance of a new char added

                        if (rInt != 1)
                        {
                            topRow[m] = 0;
                        }
                        else
                        {
                            topRow[m] = 1;
                            topRowSequentialCount[m] = 1;
                        }
                    }
                    else if (topRowSequentialCount[m] < maxSequentialChars) // If there are sequential chars but less than max allowed
                    {
                        int rInt = r.Next(1, 10); // 1/10 chance of breaking the line of sequential chars

                        if (rInt == 1)
                        {
                            topRow[m] = 0;
                            topRowSequentialCount[m] = 0;
                        }
                        else
                        {
                            topRowSequentialCount[m] += 1;
                        }
                    }
                    else // If there are sequential chars and their count >= max allowed
                    {
                        int rInt = r.Next(1, 10); // 1/10 chance of continuing the line of sequential chars

                        topRow[m] = rInt;

                        if (rInt == 1)
                        {
                            topRowSequentialCount[m] += 1;
                        }
                        else
                        {
                            topRowSequentialCount[m] = 0;
                        }
                    }
                }

                ChangeSymbolsInString(ref output, chars);
                AddStringToBeginningOfString(ref output, topRowString);
                RemoveLastRowOfString(ref output);

                Console.Clear();
                Console.Write(output);
                Thread.Sleep(100);
            }
        }

        static void AddStringToBeginningOfString(ref string input, string newString)
        {
            input = newString + "\n" + input;
        }

        static void RemoveLastRowOfString(ref string input)
        {
            int lastNewLineIndex = input.LastIndexOf("\n");
            input = input.Substring(0, lastNewLineIndex);
        }

        static void FillArrayWithRandomValues(ref int[] array, int minNumber, int maxNumber)
        {
            for (int i = 0; i < array.Length; i++)
            {
                Random r = new Random();
                int rInt = r.Next(minNumber, maxNumber);

                array[i] = rInt;
            }
        }

        static void ChangeSymbolsInString(ref string str, string[] randomSymbols)
        {
            string updatedString = string.Empty;

            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == '\n' || str[i] == ' ')
                {
                    updatedString += str[i];
                    continue;
                }

                Random r = new Random();
                int rInt = r.Next(0, randomSymbols.Length);
                updatedString += randomSymbols[rInt];
            }

            str = updatedString;
        }
    }
}
