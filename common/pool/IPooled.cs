﻿namespace OEPFramework.common.pool
{
    interface IPooled
    {
        void ToInitialState();
        void Release();
        int GetHashCode();
    }
}
