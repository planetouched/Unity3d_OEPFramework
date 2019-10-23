using System;
using System.Collections.Generic;
using Basement.OEPFramework.Futures;
using Basement.OEPFramework.Futures.Util.ThreadSafe;
using Game.AssetBundles.Futures;

namespace Game.AssetBundles
{
    public class ProcessChecker : IProcess
    {
        public float loadingProgress => CalcLoadingProgress();
        public float unpackProgress => CalcUnpackProgress();
        public event Action<IProcess> onProcessComplete;
        public bool isComplete { get; private set; }

        private readonly FutureWatcher _watcher = new FutureWatcher();
        private readonly List<IProcess> _processes = new List<IProcess>();
        private bool _loading;
        
        public void Add(IProcess process)
        {
            _processes.Add(process);
            _watcher.AddFuture(new ProcessFuture(process).Run());
        }

        public void Add(string resource, Unloader unloader = null, bool async = true)
        {
            isComplete = false;

            unloader?.Add(resource);

            var compositeFuture = AssetBundleManager.Load(resource, out var processList, async);
            
            foreach (var loadFuture in processList)
            {
                _processes.Add(loadFuture);
            }

            _watcher.AddFuture(compositeFuture);

            if (_loading)
            {
                Subscribe(compositeFuture);
            }
        }

        public void Load()
        {
            if (CallCompleteIfEmpty() || _loading)
                return;

            _loading = true;

            foreach (var future in _watcher.futures)
            {
                Subscribe(future);
            }
        }
        
        private void Subscribe(IFuture target)
        {
            target.AddListener(future =>
            {
                if (_watcher.futuresCount == 0 && future.isDone)
                {
                    _processes.Clear();
                    isComplete = true;
                    _loading = false;

                    onProcessComplete?.Invoke(this);
                }
            });
        }

        private bool CallCompleteIfEmpty()
        {
            if (_watcher.futuresCount == 0)
            {
                isComplete = true;
                onProcessComplete?.Invoke(this);

                return true;
            }
            return false;
        }

        private float CalcUnpackProgress()
        {
            if (isComplete) return 1;
            if (_processes.Count == 0)
                return 0;

            float value = 0;
            
            foreach (var progress in _processes)
            {
                value += progress.unpackProgress;
            }

            return value / _processes.Count;
        }
        
        private float CalcLoadingProgress()
        {
            if (isComplete) return 1;
            
            if (_processes.Count == 0)
            {
                return 0;
            }

            float value = 0;
            
            foreach (var progress in _processes)
            {
                value += progress.loadingProgress;
            }

            return value / _processes.Count;
        }
        
        public void Drop()
        {
            _processes.Clear();
            _watcher.CancelFutures();
            onProcessComplete = null;
        }
    }
}
