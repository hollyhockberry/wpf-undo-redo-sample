using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace undo_sample
{
    internal class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private readonly Model Model = new();

        public string Text
        {
            get => Model.Text;
            set => SetProperty(ref Model.Text, value);
        }

        public int Integer
        {
            get => Model.Integer;
            set => SetProperty(ref Model.Integer, value);
        }

        public bool Boolean
        {
            get => Model.Boolean;
            set => SetProperty(ref Model.Boolean, value);
        }

        ObservableCollection<ValueViewModel<int>>? _IntegerList;

        public ObservableCollection<ValueViewModel<int>> IntegerList => _IntegerList ??= new(Enumerable.Range(0, 5).Select(i => new ValueViewModel<int>(i)));

        protected void SetProperty<T>(ref T target, T value, [CallerMemberName] string? propertyName = null)
        {
            if (!Equals(target, value))
            {
                Caretaker.Instance.Add(new Memento(this, propertyName!, target!));
                target = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public ViewModel()
        {
            Caretaker.Instance.PropertyChanged += (s, e) =>
            {
                switch (e.PropertyName ?? "")
                {
                    case "CanUndo":
                        (UndoCommand as Command)?.RaiseCanExecute();
                        break;
                    case "CanRedo":
                        (RedoCommand as Command)?.RaiseCanExecute();
                        break;
                    default:
                        break;
                }
            };
        }

        private Command? _UndoCommand;

        public ICommand UndoCommand => _UndoCommand ??= new Command(
            (_) => Caretaker.Instance.Undo(),
            (_) => Caretaker.Instance.CanUndo);

        private Command? _RedoCommand;

        public ICommand RedoCommand => _RedoCommand ??= new Command(
            (_) => Caretaker.Instance.Redo(),
            (_) => Caretaker.Instance.CanRedo);
    }
}