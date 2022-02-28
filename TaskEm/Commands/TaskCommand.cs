using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TaskEm.Commands
{
    public class TaskCommand : ICommand
    {

        Action _TargetExecuteMethod;
        Func<bool> _TargetCanExecuteMethod;
        
        public TaskCommand(Action executeMethod)
        {
            _TargetExecuteMethod = executeMethod;
        }

        public TaskCommand(Action executeMethod, Func<bool> canExecuteMethod)
        {
            _TargetExecuteMethod = executeMethod;
            _TargetCanExecuteMethod = canExecuteMethod; 
        }

        public void OnCanExecuteChanged()
        {
            CanExecuteChanged(this, EventArgs.Empty);
        }

        bool ICommand.CanExecute(object parameter)
        {
            if(_TargetCanExecuteMethod != null)
            {
                return _TargetCanExecuteMethod();
            }

            if(_TargetExecuteMethod != null)
            {
                return true;
            }

            return false;
        }

        public event EventHandler CanExecuteChanged = delegate { };

        void ICommand.Execute(object parameter)
        {
            if(_TargetExecuteMethod != null)
            {
                _TargetExecuteMethod();
            }
        }

    }

}
