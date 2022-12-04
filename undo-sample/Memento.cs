namespace undo_sample
{
    internal class Memento
    {
        readonly object Target;

        readonly string PropertyName;

        readonly object Data;

        public Memento(object target, string propertyName, object data)
        {
            Target = target;
            PropertyName = propertyName;
            Data = data;
        }

        public Memento Apply()
        {
            var property = Target.GetType().GetProperty(PropertyName);
            var memento = new Memento(Target, PropertyName, property?.GetValue(Target)!);
            property?.SetValue(Target, Data);
            return memento;
        }
    }
}
