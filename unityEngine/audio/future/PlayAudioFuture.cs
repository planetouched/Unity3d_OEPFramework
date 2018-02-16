using OEPFramework.common.future.utils;
using OEPFramework.unityEngine.behaviour;
using OEPFramework.unityEngine.loop;
using UnityEngine;

namespace OEPFramework.unityEngine.audio.future
{
    public class PlayAudioFuture : FutureBehaviour
    {
        public AudioSource audioSource { get; private set; }
        private readonly AudioClip clip;
        private readonly FutureScenario scenario;
        
        public PlayAudioFuture(AudioSource audioSource, AudioClip clip)
        {
            this.audioSource = audioSource;
            this.clip = clip;
        }

        public PlayAudioFuture(AudioSource audioSource, AudioClip[] clips)
        {
            this.audioSource = audioSource;
            scenario = new FutureScenario();
            foreach (var c in clips)
            {
                scenario.AddFuture(new PlayAudioFuture(audioSource, c));
                scenario.Next();
            }
        }

        protected override void OnRun()
        {
            if (scenario != null)
            {
                scenario.onComplete += cancel =>
                {
                    if (!cancel)
                        Complete();
                };
                scenario.Run();
            }
            else
            {
                if (clip != null)
                    audioSource.clip = clip;

                audioSource.Play();

                LoopOn(Loops.UPDATE, Update);
                Play();
            }
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
            if (dropped) return;

            if (scenario == null)
            {
                audioSource.Stop();

                if (clip != null)
                    audioSource.clip = null;
            }
            else
            {
                if (isCancelled)
                    scenario.Cancel();
            }
        }
    }
}
