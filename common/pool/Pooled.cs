#if REFVIEW
using OEPFramework.utils;
#endif

namespace OEPFramework.common.pool
{
    public abstract class Pooled :
    #if REFVIEW
        ReferenceCounter,
    #endif
    IPooled
    {
        private readonly WeakRef<IObjectPool> weakPool;

        protected Pooled(IObjectPool pool)
        {
            weakPool = new WeakRef<IObjectPool>(pool);
        }

        public virtual void Release()
        {
            ToInitialState();
            if (weakPool.isAlive)
                weakPool.obj.ReturnObj(this);
        }

        public abstract void ToInitialState();
    }
}
