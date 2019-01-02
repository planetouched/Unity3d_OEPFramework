using System;
using System.Collections.Generic;
using game.audio.futures;
using OEPFramework.unityEngine;
using OEPFramework.unityEngine._base;
using UnityEngine;
using Object = UnityEngine.Object;

namespace game.audio
{
    public class AudioPlayer : DroppableItemBase
    {
        public static readonly List<AudioPlayer> audioPlayers = new List<AudioPlayer>();
        private static readonly Dictionary<int, bool> muteSettings = new Dictionary<int, bool>();
        private static readonly Dictionary<int, float> volumeSettings = new Dictionary<int, float>();

        public static string MUTE = GEvent.GetUniqueCategory();
        public static string SET_VOLUME = GEvent.GetUniqueCategory();

        public class AudioSourceData
        {
            public AudioSource source;
            public int layer;
            public AudioFutureBase current;
        }

        public GameObject sourceGroup { get; private set; }
        private readonly Dictionary<string, AudioSourceData> audioSourceData = new Dictionary<string, AudioSourceData>();

        public AudioPlayer(string sourceGroupId, Transform parent = null)
        {
            sourceGroup = new GameObject(sourceGroupId);
            sourceGroup.transform.SetParent(parent,false);
            audioPlayers.Add(this);
        }

        public static void Mute(int layer, bool mute)
        {
            if (muteSettings.ContainsKey(layer))
                muteSettings[layer] = mute;
            else
                muteSettings.Add(layer, mute);

            foreach (var audioPlayer in audioPlayers)
            {
                foreach (var data in audioPlayer.audioSourceData.Values)
                {
                    if (data.layer == layer)
                        data.source.mute = mute;
                }
            }

            GEvent.Call(MUTE, new KeyValuePair<int, bool>(layer, mute));
        }

        public static void SetVolume(int layer, float volume)
        {
            if (volumeSettings.ContainsKey(layer))
                volumeSettings[layer] = volume;
            else
                volumeSettings.Add(layer, volume);

            foreach (var audioPlayer in audioPlayers)
            {
                foreach (var data in audioPlayer.audioSourceData.Values)
                {
                    if (data.layer == layer)
                        data.source.volume = volume;
                }
            }

            GEvent.Call(SET_VOLUME, new KeyValuePair<int, float>(layer, volume));
        }

        public static float GetVolume(int layer)
        {
            if (volumeSettings.ContainsKey(layer))
                return volumeSettings[layer];

            return 0.5f;
        }

        public static bool IsMuted(int layer)
        {
            return muteSettings.ContainsKey(layer) && muteSettings[layer];
        }

        public AudioSource AddNewSimpleSource(string key, int layer)
        {
            if (dropped)
                throw new Exception("Dropped");

            var go = new GameObject(key);
            go.transform.SetParent(sourceGroup.transform, false);
            var source = go.AddComponent<AudioSource>();
            source.mute = IsMuted(layer);
            source.volume = GetVolume(layer);
            audioSourceData.Add(key, new AudioSourceData { layer = layer, source = source });
            return source;
        }

        public AudioSource AddSource(string key, AudioSource source, int layer)
        {
            if (dropped)
                throw new Exception("Dropped");

            source.mute = IsMuted(layer);
            source.volume = GetVolume(layer);

            audioSourceData.Add(key, new AudioSourceData { layer = layer, source = source });
            return source;
        }

        public AudioSourceData GetSource(string key)
        {
            return audioSourceData[key];
        }

        public void RemoveSource(string key)
        {
            if (dropped)
                throw new Exception("Dropped");

            var data = audioSourceData[key];
            if (data.current != null)
                data.current.Cancel();

            Object.Destroy(data.source.gameObject);

            audioSourceData.Remove(key);
        }

        public void RemoveAllSources()
        {
            foreach (var key in new List<string>(audioSourceData.Keys))
            {
                RemoveSource(key);
            }

            audioSourceData.Clear();
        }

        public AudioFutureBase Play(string sourceKey, AudioClip[] clips, bool start = true)
        {
            if (clips == null) return null;
            return InnerPlay(sourceKey, null, clips, start);
        }

        public AudioFutureBase Play(string sourceKey, AudioClip clip, bool start = true)
        {
            if (clip == null) return null;
            return InnerPlay(sourceKey, clip, null, start);
        }

        private AudioFutureBase InnerPlay(string sourceKey, AudioClip clip, AudioClip[] clips, bool start)
        {
            if (dropped)
                throw new Exception("Dropped");

            var sourceData = audioSourceData[sourceKey];

            if (clip == null && clips == null && sourceData.source.clip == null)
                return null;

            if (sourceData.current != null)
                sourceData.current.Cancel();

            sourceData.current = clip != null ? new AudioFuture(sourceData.source, clip) : (AudioFutureBase)new SequenceAudioFuture(sourceData.source, clips);
            sourceData.current.AddListener(f => { sourceData.current = null; });

            if (start)
            {
                sourceData.current.Run();
            }

            return sourceData.current;
        }

        public string GetFreeSource()
        {
            foreach (var pair in audioSourceData)
            {
                if (pair.Value.current == null)
                    return pair.Key;
            }
            return null;
        }

        public AudioFutureBase PlayOnFreeSource(AudioClip clip = null)
        {
            string free = GetFreeSource();
            if (free != null)
                return Play(free, clip);

            return null;
        }

        public AudioFutureBase PlayOnFreeSource(AudioClip [] clips)
        {
            string free = GetFreeSource();
            if (free != null)
                return Play(free, clips);

            return null;
        }

        public override void Drop()
        {
            if (dropped) return;

            RemoveAllSources();
            Object.Destroy(sourceGroup);
            audioPlayers.Remove(this);
            base.Drop();
        }
    }
}
