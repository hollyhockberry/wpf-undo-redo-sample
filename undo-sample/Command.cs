using System;
using System.Windows.Input;

namespace undo_sample
{
    internal class Command : ICommand
    {
        private readonly Action<object?> execute;

        private readonly Func<object?, bool>? canExecute;

        public event EventHandler? CanExecuteChanged;

        public Command(Action<object?> execute, Func<object?, bool>? canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public void RaiseCanExecute() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        public bool CanExecute(object? parameter) => canExecute is null ? true : canExecute(parameter);

        public void Execute(object? parameter) => execute(parameter);
    }
}
