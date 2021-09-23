using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boolean_Expression_Simplifier
{
    class Program
    {
        static void populateTruthTable(bool[,] TruthTable, string algerbraicExpression)
        {

            //Populate input values
            bool inputValue;

            for (int i = 0; i < TruthTable.GetLength(0) - 1; i++)
            {
                inputValue = false;
                int y = 0;
                for (int j = 0; j < Math.Pow(2, i + 1); j++)
                {
                    for (int k = 0; k < Math.Pow(2, ((TruthTable.GetLength(0) - 2) - i)); k++)
                    {
                        TruthTable[i, y] = inputValue;
                        y++;
                    }
                    inputValue = !inputValue;
                }
            }

            //Calculate and populate output values
            string[] expressions;
            expressions = algerbraicExpression.Split('+');
            bool outputValue;

            for (int i = 0; i < TruthTable.GetLength(1); i++)
            {
                outputValue = true;
                foreach (string expression in expressions)
                {
                    outputValue = true;
                    foreach (char term in expression.ToCharArray())
                    {
                        if (TruthTable[term - 65, i] == false)
                        {
                            outputValue = false;
                        }
                    }
                    if (outputValue == true)
                    {
                        break; // <-- I hate this
                    }
                }
                TruthTable[TruthTable.GetLength(0)-1, i] = outputValue;
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
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Z");
            RowPad();
            Console.WriteLine();


            for (int i = 0; i < TruthTable.GetLength(1)-1; i++)
            {
                for (int j = 0; j < TruthTable.GetLength(0); j++)
                {
                    int value = TruthTable[j, i] ? 1 : 0;
                    
                    backGroundBinaryColour(value);
                    Console.Write((value).ToString());
                    RowPad();
                }
                Console.WriteLine();
            }
            Console.WriteLine();
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

        static bool exitTest()
        {
            Console.WriteLine("Run again?");
            return Console.ReadLine().ToLower() == "yes";
        }

        static void Main(string[] args)
        {
            string oldOut;
            int numInputs;
            double numOutputs;
            bool[,] myTruthTable;

            do
            {
                //Setup
                Console.Clear();
                Console.WriteLine("How many inputs are there?");
                numInputs = System.Convert.ToInt16(Console.ReadLine());
                numOutputs = Math.Pow(2, numInputs);

                Console.WriteLine("Whats the boolean expression?");
                oldOut = Console.ReadLine();

                myTruthTable = new bool[numInputs + 1, (int)numOutputs + 1];

                //Calculate the outputs based on the expression




                //Populate and display the truth table
                populateTruthTable(myTruthTable, oldOut);
                displayTruthTable(myTruthTable);

            } while (exitTest());
        }



    }
}
