using Basement.OEPFramework.UnityEngine.Loop;
using UnityEngine;

namespace Game.Audio.Futures
{
    public class AudioFuture : AudioFutureBase
    {
        private readonly AudioClip _clip;
        
        public AudioFuture(AudioSource audioSource, AudioClip clip)
        {
            this.audioSource = audioSource;
            _clip = clip;
        }

        protected override void OnRun()
        {
            if (audioSource == null || _clip == null)
            {
                Cancel();
                return;
            }

            if (_clip != null)
                audioSource.clip = _clip;

            audioSource.Play();
            LoopOn(Loops.UPDATE, Update);
            Play();
        }

        private void Update()
        {
            if (audioSource == null)
            {
                Cancel();
                return;
            }

            if (!audioSource.loop && !audioSource.isPlaying)
                Complete();
        }

        protected override void OnComplete()
        {
            if (audioSource != null)
            {
                audioSource.Stop();

                if (_clip != null)
                    audioSource.clip = null;
            }
        }
    }
}
