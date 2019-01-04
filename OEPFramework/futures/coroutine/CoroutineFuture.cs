using System;
using System.Collections.Generic;

namespace OEPFramework.futures.coroutine
{
    public class CoroutineFuture : CoroutineFutureBase
    {
        private Func<IEnumerator<IFuture>> _func;
        
        public CoroutineFuture(Func<IEnumerator<IFuture>> func)
        {
            _func = func;
        }
        
        protected override void OnRun()
        {
            enumerator = _func();
            Next(null);
        }

        protected override void OnComplete()
        {
            base.OnComplete();
            _func = null;
        }
    }
}
