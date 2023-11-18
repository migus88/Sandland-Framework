namespace Sandland.Core.Interfaces
{
    public interface IPool<T>
    {
        T GetObject();
        void ReturnObject(T obj);
    }
}