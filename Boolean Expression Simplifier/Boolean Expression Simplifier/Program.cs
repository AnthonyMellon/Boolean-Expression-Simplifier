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
            for (int row = 0; row < TruthTable.GetLength(1)-1; row++)
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

                    displayValue(Convert.ToString((TruthTable[col, row] ? 1 : 0))[0], backGroundBinaryColour(TruthTable[col, row]), true);
                }

                //Calculate and populate the output value
                bool outputValue = true;
                foreach (string expression in ANDexpressions) //Loop through each expression after sperating by ors
                {
                    outputValue = true;
                    for (int term = 0; term < expression.Length; term++)  //A, !, B
                    {
                        if (expression[term] != '!')
                        {
                            if (term != 0)  //Make sure there is a term before this one
                            {
                                if (expression[term-1] == '!') //If there is a not before this term
                                {
                                    if (TruthTable[expression[term] - 65, row] == true)
                                    {
                                        outputValue = false;
                                    }
                                }
                                else //If there is no not before this term
                                {
                                    if (TruthTable[expression[term] - 65, row] == false)
                                    {
                                        outputValue = false;
                                    }
                                }
                            }
                            else //If this is the first term
                            {
                                if (TruthTable[expression[term] - 65, row] == false)
                                {
                                    outputValue = false;
                                }
                            }
                        }
                    }
                    if (outputValue == true)
                    {
                        break; // <-- i hate this
                    }
                }

                //Display the output value
                TruthTable[myTruthTable.GetLength(0) - 1, row] = outputValue;
                displayValue(Convert.ToString((outputValue ? 1 : 0))[0], backGroundBinaryColour(outputValue), true);
                Console.WriteLine();

            }
        }

        static void truthTableHeader(bool[,] TruthTable)
        {
            for (int i = 65; i < TruthTable.GetLength(0) - 1 + 65; i++)
            {
                displayValue((char)i, ConsoleColor.Cyan, true);
            }
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Z");
            RowPad();
            Console.WriteLine();
        }
        #endregion

        static void generateKarnaughMap(bool[,] TruthTable)
        {
            int sizeX;
            int numInputsX;
            int sizeY;
            int numInputsY;

            //Get the number of inputs for each axis. Use this to find the number of outputs along each axis
            numInputsX = (int)Math.Round(((double)(TruthTable.GetLength(0) - 1) / 2) - 0.1);
            numInputsY = (int)Math.Round(((double)(TruthTable.GetLength(0) - 1) / 2) + 0.1);
            sizeX = (int)Math.Pow(2, numInputsX);
            sizeY = (int)Math.Pow(2, numInputsY);

            bool[,] myKarnaughMap = new bool[sizeX, sizeY];

            //Populate KarnaughMap            
            for (int x = 0; x < sizeX; x++) //This takes forever
            {
                for (int y = 0; y < sizeY; y++)
                {               
                    //Generate greycodes for x and y axis labels       
                    bool[,] greyCodeX = generateGreyCode(numInputsX);
                    bool[,] greyCodeY = generateGreyCode(numInputsY);


                    //copy the binary number from the grey code for the x axis for the current row
                    int[] binX;
                    binX = new int[greyCodeX.GetLength(0)];
                    for(int i = 0; i < binX.Length; i++)
                    {
                        binX[i] = greyCodeX[i, x] ? 1 : 0;
                    }



                    //copy the binary number from the grey code for the x axis for the current row
                    int[] binY;
                    binY = new int[greyCodeY.GetLength(0)];
                    for(int i = 0; i < binY.Length; i++)
                    {
                        binY[i] = greyCodeY[i, y] ? 1 : 0;
                    }


                    //Convert the binary numbers for the current x / y axis label into decimal numbers to be used for indexing
                    int deciX = binaryToDecimal(binX);
                    int deciY = binaryToDecimal(binY);

                    int outputIndex = deciX + (sizeX*deciY);
                    //Console.WriteLine($"X:{x} , Y:{y} , DX:{deciX} , DY:{deciY} , I:{outputIndex}");
                    //Console.WriteLine(myTruthTable[TruthTable.GetLength(0) - 1, outputIndex]);


                    //Copy the output over from the truth table
                    myKarnaughMap[x, y] = myTruthTable[TruthTable.GetLength(0)-1, outputIndex];
                }
            }

            drawKarnaughMap(myKarnaughMap, numInputsX, numInputsY);
        }

        static void drawKarnaughMap(bool[,] karnaughMap, int numInputsX, int numInputsY)
        {            
            bool[,] greyCodeX = generateGreyCode(numInputsX);
            bool[,] greyCodeY = generateGreyCode(numInputsY);

            //Draw greycode labels above each column
            for(int i = 0; i < greyCodeX.GetLength(0); i++)
            {
                //Pad over to align with karnaugh map
                for(int j = 0; j < numInputsY; j++)
                {
                    Console.Write(" ");
                }
                RowPad();

                for(int j = 0; j < greyCodeX.GetLength(1); j++)
                {
                    char val = (greyCodeX[i, j] ? 1 : 0).ToString()[0];
                    displayValue(val, backGroundBinaryColour(greyCodeX[i, j]), true);
                }
                Console.WriteLine();                               
            }

            for(int i = 0; i < greyCodeY.GetLength(0); i++) //Add input headings to y axis
            {
                displayValue((char)(i + 65), ConsoleColor.Cyan, false);
                //Console.Write((char)(i+65));
            }
            Console.WriteLine();

            for (int i = 0; i < karnaughMap.GetLength(1); i++) //Loop through each row
            {
                //Append the grey code label in front of each row
                for(int j = 0; j < greyCodeY.GetLength(0); j++)
                {
                    char val = (greyCodeY[j, i] ? 1: 0).ToString()[0];
                    displayValue(val, backGroundBinaryColour(greyCodeY[j, i]), false);
                }                

                RowPad();

                //Draw the output bits
                for (int j = 0; j < karnaughMap.GetLength(0); j++) //Loop through each column
                {
                    displayValue(Convert.ToString(karnaughMap[j, i] ? 1 : 0)[0], backGroundBinaryColour(karnaughMap[j, i]), true);
                }
                Console.WriteLine();
            }
        }

        static bool[,] generateGreyCode(int numInputs)
        {
            bool[,] greyCode = new bool[numInputs,(int)Math.Pow(2,numInputs)];

            for(int i = 0; i < numInputs; i++) //Loop through each input
            {
                int largeGreyCodeLength = (int)(Math.Pow(2, numInputs)*2);
                bool[] largeGreyCode = new bool[largeGreyCodeLength];
                
                int index = 0;
                bool output = false;

                while (index < largeGreyCode.Length)
                {
                    for (int k = 0; k < Math.Pow(2, numInputs - i); k++) //K is less than 2 to the power of inverse index
                    {
                        largeGreyCode[index] = output;
                        index++;
                    }
                    output = !output;
                }

                for(int j = 0; j < greyCode.GetLength(1); j++)
                {
                    greyCode[i, j] = largeGreyCode[j + (int)Math.Pow(2, numInputs - (i + 1))];
                }
            }

            return greyCode;
        }

        #region Minor_Processing_Functions

        static int binaryToDecimal(int[] binaryNumber)
        {
            int decimalNumber = 0;

            for(int i = 0; i < binaryNumber.Length; i++)
            {
                if(binaryNumber[i] == 1)
                {
                    decimalNumber += (int)(Math.Pow(2, binaryNumber.Length - i - 1));
                }
            }

            return decimalNumber;
        }

        static void displayValue(char value, ConsoleColor colour, bool pad)
        {
            Console.ForegroundColor = colour;
            Console.Write(value);
            if(pad)
            {
                RowPad();
            }

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
            generateTruthTable(myTruthTable, oldOut);

            Console.WriteLine("\n\n----------\n\n");

            //Generate and draw the Karnaugh Map
            generateKarnaughMap(myTruthTable);           

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
