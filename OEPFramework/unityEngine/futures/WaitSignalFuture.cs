﻿using OEPFramework.futures;

namespace OEPFramework.unityEngine.futures
{
    public class WaitSignalFuture : Future
    {
        private readonly string _signalCategory;
        
        public WaitSignalFuture(string signalCategory)
        {
            _signalCategory = signalCategory;
        }

        void SignalComplete(object obj)
        {
            Complete();
        }

        protected override void OnRun()
        {
            GEvent.Attach(_signalCategory, SignalComplete, null);
        }

        protected override void OnComplete()
        {
            GEvent.Detach(_signalCategory, SignalComplete);
        }
    }
}
