using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boolean_Expression_Simplifier
{
    class Program
    {
        #region Global_Variables
        static string oldOut;
        static int numInputs;
        static double numOutputs;
        static bool[,] myTruthTable;
        #endregion

        #region Major_Processing_Functions
        static void generateTruthTable(bool[,] TruthTable, string algerbraicExpression)
        {
            truthTableHeader(TruthTable);

            string[] ANDexpressions;
            ANDexpressions = algerbraicExpression.Split('+');

            //Populate and display input values
            for (int row = 0; row < TruthTable.GetLength(1); row++)
            {

                //Calculate and populate the input values
                int total = row;
                for (int col = 0; col < TruthTable.GetLength(0) - 1; col++)
                {
                    int colValue = (int)Math.Pow(2, (TruthTable.GetLength(0) - 2) - col);
                    if (colValue <= total)
                    {
                        total -= colValue;
                        TruthTable[col, row] = true;
                    }

                    displayValue((char)(TruthTable[col, row] ? 0:1), backGroundBinaryColour(TruthTable[col, row]));
                }

                //Calculate and populate the output value
                bool outputValue = true;
                foreach (string expression in ANDexpressions)
                {
                    outputValue = true;
                    foreach (char term in expression.ToCharArray())
                    {

                        if (TruthTable[term - 65, row] == false)
                        {
                            outputValue = false;
                        }
                    }
                    if (outputValue == true)
                    {
                        break; // <-- i hate this
                    }
                }

                //Display the output value
                displayValue((char)(outputValue ? 1 : 0), backGroundBinaryColour(outputValue));
                Console.WriteLine();

            }
        }

        static void truthTableHeader(bool[,] TruthTable)
        {
            for (int i = 65; i < TruthTable.GetLength(0) - 1 + 65; i++)
            {
                displayValue((char)i, ConsoleColor.Cyan);
            }
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Z");
            RowPad();
            Console.WriteLine();
        }
        #endregion

        #region Minor_Processing_Functions
        //static void displayValue(bool value)
        //{
        //    backGroundBinaryColour(value);
        //    Console.Write((value ? 1 : 0));
        //    RowPad();
        //}

        static void displayValue(char value, ConsoleColor colour)
        {
            Console.ForegroundColor = colour;
            Console.Write(value);
            RowPad();
        }
        
        static ConsoleColor backGroundBinaryColour(bool value)
        {
            if (value)
            {
                return ConsoleColor.Green;
            }
            else
            {
                return ConsoleColor.Red;
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
        #endregion

        #region Other_Functions
        static void instructions()
        {
            Console.WriteLine("Enter algebraic expressions in form of 'AB+C'");
        }
        #endregion
      
        #region Main_Functions
        static void setup()
        {

        }
        static bool loop()
        {
            //Setup
            Console.Clear();
            instructions();

            //Get the number of inputs, use this to calculate number of outputs. Use these two values to create the truth table array
            Console.WriteLine("How many inputs are there?");
            numInputs = System.Convert.ToInt16(Console.ReadLine());
            numOutputs = Math.Pow(2, numInputs);
            myTruthTable = new bool[numInputs + 1, (int)numOutputs + 1];

            //Get the boolean expression
            Console.WriteLine("Whats the boolean expression?");
            oldOut = Console.ReadLine();

            //Populate and display the truth table
            var SW = new Stopwatch();
            SW.Start();
            //truthTableHeader(myTruthTable);
            generateTruthTable(myTruthTable, oldOut);
            SW.Stop();
            Console.WriteLine(SW.Elapsed.TotalMilliseconds);

            

            return (exitTest());
        }
        #endregion


        static void Main(string[] args)
        {
            setup();
            while (loop() == true);
        }
    }
}
