using System;
using System.Collections.Generic;
using Basement.OEPFramework.Futures;
using Basement.OEPFramework.Futures.Util.ThreadSafe;
using OEPCommon.AssetBundles.Futures;

namespace OEPCommon.AssetBundles
{
    public class ProcessChecker : IProcess
    {
        public float loadingProgress => CalcLoadingProgress();
        public float unpackProgress => CalcUnpackProgress();
        public event Action<IProcess> onProcessComplete;
        public bool isComplete { get; private set; }

        private readonly FutureWatcher _watcher = new FutureWatcher();
        private readonly IList<IProcess> _processes = new List<IProcess>();
        private bool _loading;
        
        private readonly IList<(string resource, bool async)> _itemsToLoad = new List<(string resource, bool async)>();
        private readonly IList<IProcess> _processesToLoad = new List<IProcess>();
        
        public void Add(IProcess process)
        {
            _processesToLoad.Add(process);
        }

        public void Add(string resource, bool async = true)
        {
            _itemsToLoad.Add((resource, async));
        }

        public void Load(Unloader unloader = null)
        {
            isComplete = false;
            
            foreach (var pair in _itemsToLoad)
            {
                var loadFuture = AssetBundleManager.Load(pair.resource, out var processList, pair.async);
                
                unloader?.Add(pair.resource);

                if (!loadFuture.isDone)
                {
                    foreach (var process in processList)
                    {
                        _processes.Add(process);
                    }

                    AttachFuture(loadFuture);
                }
            }
            
            _itemsToLoad.Clear();

            foreach (var process in _processesToLoad)
            {
                if (!process.isComplete)
                {
                    _processes.Add(process);
                    AttachFuture(new ProcessFuture(process).Run());
                }
            }
            
            _processesToLoad.Clear();

            if (_processes.Count == 0)
            {
                isComplete = true;
                onProcessComplete?.Invoke(this);
            }
        }

        private void AttachFuture(IFuture future)
        {
            _watcher.AddFuture(future);
            
            future.AddListener(f =>
            {
                if (_watcher.futuresCount == 0)
                {
                    _processes.Clear();

                    if (f.isDone)
                    {
                        isComplete = true;
                        onProcessComplete?.Invoke(this);
                    }
                }
            });
        }

        private float CalcUnpackProgress()
        {
            if (isComplete) return 1;
            
            if (_processes.Count == 0)
            {
                return 0;
            }

            float value = 0;
            
            foreach (var progress in _processes)
            {
                value += progress.unpackProgress;
            }

            return value / _processes.Count;
        }
        
        private float CalcLoadingProgress()
        {
            if (isComplete)
            {
                return 1;
            }
            
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
