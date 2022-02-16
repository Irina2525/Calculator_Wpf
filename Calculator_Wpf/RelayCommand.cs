using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Calculator_Wpf
{
    class RelayCommand : ICommand //это класс обертка для команды, для того что бы был командой нужно реализовать интерфейс ICommand
    {

        //создаем  два закрытых поля 
        private readonly Action<object> execute;
        private readonly Func<object, bool> canExecute;

        public event EventHandler CanExecuteChanged //метод CanExecuteChanged срабатывающий каждый раз когда состояние CanExecute могло измениться
        {
            //события add и remove
            add => CommandManager.RequerySuggested += value; // add происходит когда очередной обработчик подписывается на события , т.е. очередной метод связывается с делегатом,
                                                             // в этом случаем связываем обработчик с делегатом RequerySuggested в классе CommandManager
            remove => CommandManager.RequerySuggested -= value; // а в случае удаления обработчика удаляем его из RequerySuggested
        }

        // что бы class RelayCommand получился универсальным делаем конструктор
        public RelayCommand(Action<object> Execute, Func<object, bool> CanExecute)
        {
            execute = Execute ?? throw new ArgumentNullException(nameof(Execute)); // делаем проверку в закрытое поле execute присвои переданный аргумент Execute только в том случае
                                                                                   // если он не null, а если null то команду не выполняем, выбросим исключение ArgumentNullException 
            canExecute = CanExecute;
        }

        // в методе вызывать соот-щий делегат
        public bool CanExecute(object parameter) => canExecute?.Invoke(parameter) ?? true; // метод проверяет доступность команды
                                                                                           // выполняет проверку не равен ли null, если не равен null вызовет метод Invoke, если равен null возращает true  

        public void Execute(object parameter) => execute(parameter); // метод выполнится, когда команда будет вызвана
    }
}