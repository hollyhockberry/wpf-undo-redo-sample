using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

        protected void SetProperty<T>(ref T target, T value, [CallerMemberName] string? propertyName = null)
        {
            if (!Equals(target, value))
            {
                target = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private bool Active { get; set; } = true;

        private Stack<Memento> Mementos { get; } = new Stack<Memento>();

        private Dictionary<object, Dictionary<string, object>> Values { get; } = new Dictionary<object, Dictionary<string, object>>();

        public void Register(object target)
        {
            if (target is INotifyPropertyChanged nc)
            {
                Values[nc] = nc.GetType().GetProperties()
                    .Where(p => p.CanWrite)
                    .ToDictionary(p => p.Name, p => p.GetValue(nc)!);

                nc.PropertyChanged += (s, e) =>
                {
                    if (!Active) return;

                    var target = (object)s!;
                    var name = (string)e.PropertyName!;
                    Mementos.Push(new Memento(target, name, Values[target][name]!));
                    Values[target][name] = target.GetType().GetProperty(name)?.GetValue(nc)!;

                    CanUndo = Mementos.Count > 0;
                };
            }
        }

        public void Undo()
        {
            if (!CanUndo) return;

            try
            {
                Active = false;
                var memento = Mementos.Pop();
                Values[memento.Target][memento.PropertyName] = memento.Data;

                var property = memento.Target.GetType().GetProperty(memento.PropertyName);
                property?.SetValue(memento.Target, memento.Data);
            }
            finally
            {
                CanUndo = Mementos.Count > 0;
                Active = true;
            }
        }
    }
}
