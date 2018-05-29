using System;
using Assets.OEPFramework.unityEngine.behaviour;
using Assets.OEPFramework.unityEngine.loop;

namespace Assets.OEPFramework.unityEngine.future
{
    public class UpdateLoopFuture : FutureBehaviour
    {
        private Action<UpdateLoopFuture> updateAction;
        public UpdateLoopFuture(Action<UpdateLoopFuture> updateAction)
        {
            this.updateAction = updateAction;
        }
        
        protected override void OnRun()
        {
            LoopOn(Loops.UPDATE, Update);
            Play();
        }

        private void Update()
        {
            updateAction(this);
        }

        protected override void OnComplete()
        {
            Drop();
        }

        public override void Drop()
        {
            if (dropped) return;
            updateAction = null;
            base.Drop();
        }
    }
}
