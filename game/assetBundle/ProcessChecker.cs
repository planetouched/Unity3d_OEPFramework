using System;
using System.Collections.Generic;
using common.utils;
using game.assetBundle.futures;
using OEPFramework.futures;
using OEPFramework.futures.utils.threadsafe;

namespace game.assetBundle
{
    public class ProcessChecker : IProcess
    {
        public float loadingProgress => CalcLoadingProgress();
        public float unpackProgress => CalcUnpackProgress();
        public Action<IProcess> onProcessComplete { get; set; }
        public bool isComplete { get; private set; }

        private readonly AssetBundleManager _manager;
        private readonly FutureWatcher _watcher = new FutureWatcher();
        private readonly List<IProcess> _processes = new List<IProcess>();
        private bool _loading;

        public ProcessChecker()
        {
            _manager = SingletonManager.Get<AssetBundleManager>();
        }

        public void Add(IProcess process)
        {
            _processes.Add(process);
            _watcher.AddFuture(new ProcessFuture(process).Run());
        }

        public void Add(string resource, Unloader unloader = null, bool async = true)
        {
            isComplete = false;

            if (unloader != null)
                unloader.Add(resource);

            List<IProcess> processList;
            var compositeFuture = _manager.Load(resource, out processList, async);
            foreach (var loadFuture in processList)
            {
                _processes.Add(loadFuture);
            }

            _watcher.AddFuture(compositeFuture);

            if (_loading)
                Subscribe(compositeFuture);
        }

        void Subscribe(IFuture target)
        {
            target.AddListener(future =>
            {
                if (_watcher.futuresCount == 0 && future.isDone)
                {
                    _processes.Clear();
                    isComplete = true;
                    _loading = false;

                    if (onProcessComplete != null)
                        onProcessComplete(this);
                }
            });
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

        bool CallCompleteIfEmpty()
        {
            if (_watcher.futuresCount == 0)
            {
                isComplete = true;
                if (onProcessComplete != null)
                    onProcessComplete(this);

                return true;
            }
            return false;
        }

        float CalcUnpackProgress()
        {
            if (isComplete) return 1;
            if (_processes.Count == 0)
                return 0;

            float value = 0;
            foreach (var progress in _processes)
                value += progress.unpackProgress;
            
            return value / _processes.Count;
        }
        
        float CalcLoadingProgress()
        {
            if (isComplete) return 1;
            if (_processes.Count == 0)
                return 0;

            float value = 0;
            foreach (var progress in _processes)
                value += progress.loadingProgress;

            return value / _processes.Count;
        }
        
        public void Drop()
        {
            _processes.Clear();
            _watcher.CancelFutures();
        }
    }
}
