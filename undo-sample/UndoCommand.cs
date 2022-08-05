using System;
using System.Windows.Input;

namespace undo_sample
{
    internal class UndoCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public UndoCommand()
        {
            Caretaker.Instance.PropertyChanged += (s, e) => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool CanExecute(object? parameter) => Caretaker.Instance.CanUndo;

        public void Execute(object? parameter) => Caretaker.Instance.Undo();
    }
}
