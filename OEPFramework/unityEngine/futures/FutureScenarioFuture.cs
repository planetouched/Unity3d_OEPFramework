using OEPFramework.futures;
using OEPFramework.futures.utils;

namespace OEPFramework.unityEngine.futures
{
    public class FutureScenarioFuture : Future
    {
        private readonly FutureScenario _futureScenario;

        public FutureScenarioFuture(FutureScenario futureScenario)
        {
            _futureScenario = futureScenario;
        }
        protected override void OnRun()
        {
            if (_futureScenario.isEmpty)
            {
                Complete();
                return;
            }

            _futureScenario.onComplete += result =>
            {
                if (!result)
                    Complete();
                else
                    Cancel();
            };

            _futureScenario.Run();
        }

        protected override void OnComplete()
        {
            if (isCancelled)
                _futureScenario.Cancel();
        }
    }
}
