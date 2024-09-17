namespace Rhythm
{
    public interface IObjectPoolProvider<T>
    {
        T Create();
        void Destroy(T obj);
        void Clear();
    }
}