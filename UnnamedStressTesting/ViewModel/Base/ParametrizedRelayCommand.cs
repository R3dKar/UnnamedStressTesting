using System;
using System.Windows.Input;

namespace UnnamedStressTesting
{
    public class ParametrizedRelayCommand : ICommand
    {
        private readonly Action<object> action;

        public event EventHandler CanExecuteChanged = (sender, e) => { };

        public ParametrizedRelayCommand(Action<object> action)
        {
            this.action = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this.action(parameter);
        }
    }
}
