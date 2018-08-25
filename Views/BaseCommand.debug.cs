using System;
using System.Windows.Input;

namespace IngameScript.Views
{
    public class BaseCommand : ICommand
    {
        protected readonly Action _TargetExecuteMethod;
        protected readonly Func<bool> _TargetCanExecuteMethod;

        public BaseCommand(Action executeMethod, Func<bool> canExecuteMethod = null)
        {
            _TargetExecuteMethod = executeMethod;
            _TargetCanExecuteMethod = canExecuteMethod;
        }

        public void RaiseCanExecuteChanged() => CanExecuteChanged(this, EventArgs.Empty);

        bool ICommand.CanExecute(object parameter)
        {
            if (_TargetCanExecuteMethod != null)
            {
                return _TargetCanExecuteMethod();
            }

            return _TargetExecuteMethod != null;
        }

        public event EventHandler CanExecuteChanged = delegate { };

        void ICommand.Execute(object parameter) => _TargetExecuteMethod?.Invoke();
    }
}
