﻿using System.ComponentModel;
using System.Runtime.CompilerServices;

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
                target = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}