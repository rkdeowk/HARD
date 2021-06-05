using System;
using System.Windows.Input;

namespace HARD.ViewModel
{
    internal class DelegateComand : ICommand
    {
        private readonly Action execute;
        private readonly Func<bool> canExecute;

        public DelegateComand(Action execute) : this(execute, null)
        {

        }

        public DelegateComand(Action execute, Func<bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }
        
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            if (canExecute == null)
            {
                return true;
            }
            return canExecute();
        }

        public void Execute(object parameter)
        {
            execute();
        }

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }
    }
}