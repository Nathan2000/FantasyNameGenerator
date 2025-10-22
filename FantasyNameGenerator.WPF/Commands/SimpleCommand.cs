using System.Windows.Input;

namespace FantasyNameGenerator.WPF.Commands
{
    class SimpleCommand : ICommand
    {
        public Predicate<object?> CanExecuteMethod { get; set; }
        public Action<object?> ExecuteMethod { get; set; }

        private event EventHandler? CanExecuteChangedInternal;
        public event EventHandler? CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
                CanExecuteChangedInternal += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
                CanExecuteChangedInternal -= value;
            }
        }

        public bool CanExecute(object? parameter)
        {
            if (CanExecuteMethod != null)
            {
                return CanExecuteMethod(parameter);
            }
            return true;
        }

        public void OnCanExecuteChanged()
        {
            CanExecuteChangedInternal?.Invoke(this, EventArgs.Empty);
        }

        public void Execute(object? parameter)
        {
            ExecuteMethod?.Invoke(parameter);
        }

        public SimpleCommand() : this(obj => { }, obj => true) { }

        public SimpleCommand(Action<object?> executeMethod) : this(executeMethod, obj => true) { }

        public SimpleCommand(Action<object?> executeMethod, Predicate<object?> canExecuteMethod)
        {
            CanExecuteMethod = canExecuteMethod;
            ExecuteMethod = executeMethod;
        }
    }
}
