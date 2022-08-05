namespace undo_sample
{
    internal class Memento
    {
        public object Target { get; }

        public string PropertyName { get; }

        public object Data { get; }

        public Memento(object target, string propertyName, object data)
        {
            Target = target;
            PropertyName = propertyName;
            Data = data;
        }
    }
}
