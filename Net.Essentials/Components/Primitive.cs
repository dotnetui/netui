namespace Net.Essentials
{
    public class Primitive<T>
    {
        public Primitive() { }
        public Primitive(T value)
        {
            Value = value;
        }

        public T Value { get; set; }

        public static implicit operator T(Primitive<T> p) => p == default ? default : p.Value;
        public static implicit operator Primitive<T>(T v) => new Primitive<T>(v);
    }
}
