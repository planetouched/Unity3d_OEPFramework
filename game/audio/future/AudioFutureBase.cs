using OEPFramework.unityEngine.behaviour;
using UnityEngine;

namespace Assets.game.audio.future
{
    public abstract class AudioFutureBase : FutureBehaviour
    {
        public AudioSource audioSource { get; protected set; }
    }
}
