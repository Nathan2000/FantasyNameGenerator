namespace Gui
{
    using System;
    using System.Windows.Input;

    public class SimpleCommand : ICommand
    {
        public Predicate<object> CanExecuteMethod { get; set; }
        public Action<object> ExecuteMethod { get; set; }

        public bool CanExecute(object parameter)
        {
            if (this.CanExecuteMethod != null)
            {
                return this.CanExecuteMethod(parameter);
            }

            return true;
        }

        private event EventHandler CanExecuteChangedInternal;

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
                this.CanExecuteChangedInternal += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;
                this.CanExecuteChangedInternal -= value;
            }
        }

        public void OnCanExecuteChanged()
        {
            EventHandler handler = this.CanExecuteChangedInternal;
            if (handler != null)
            {
                handler.Invoke(this, EventArgs.Empty);
            }
        }

        public void Execute(object parameter)
        {
            if (this.ExecuteMethod != null)
            {
                this.ExecuteMethod(parameter);
            }
        }
    }
}