using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.
            Console.WriteLine("Hello World, Calculator!");


            double result = Calculate("1.9 - 4.2 * 9 + 4 / 5"); //no level
            Console.WriteLine(result);

            double result2 = Calculate("7 * 5 + ( 6 - 3 * 2 )"); // 1 level
            Console.WriteLine(result2);

            double result3 = Calculate("1 + 3 * ( 6 + 5 * ( 1.5 + 2 ) ) + ( 2 * 3 + ( 6.8 + 1 ) )"); //2 level
            Console.WriteLine(result3);

            double result4 = Calculate("1 + 3 * ( 7 * 3 + 7 - ( 9 + 3 / ( 1 * 3 ) ) ) - 90 * ( 12.9 - 3 )"); //3 level
            Console.WriteLine(result4);

            double resultErr = Calculate("1 * 7 + ( * 6 )"); //Error Fomula
            Console.WriteLine(resultErr);

            Console.ReadLine();
        }

        public static double Calculate(string formulaString)
        {
            double result = 0;

            try
            {
                List<string> paramValue = new List<string>(formulaString.Split(' '));

                int noOfLvlSubOperation = 0;
                bool isClosed = false;
                //determine how many level sub operation
                foreach (string v in paramValue)
                {
                    if (v == "(")
                        if (!isClosed)
                            noOfLvlSubOperation++;

                    if (v == ")")
                        isClosed = true;
                }

                if (noOfLvlSubOperation == 0)
                {
                    result = operation(paramValue);
                }
                else
                {
                    //Calculation level by level 
                    bool isInner = false;
                    for (int c = noOfLvlSubOperation; c > 0; c--)
                    {
                        int currentLvl = 0;
                        List<string> subOps = new List<string>();
                        for (int i = 0; i < paramValue.Count; i++)
                        {
                            if (paramValue[i] == ")")
                            {
                                if (isInner)
                                {
                                    string r = operation(subOps).ToString();
                                    paramValue[i] = paramValue[i].Replace(")", r);
                                    subOps.Clear();
                                    isInner = false;
                                }
                                currentLvl--;
                            }

                            if (currentLvl == noOfLvlSubOperation && isInner)
                            {
                                subOps.Add(paramValue[i]);
                                paramValue[i] = "";
                            }

                            if (paramValue[i] == "(")
                            {
                                //isInner = true;
                                currentLvl++;
                                if (currentLvl == noOfLvlSubOperation)
                                {
                                    isInner = true;
                                    paramValue[i] = "";
                                }
                            }
                        }

                        paramValue.RemoveAll(x => x == "");

                        if (noOfLvlSubOperation == 1)
                        {
                            result = operation(paramValue);
                            break;
                        }

                        noOfLvlSubOperation--;
                        currentLvl--;

                    }
                }
            }
            catch (Exception)
            {
                Console.Write("ERROR : ");
                result = 0;
            }
            

            return result;
        }

        //calculation, multiply & divide first, add & minus second
        private static double operation(List<string> formula)
        {
            for (int p = 0; p < formula.Count; p++)
            {
                if (formula[p] == "*" || formula[p] == "/")
                {
                    if (formula[p] == "*")
                    {
                        formula[p] = (Convert.ToDouble(formula[p - 1]) * Convert.ToDouble(formula[p + 1])).ToString();
                        formula[p - 1] = "";
                        formula[p + 1] = "";
                    }
                    else
                    {
                        formula[p] = (Convert.ToDouble(formula[p - 1]) / Convert.ToDouble(formula[p + 1])).ToString();
                        formula[p - 1] = "";
                        formula[p + 1] = "";
                    }
                }
            }
            formula.RemoveAll(x => x == "");
            double returnValue = 0;
            for (int q = 0; q < formula.Count; q++)
            {
                if (q == 0)
                    returnValue = Convert.ToDouble(formula[0]);

                if (formula[q] == "+")
                {
                    returnValue += Convert.ToDouble(formula[q + 1]);
                }
                else if (formula[q] == "-")
                {
                    returnValue -= Convert.ToDouble(formula[q + 1]);
                }
            }
            return returnValue;
        }


    }
}
