namespace logic.core.common
{
    public interface IParent<T>
    {
        void SetParent(T parent);
        T GetParent();
    }
}