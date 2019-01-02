using Assets.OEPFramework.futures;
using Assets.OEPFramework.futures.utils;

namespace Assets.OEPFramework.unityEngine.futures
{
    public class FutureScenarioFuture : Future
    {
        private readonly FutureScenario futureScenario;

        public FutureScenarioFuture(FutureScenario futureScenario)
        {
            this.futureScenario = futureScenario;
        }
        protected override void OnRun()
        {
            if (futureScenario.isEmpty)
            {
                Complete();
                return;
            }

            futureScenario.onComplete += result =>
            {
                if (!result)
                    Complete();
                else
                    Cancel();
            };

            futureScenario.Run();
        }

        protected override void OnComplete()
        {
            if (isCancelled)
                futureScenario.Cancel();
        }
    }
}
