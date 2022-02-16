using Calculator_Wpf.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Calculator_Wpf.ViewModels
{
    class MainWindowViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string PropertyName = null)  //этот метод принимает один аргумент строковый, это имя свойства которое обновилось - PropertyName
                                                                               // [CallerMemberName] этот атрибут позволяет при вызове PropertyChanged не указывать название аргумента
                                                                               // а компилятор сам подставит это название, которое он возьмет из вызывающего свойства 
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName)); // через Invoke заставляем выполниться все методы, которые сообщены с делегатом PropertyChanged
                                                                                       // создаем новый экземпляр класса PropertyChangedEventArgs
        }

        private string lastOperation;       // закрытое поле  - символ последней нажатой клавиши операции
        private bool newDisplayRequired = false;        //закрытое поле - очищение дисплея перед вводом следующего числа
        private string expression;      //закрытое поле - выражение производимых вычислений
        private string display = "0";       //закрытое поле - отображание на дисплее калькулятора

        private Calculator calculator = new Calculator();     //экземпляр класса калькулятор

        public string FirstVar      //открытое свойство через которое осуществляется взаимодействие с полем FirstVar класса калькулятор
        {
            get => calculator.FirstVar; 
            set => calculator.FirstVar = value; 
        }

        public string SecondVar      //открытое свойство через которое осуществляется взаимодействие с полем SecondVar  мкласса калькулятор
        {
            get => calculator.SecondVar;
            set => calculator.SecondVar = value;
        }

        public string Operation      //открытое свойство через которое осуществляется взаимодействие с полем Operation класса калькулятор     
        {
            get => calculator.Operation;
            set => calculator.Operation = value;
        }

        public string LastOperation      //открытое свойство через которое осуществляется взаимодействие с полем lastOperation
        {
            get => lastOperation;
            set => lastOperation = value;
        }

        public string Result      //открытое свойство через которое осуществляется взаимодействие с полем Result 
        {
            get => calculator.Result;
        }

        public string DisplayCalculator      //открытое свойство через которое осуществляется взаимодействие с полем display 
        {
            get                  // возвращаем значение соот-го закрытого поля 
            {
                if (display.Length > 11)
                    return display.Substring(0, 11);
                else
                    return display;
            }
            set                // присвоит в закрытое поле  value, кт передали.
                               // И вызвать событие PropertyChanged, т.е. сообщить что состояние поля изменилось, записали новое значение
            {
                display = value;
                OnPropertyChanged();
            }
        }

        public string Expression      //свойство через которое осуществляется взаимодействие с полем expression 
        {
            get => expression;
            set
            {
                expression = value;
                OnPropertyChanged();
            }
        }


        // создаем  свойство которое будет представлять из себя команду 
        public ICommand ClearButtonCommand { get; }        //команда для нажатия клавиши Clear - 0

        public void OnClearButtonCommandExecute(object p)
        {
            DisplayCalculator = "0";
            FirstVar = string.Empty;
            SecondVar = string.Empty;
            Operation = string.Empty;
            LastOperation = string.Empty;
            Expression = string.Empty;
            newDisplayRequired = false;
        }

        public bool CanClearButtonCommandExecuted(object p)
        {
            return true;
        }


        // создаем  свойство которое будет представлять из себя команду 
        public ICommand DeleteButtonCommand { get; }        //команда нажатия клавиши Delete

        public void OnDeleteButtonCommandExecute(object p)
        {
            if (DisplayCalculator.Length > 1)
                DisplayCalculator = DisplayCalculator.Substring(0, DisplayCalculator.Length - 1);
            else
                DisplayCalculator = "0";
        }

        public bool CanDeleteButtonCommandExecuted(object p)
        {
            if (DisplayCalculator == "Error!")
                return false;
            else
                return true;
        }


        // создаем  свойство которое будет представлять из себя команду 
        public ICommand DigitalButtonCommand { get; }        //команда нажатия цифровых клавиш

        public void OnDigitalButtonCommandExecute(object p)
        {
            string button = (string)p;
            switch (button)
            {
                case "+/-":
                    if (DisplayCalculator.Contains("-"))
                        DisplayCalculator = DisplayCalculator.Remove(DisplayCalculator.IndexOf("-"), 1);
                    else
                        DisplayCalculator = "-" + DisplayCalculator;
                    break;
                case ",":
                    if (newDisplayRequired)
                        DisplayCalculator = "0,";
                    else if (!DisplayCalculator.Contains(","))
                        DisplayCalculator += ",";
                    break;
                default:
                    if (DisplayCalculator == "0" || newDisplayRequired)
                        DisplayCalculator = button;
                    else
                        DisplayCalculator += button;
                    break;
            }
            newDisplayRequired = false;
        }

        public bool CanDigitalButtonCommandExecuted(object p)
        {
            if (DisplayCalculator == "Error!")
                return false;
            else
                return true;
        }


        // создаем  свойство которое будет представлять из себя команду 
        public ICommand OneOperationButtonCommand { get; }      //команда нажатия клавиш операций, выполняемых с одним числом

        public void OnOneOperationButtonCommandExecute(object p)
        {
            string operation = (string)p;
            try
            {
                FirstVar = DisplayCalculator;
                Operation = operation;
                calculator.CalculateResult();
                Expression = Operation + "(" + Math.Round(Convert.ToDouble(FirstVar), 5) + ") = ";
                LastOperation = "=";
                DisplayCalculator = Result;
                FirstVar = DisplayCalculator;
                newDisplayRequired = true;
            }
            catch (Exception)
            {
                DisplayCalculator = "Error!";
            }
        }

        public bool CanOneOperationButtonCommandExecuted(object p)
        {
            if (DisplayCalculator == "Error!")
                return false;
            else
                return true;
        }


        // создаем  свойство которое будет представлять из себя команду 
        public ICommand TwoOperationButtonCommand { get; }      //команда нажатия клавиш операций, выполняемых с двумя числами

        public void OnTwoOperationButtonCommandExecute(object p)
        {
            string operation = (string)p;
            try
            {
                if (FirstVar == string.Empty || LastOperation == "=")
                {
                    FirstVar = DisplayCalculator;
                    LastOperation = operation;
                }
                else
                {
                    SecondVar = DisplayCalculator;
                    Operation = LastOperation;
                    calculator.CalculateResult();
                    Expression = Math.Round(Convert.ToDouble(FirstVar), 5) + " "
                        + Operation + " " + Math.Round(Convert.ToDouble(SecondVar), 5) + " = ";
                    LastOperation = operation;
                    DisplayCalculator = Result;
                    FirstVar = DisplayCalculator;
                }
                newDisplayRequired = true;
            }
            catch (Exception)
            {
                DisplayCalculator = "Error!";
            }
        }

        public bool CanTwoOperationButtonCommand(object p)
        {
            if (DisplayCalculator == "Error!")
                return false;
            else
                return true;
        }

        public MainWindowViewModel() // в конструкторе инициализируем команду и для создания используем класс RelayCommand
        {         
            DigitalButtonCommand = new RelayCommand(OnDigitalButtonCommandExecute, CanDigitalButtonCommandExecuted);
            OneOperationButtonCommand = new RelayCommand(OnOneOperationButtonCommandExecute, CanOneOperationButtonCommandExecuted);
            TwoOperationButtonCommand = new RelayCommand(OnTwoOperationButtonCommandExecute, CanTwoOperationButtonCommand);
            ClearButtonCommand = new RelayCommand(OnClearButtonCommandExecute, CanClearButtonCommandExecuted);
            DeleteButtonCommand = new RelayCommand(OnDeleteButtonCommandExecute, CanDeleteButtonCommandExecuted);
        }
        
    }
}