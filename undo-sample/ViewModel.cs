﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace undo_sample
{
    internal class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private string _Text = "SampleText1";

        public string Text
        {
            get => _Text;
            set => SetProperty(ref _Text, value);
        }

        private int _Integer = 100;

        public int Integer
        {
            get => _Integer;
            set => SetProperty(ref _Integer, value);
        }

        private bool _Boolean;

        public bool Boolean
        {
            get => _Boolean;
            set => SetProperty(ref _Boolean, value);
        }

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