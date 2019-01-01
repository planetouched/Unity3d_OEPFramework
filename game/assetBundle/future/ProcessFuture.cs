using System;
using OEPFramework.unityEngine.behaviour;
using OEPFramework.unityEngine.loop;

namespace Assets.game.assetBundle.future
{
    public class ProcessFuture : FutureBehaviour, IProcess
    {
        public float loadingProgress { get { return process.loadingProgress; } }
        public float unpackProgress { get { return process.unpackProgress; } }
        public Action<IProcess> onProcessComplete { get; set; }
        public bool isComplete { get; private set; }

        private readonly IProcess process;

        public ProcessFuture(IProcess process)
        {
            this.process = process;
            LoopOn(Loops.UPDATE, Update);
        }

        private void Update()
        {
            if (process.isComplete)
                Complete();
        }

        protected override void OnRun()
        {
            Play();
        }

        protected override void OnComplete()
        {
            if (isDone)
            {
                isComplete = true;
                if (onProcessComplete != null)
                    onProcessComplete(this);
                onProcessComplete = null;
            }
        }
    }
}
