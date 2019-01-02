using System;
using System.Collections.Generic;

namespace OEPFramework.futures.coroutine
{
    public class CoroutineFuture<T1, T2> : CoroutineFutureBase
    {
        private Func<T1, T2, IEnumerator<IFuture>> func;

        private readonly T1 param1;
        private readonly T2 param2;

        public CoroutineFuture(Func<T1, T2, IEnumerator<IFuture>> func, T1 param1, T2 param2)
        {
            this.param1 = param1;
            this.param2 = param2;
            this.func = func;
        }

        protected override void OnRun()
        {
            enumerator = func(param1, param2);
            Next(null);
        }

        protected override void OnComplete()
        {
            base.OnComplete();
            func = null;
        }
    }
}
