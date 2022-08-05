using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace undo_sample
{
    internal class Caretaker : INotifyPropertyChanged
    {
        public static Caretaker Instance { get; } = new Caretaker();

        private Caretaker() { }

        public event PropertyChangedEventHandler? PropertyChanged;

        private bool _CanUndo;

        public bool CanUndo
        {
            get => _CanUndo;
            set => SetProperty(ref _CanUndo, value);
        }

        private bool _CanRedo;

        public bool CanRedo
        {
            get => _CanRedo;
            set => SetProperty(ref _CanRedo, value);
        }

        protected void SetProperty<T>(ref T target, T value, [CallerMemberName] string? propertyName = null)
        {
            if (!Equals(target, value))
            {
                target = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private bool Active { get; set; } = true;

        private Stack<Memento> UndoMementos { get; } = new Stack<Memento>();

        private Stack<Memento> RedoMementos { get; } = new Stack<Memento>();

        public void Add(Memento memento)
        {
            if (Active)
            {
                UndoMementos.Push(memento);
                RedoMementos.Clear();
                CanUndo = UndoMementos.Count > 0;
                CanRedo = RedoMementos.Count > 0;
            }
        }

        public void Undo()
        {
            if (!CanUndo) return;

            try
            {
                Active = false;
                var memento = UndoMementos.Pop();
                var property = memento.Target.GetType().GetProperty(memento.PropertyName);
                RedoMementos.Push(new Memento(memento.Target, memento.PropertyName, property?.GetValue(memento.Target)!));
                property?.SetValue(memento.Target, memento.Data);
            }
            finally
            {
                CanUndo = UndoMementos.Count > 0;
                CanRedo = RedoMementos.Count > 0;
                Active = true;
            }
        }

        public void Redo()
        {
            if (!CanRedo) return;

            try
            {
                Active = false;
                var memento = RedoMementos.Pop();
                var property = memento.Target.GetType().GetProperty(memento.PropertyName);
                UndoMementos.Push(new Memento(memento.Target, memento.PropertyName, property?.GetValue(memento.Target)!));
                property?.SetValue(memento.Target, memento.Data);
            }
            finally
            {
                CanUndo = UndoMementos.Count > 0;
                CanRedo = RedoMementos.Count > 0;
                Active = true;
            }
        }
    }
}
