using System.ComponentModel;

namespace undo_sample
{
    internal class ValueViewModel<T> : INotifyPropertyChanged
    {
        private T? _Value;

        public T? Value
        {
            get => _Value;
            set
            {
                if (!Equals(_Value, value))
                {
                    Caretaker.Instance.Add(new Memento(this, nameof(Value), _Value));
                    _Value = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public ValueViewModel(T value) => _Value = value;

        public ValueViewModel() { }
    }
}
