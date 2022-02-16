using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator_Wpf.Models
{
    public class Calculator
    {
        private string result;      //поле результата
        public string FirstVar { get; set; }    //автосвойство 1 числа
        public string SecondVar { get; set; }     //автосвойство 2 числа
        public string Result { get => result; } //результат
        public string Operation { get; set; }       //автосвойство операции


        public Calculator()
        {
            FirstVar = string.Empty;
            SecondVar = string.Empty;
            Operation = string.Empty;
            result = string.Empty;
        }

        // открытый метод для вычисления
        public void CalculateResult()       
        {
            ValidateOperation();
            try
            {
                switch (Operation)
                {
                    case ("+"):
                        result = (Convert.ToDouble(FirstVar) + Convert.ToDouble(SecondVar)).ToString();
                        break;

                    case ("-"):
                        result = (Convert.ToDouble(FirstVar) - Convert.ToDouble(SecondVar)).ToString();
                        break;

                    case ("*"):
                        result = (Convert.ToDouble(FirstVar) * Convert.ToDouble(SecondVar)).ToString();
                        break;

                    case ("/"):

                        result = (Convert.ToDouble(FirstVar) / Convert.ToDouble(SecondVar)).ToString();

                        break;

                    case ("%"):
                        result = (Convert.ToDouble(FirstVar) / Convert.ToDouble(SecondVar)).ToString();
                        break;

                    case ("1/x"):
                        if (Convert.ToDouble(FirstVar) != 0)
                            result = (1 / (Convert.ToDouble(FirstVar))).ToString();
                        else
                            throw new Exception();
                        break;

                    case ("sqr"):
                        result = Math.Pow(Convert.ToDouble(FirstVar), 2).ToString();
                        break;

                    case ("sqrt"):
                        if (Convert.ToDouble(FirstVar) >= 0)
                            result = Math.Sqrt(Convert.ToDouble(FirstVar)).ToString();
                        else
                            throw new Exception();
                        break;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void ValidateVar(string operand)        // закрытый метод, который проверяет корректность чисел
        {
            try
            {
                Convert.ToDouble(operand);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void ValidateOperation()        //закрытый метод, который проверяет корректность 
        {
            switch (Operation)
            {
                case "%":
                case "/":
                case "*":
                case "-":
                case "+":
                    ValidateVar(FirstVar);
                    ValidateVar(SecondVar);
                    break;
                case "1/x":
                case "sqr":
                case "sqrt":

                    ValidateVar(FirstVar);
                    break;
                default:
                    throw new Exception();
            }
        }
    }
}
    
