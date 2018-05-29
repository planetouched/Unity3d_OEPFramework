using System;

namespace Assets.OEPFramework.unityEngine.behaviour
{
    public interface ILoopable
    {
        void LoopOn(int loopType, Action action);
        void LoopOff(int loopType);
        /*
        void SetIndexToLast(int loopType);
        void SwapWith(LoopBehaviour target, int loopType);
        void SetIndexToFirst(int loopType);*/
    }
}
