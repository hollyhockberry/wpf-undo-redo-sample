using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace undo_sample
{
    internal class ObservableCollectionMemento<T> : IMemento
    {
        readonly ObservableCollection<T> Target;

        NotifyCollectionChangedAction Action;

        readonly IList Items;

        readonly int StartingIndex = -1;

        public ObservableCollectionMemento(ObservableCollection<T> target, NotifyCollectionChangedEventArgs args)
        {
            Target = target;
            Action = args.Action;

            switch (Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (args.NewItems == null) throw new Exception();
                    Items = args.NewItems;
                    StartingIndex = args.NewStartingIndex;
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (args.OldItems == null) throw new Exception();
                    Items = args.OldItems;
                    StartingIndex = args.OldStartingIndex;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public IMemento Apply()
        {
            switch (Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var _ in Enumerable.Range(0, Items?.Count ?? 0))
                    {
                        Target.RemoveAt(StartingIndex);
                    }
                    Action = NotifyCollectionChangedAction.Remove;
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var i in Items)
                    {
                        Target.Insert(StartingIndex, (T)i);
                    }
                    Action = NotifyCollectionChangedAction.Add;
                    break;
                default:
                    throw new NotImplementedException();
            }
            return this;
        }
    }
}
