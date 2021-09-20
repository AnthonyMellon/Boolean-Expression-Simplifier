using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boolean_Expression_Simplifier
{
    class Program
    {
        static void populateTruthTable(bool[,] TruthTable)
        {
            bool output;
            for (int i = 0; i < TruthTable.GetLength(0) - 1; i++)
            {
                output = false;
                int y = 0;
                for (int j = 0; j < Math.Pow(2, i + 1); j++)
                {
                    for (int k = 0; k < Math.Pow(2, ((TruthTable.GetLength(0) - 2) - i)); k++)
                    {
                        TruthTable[i, y] = output;
                        y++;
                    }
                    output = !output;
                }
            }
        }

        static void displayTruthTable(bool[,] TruthTable)
        {
            for (int i = 65; i < TruthTable.GetLength(0)-1 + 65; i++)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write((char)i);
                RowPad();
            }
            Console.WriteLine();

            for (int i = 0; i < TruthTable.GetLength(1)-1; i++)
            {
                for (int j = 0; j < TruthTable.GetLength(0) - 1; j++)
                {
                    int value = TruthTable[j, i] ? 1 : 0;
                    
                    backGroundBinaryColour(value);
                    Console.Write((value).ToString());
                    RowPad();
                }
                Console.WriteLine();
            }
        }

        static void backGroundBinaryColour(int value)
        {
            if (value == 1)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
        }

        static void RowPad()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(" | ");
        }

        static void Main(string[] args)
        {
            string oldOut;
            int numInputs;
            double numOutputs;
            bool[,] originalTruthTable;

            Console.WriteLine("How many inputs are there?");
            numInputs = System.Convert.ToInt16(Console.ReadLine());
            numOutputs = Math.Pow(2, numInputs);

            Console.WriteLine("What are the outputs?");
            oldOut = Console.ReadLine();

            originalTruthTable = new bool[numInputs + 1, (int)numOutputs + 1];
            populateTruthTable(originalTruthTable);

            displayTruthTable(originalTruthTable);
            Console.ReadLine();
        }



    }
}
