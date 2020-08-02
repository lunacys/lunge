namespace lunge.Library.Resources
{
    public interface IResourceManager
    {
        void Insert<T>(T provider);
        bool FetchOrAdd<T>(out T obj);
        bool TryFetch<T>(out T obj);
        void Delete<T>();
        void Clear();
    }
}