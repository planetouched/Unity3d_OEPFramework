using System;
using System.Collections.Generic;

namespace Assets.OEPFramework.future.coroutine
{
    public class CoroutineFuture : CoroutineFutureBase
    {
        private Func<IEnumerator<IFuture>> func;
        
        public CoroutineFuture(Func<IEnumerator<IFuture>> func)
        {
            this.func = func;
        }
        
        protected override void OnRun()
        {
            enumerator = func();
            Next(null);
        }

        protected override void OnComplete()
        {
            base.OnComplete();
            func = null;
        }
    }
}
