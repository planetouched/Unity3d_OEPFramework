using OEPFramework.unityEngine.loop;
using UnityEngine;

namespace Assets.game.audio.futures
{
    public class AudioFuture : AudioFutureBase
    {
        private readonly AudioClip clip;
        
        public AudioFuture(AudioSource audioSource, AudioClip clip)
        {
            this.audioSource = audioSource;
            this.clip = clip;
        }

        protected override void OnRun()
        {
            if (audioSource == null || clip == null)
            {
                Cancel();
                return;
            }

            if (clip != null)
                audioSource.clip = clip;

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

                if (clip != null)
                    audioSource.clip = null;
            }
        }
    }
}
