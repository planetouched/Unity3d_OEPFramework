using System.Collections.Generic;
using Assets.OEPFramework.future;
using Assets.OEPFramework.unityEngine.utils;
using UnityEngine;

namespace Assets.game.audio.future
{
    public class SequenceAudioFuture : AudioFutureBase
    {
        private IFuture coroutine;
        private readonly AudioClip[] clips;

        public SequenceAudioFuture(AudioSource audioSource, AudioClip[] clips)
        {
            this.audioSource = audioSource;
            this.clips = clips;
        }

        protected override void OnRun()
        {
            if (audioSource == null || clips == null || clips.Length == 0)
            {
                Cancel();
                return;
            }

            coroutine = FutureUtils.Coroutine(C0).Run().AddListener(f =>
            {
                if (f.isDone)
                    Complete();
            });
        }

        private IEnumerator<IFuture> C0()
        {
            foreach (var clip in clips)
            {
                yield return new AudioFuture(audioSource, clip).Run();
            }
        }

        protected override void OnComplete()
        {
            if (isCancelled)
                coroutine.Cancel();
        }
    }
}
