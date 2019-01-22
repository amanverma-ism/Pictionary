using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Pictionary.Capsules
{
    public class BaseCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        Action<object> _execute;
        Func<bool> _canExecute;

        public BaseCommand(Action action)
        {
            _execute = p => action();
            _canExecute = null;
        }
        public BaseCommand(Action action, Func<bool> func)
        {
            _execute = p => action();
            _canExecute = func;
        }

        public void OnCanExecuteChanged(object sender, EventArgs e)
        {
            EventHandler handler = CanExecuteChanged;
            if(handler!=null)
            {
                handler(sender, e);
            }
        }

        public bool CanExecute(object parameter)
        {
            if(_canExecute!=null)
            {
                return _canExecute();
            }
            else
            {
                return true;
            }
        }

        public void Execute(object parameter)
        {
            if(CanExecute(parameter))
                _execute(parameter);
        }
    }
}
