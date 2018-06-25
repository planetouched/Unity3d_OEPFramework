namespace Assets.common.pool
{
    interface IPooled
    {
        void ToInitialState();
        void Release();
        int GetHashCode();
    }
}
