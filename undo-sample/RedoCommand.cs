using System;
using System.Windows.Input;

namespace undo_sample
{
    internal class RedoCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public RedoCommand()
        {
            Caretaker.Instance.PropertyChanged += (s, e) => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool CanExecute(object? parameter) => Caretaker.Instance.CanRedo;

        public void Execute(object? parameter) => Caretaker.Instance.Redo();
    }
}
