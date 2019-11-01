namespace OEPCommon.Pool
{
    interface IPooled
    {
        void ToInitialState();
        void Release();
        int GetHashCode();
    }
}
