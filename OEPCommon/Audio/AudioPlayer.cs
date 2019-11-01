using System;
using System.Collections.Generic;
using Basement.OEPFramework.UnityEngine;
using Basement.OEPFramework.UnityEngine._Base;
using OEPCommon.Audio.Futures;
using UnityEngine;
using Object = UnityEngine.Object;

namespace OEPCommon.Audio
{
    public class AudioPlayer : DroppableItemBase
    {
        public class AudioSourceData
        {
            public AudioSource source;
            public int layer;
            public AudioFutureBase current;
        }
        
        public static string MUTE = GEvent.GetUniqueCategory();
        public static string SET_VOLUME = GEvent.GetUniqueCategory();
        public GameObject sourceGroup { get; }
        
        private static readonly List<AudioPlayer> _audioPlayers = new List<AudioPlayer>();
        private static readonly Dictionary<int, bool> _muteSettings = new Dictionary<int, bool>();
        private static readonly Dictionary<int, float> _volumeSettings = new Dictionary<int, float>();
        private readonly Dictionary<string, AudioSourceData> _audioSourceData = new Dictionary<string, AudioSourceData>();

        public AudioPlayer(string sourceGroupId, Transform parent = null)
        {
            sourceGroup = new GameObject(sourceGroupId);
            sourceGroup.transform.SetParent(parent,false);
            _audioPlayers.Add(this);
        }

        public static void Mute(int layer, bool mute)
        {
            if (_muteSettings.ContainsKey(layer))
                _muteSettings[layer] = mute;
            else
                _muteSettings.Add(layer, mute);

            foreach (var audioPlayer in _audioPlayers)
            {
                foreach (var data in audioPlayer._audioSourceData.Values)
                {
                    if (data.layer == layer)
                        data.source.mute = mute;
                }
            }

            GEvent.Call(MUTE, new KeyValuePair<int, bool>(layer, mute));
        }

        public static void SetVolume(int layer, float volume)
        {
            if (_volumeSettings.ContainsKey(layer))
                _volumeSettings[layer] = volume;
            else
                _volumeSettings.Add(layer, volume);

            foreach (var audioPlayer in _audioPlayers)
            {
                foreach (var data in audioPlayer._audioSourceData.Values)
                {
                    if (data.layer == layer)
                        data.source.volume = volume;
                }
            }

            GEvent.Call(SET_VOLUME, new KeyValuePair<int, float>(layer, volume));
        }

        public static float GetVolume(int layer)
        {
            if (_volumeSettings.ContainsKey(layer))
                return _volumeSettings[layer];

            return 0.5f;
        }

        public static bool IsMuted(int layer)
        {
            return _muteSettings.ContainsKey(layer) && _muteSettings[layer];
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
            _audioSourceData.Add(key, new AudioSourceData { layer = layer, source = source });
            return source;
        }

        public AudioSource AddSource(string key, AudioSource source, int layer)
        {
            if (dropped)
                throw new Exception("Dropped");

            source.mute = IsMuted(layer);
            source.volume = GetVolume(layer);

            _audioSourceData.Add(key, new AudioSourceData { layer = layer, source = source });
            return source;
        }

        public AudioSourceData GetSource(string key)
        {
            return _audioSourceData[key];
        }

        public void RemoveSource(string key)
        {
            if (dropped)
                throw new Exception("Dropped");

            var data = _audioSourceData[key];
            if (data.current != null)
                data.current.Cancel();

            Object.Destroy(data.source.gameObject);

            _audioSourceData.Remove(key);
        }

        public void RemoveAllSources()
        {
            foreach (var key in new List<string>(_audioSourceData.Keys))
            {
                RemoveSource(key);
            }

            _audioSourceData.Clear();
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

            var sourceData = _audioSourceData[sourceKey];

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
            foreach (var pair in _audioSourceData)
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
            _audioPlayers.Remove(this);
            base.Drop();
        }
    }
}
