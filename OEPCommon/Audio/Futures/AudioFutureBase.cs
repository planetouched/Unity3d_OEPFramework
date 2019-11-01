using Basement.OEPFramework.UnityEngine.Behaviour;
using UnityEngine;

namespace OEPCommon.Audio.Futures
{
    public abstract class AudioFutureBase : FutureBehaviour
    {
        public AudioSource audioSource { get; protected set; }
    }
}
