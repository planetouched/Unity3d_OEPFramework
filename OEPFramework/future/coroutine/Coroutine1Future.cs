using System;
using System.Collections.Generic;

namespace Assets.OEPFramework.future.coroutine
{
    public class CoroutineFuture<T1> : CoroutineFutureBase
    {
        private Func<T1, IEnumerator<IFuture>> func;

        private readonly T1 param1;

        public CoroutineFuture(Func<T1, IEnumerator<IFuture>> func, T1 param1)
        {
            this.param1 = param1;
            this.func = func;
        }

        protected override void OnRun()
        {
            enumerator = func(param1);
            Next(null);
        }

        protected override void OnComplete()
        {
            base.OnComplete();
            func = null;
        }
    }
}
