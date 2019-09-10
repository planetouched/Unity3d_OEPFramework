using System.Collections.Generic;
using Basement.OEPFramework.Futures;
using Basement.OEPFramework.UnityEngine.Util;
using UnityEngine;

namespace Game.Audio.Futures
{
    public class SequenceAudioFuture : AudioFutureBase
    {
        private IFuture _coroutine;
        private readonly AudioClip[] _clips;

        public SequenceAudioFuture(AudioSource audioSource, AudioClip[] clips)
        {
            this.audioSource = audioSource;
            _clips = clips;
        }

        protected override void OnRun()
        {
            if (audioSource == null || _clips == null || _clips.Length == 0)
            {
                Cancel();
                return;
            }

            _coroutine = FutureUtils.Coroutine(C0).Run().AddListener(f =>
            {
                if (f.isDone)
                    Complete();
            });
        }

        private IEnumerator<IFuture> C0()
        {
            foreach (var clip in _clips)
            {
                yield return new AudioFuture(audioSource, clip).Run();
            }
        }

        protected override void OnComplete()
        {
            if (isCancelled)
                _coroutine.Cancel();
        }
    }
}
