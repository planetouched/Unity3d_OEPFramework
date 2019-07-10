using Basement.OEPFramework.UnityEngine.Behaviour;
using UnityEngine;

namespace Basement.Game.Audio.Futures
{
    public abstract class AudioFutureBase : FutureBehaviour
    {
        public AudioSource audioSource { get; protected set; }
    }
}
