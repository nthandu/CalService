using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Services.Core
{
    public class Calculator : ICalculator
    {
        #region Private Members

        private static readonly char[] AllowedOperators = new[] { '+', '-', '*', '/' };
        private static readonly IDictionary<char, Func<string, string, string>> PriorityOps = new Dictionary<char, Func<string, string, string>>()
        {
            {'*', (leftChar, rightChar) =>
            {
                float leftVal = float.Parse(leftChar);
                float rightVal = float.Parse(rightChar);
                return (leftVal * rightVal).ToString();
            } } ,
            {'/', (leftChar, rightChar) =>
            {
                float leftVal = float.Parse(leftChar);
                float rightVal = float.Parse(rightChar);
                return (leftVal / rightVal).ToString();
            } },
        };
        private static readonly IDictionary<char, Func<string, string, string>> LowerPriorityOps = new Dictionary<char, Func<string, string, string>>()
        {
            {'+', (leftChar, rightChar) =>
            {
                float leftVal = float.Parse(leftChar);
                float rightVal = float.Parse(rightChar);
                return (leftVal + rightVal).ToString();
            } } ,
            {'-', (leftChar, rightChar) =>
            {
                float leftVal = float.Parse(leftChar);
                float rightVal = float.Parse(rightChar);
                return (leftVal - rightVal).ToString();
            } },
        };

        #endregion

        #region ICalculator Implementaion

        /// <summary>
        /// Performs the calculation on the user input.
        /// </summary>
        /// <param name="input"></param>
        public CalculationResponse Calculate(string input)
        {
            var response = new CalculationResponse { HasError = false };

            if (string.IsNullOrWhiteSpace(input))
            {
                response.HasError = true;
                response.ErrorMessage = "Input can not be null or empty";
                return response;
            }

            // check if input contains any alphabets
            if (Regex.Matches(input, @"[a-zA-Z]").Count > 0)
            {
                response.HasError = true;
                response.ErrorMessage = "Input can not contain any alphabets";
                return response;
            }

            // Check if input contains any special characters other than the allowed operators
            // Assuming input would not contain float values, so not allowing '.' along with other special characters
            if (ContainsNotAllowedSpecialCharacters(input))
            {
                response.HasError = true;
                response.ErrorMessage = "Input can not contain special characters other than +,-,*,/";
                return response;
            }

            // Start from left to right
            var inputAfterMultiplication = PerformPriorityOperations(input);

            response.Result = inputAfterMultiplication;
            return response;
        }

        #endregion

        private string PerformPriorityOperations(string input)
        {
            string response = input;

            if (input != null &&
                (input.IndexOf("*", StringComparison.Ordinal) > 0 ||
                 input.IndexOf("/", StringComparison.Ordinal) > 0))
            {
                response = PerformPriorityCals(response, PriorityOps);
            }

            if (response != null &&
                (response.IndexOf("+", StringComparison.Ordinal) > 0 ||
                 response.IndexOf("-", StringComparison.Ordinal) > 0))
            {
                response = PerformLowPriorityCals(response, LowerPriorityOps);
            }

            return response;
        }

        private static string PerformPriorityCals(string response,
            IDictionary<char, Func<string, string, string>> operations)
        {
            if (response == null)
                return string.Empty;

            return PerformCalOps(response, operations, res => ((res.IndexOf("*") > 0 ||
                                                                res.IndexOf("/") > 0)));
        }

        private static string PerformLowPriorityCals(string response,
            IDictionary<char, Func<string, string, string>> operations)
        {
            if (response == null)
                return string.Empty;

            return PerformCalOps(response, operations, res => ((res.IndexOf("+") > 0 ||
                                                                res.IndexOf("-") > 0)));
        }

        private static string PerformCalOps(string response,
            IDictionary<char, Func<string, string, string>> operations,
            Func<string, bool> shouldContinueWithCals)
        {
            if (!shouldContinueWithCals(response))
            {
                return response;
            }
            string leftChar = string.Empty;
            string rightChar = string.Empty;
            int operatorIndex = -1;
            int charIndex = 0;
            char operand = '*';
            string updatesResponse = response;
            foreach (var inputChar in response.ToCharArray())
            {
                if (Char.IsDigit(inputChar) ||
                    inputChar == '.')
                {
                    if (operatorIndex < 0)
                    {
                        leftChar = leftChar + inputChar;
                    }
                    else
                    {
                        rightChar = rightChar + inputChar;
                        if (operatorIndex > 0 &&
                            ShallProceedWithCalculation(response, operatorIndex, charIndex + 1))
                        {
                            updatesResponse = updatesResponse.Remove(operatorIndex - (leftChar.Length),
                                leftChar.Length + rightChar.Length + 1);
                            var calResponse = operations[operand](leftChar, rightChar);
                            updatesResponse = updatesResponse.Insert(operatorIndex - (leftChar.Length),
                                calResponse);
                            break;
                        }
                    }
                }
                else
                {
                    operand = inputChar;
                    if (!operations.ContainsKey(inputChar))
                    {
                        leftChar = "";
                    }
                    else
                    {
                        operatorIndex = charIndex;
                    }
                }

                charIndex++;
            }

            return PerformCalOps(updatesResponse, operations, shouldContinueWithCals);
        }

        private static bool ShallProceedWithCalculation(string response, in int operatorIndex, int charIndex)
        {
            if (response != null &&
                response.Length > charIndex)
            {
                // Digit can contain more than one character.
                // We need to make sure next character is an operand or end of the input before proceeding with cal.
                char nextCharacter = response.ToCharArray()[charIndex];
                if (char.IsDigit(nextCharacter) ||
                    nextCharacter == '.')
                {
                    return false;
                }
            }

            return true;
        }

        private bool ContainsNotAllowedSpecialCharacters(string input)
        {
            foreach (var eachChar in input.ToCharArray())
            {
                // If Char is not a digit
                // Check if it's from the list of allowed characters
                if (!Char.IsLetterOrDigit(eachChar) &&
                    !AllowedOperators.Contains(eachChar))
                {
                    return true;
                }
            }

            return false;
        }
    }
}