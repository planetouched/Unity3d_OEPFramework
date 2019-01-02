using OEPFramework.unityEngine.behaviour;
using UnityEngine;

namespace game.audio.futures
{
    public abstract class AudioFutureBase : FutureBehaviour
    {
        public AudioSource audioSource { get; protected set; }
    }
}
