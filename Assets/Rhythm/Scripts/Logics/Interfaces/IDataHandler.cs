namespace Rhythm
{
    public interface IDataHandler<T>
    {
        T this[string id] { get; set; }
    }
}