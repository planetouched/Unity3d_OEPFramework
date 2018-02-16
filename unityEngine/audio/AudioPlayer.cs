using System;
using System.Collections.Generic;
using OEPFramework.common.future.utils.threadsafe;
using OEPFramework.unityEngine.audio.future;
using OEPFramework.unityEngine._base;
using UnityEngine;

namespace OEPFramework.unityEngine.audio
{
    public class AudioPlayer : DroppableItemBase
    {
        public static readonly List<AudioPlayer> audioPlayers = new List<AudioPlayer>();
        private static readonly Dictionary<int, bool> muteSettings = new Dictionary<int, bool>();
        private static readonly Dictionary<int, float> volumeSettings = new Dictionary<int, float>();

        public class AudioSourceData
        {
            public AudioSource source;
            public int layer;
            public PlayAudioFuture current;
        }

        private readonly GameObject sourceGroup;
        private readonly Dictionary<string, AudioSourceData> audioSourceData = new Dictionary<string, AudioSourceData>();
        readonly FutureWatcher futureWatcher = new FutureWatcher();

        public AudioPlayer(string sourceGroupId, Transform parent = null)
        {
            sourceGroup = new GameObject(sourceGroupId);
            sourceGroup.transform.SetParent(parent);
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
        }

        static float GetVolume(int layer)
        {
            if (volumeSettings.ContainsKey(layer))
                return volumeSettings[layer];

            return 0.5f;
        }

        static bool IsMuted(int layer)
        {
            return muteSettings.ContainsKey(layer) && muteSettings[layer];
        }

        public AudioSource AddNewSimpleSource(string key, int layer = 0)
        {
            if (dropped)
                throw new Exception("Dropped");

            var go = new GameObject(key);
            go.transform.SetParent(sourceGroup.transform);
            var source = go.AddComponent<AudioSource>();
            source.mute = IsMuted(layer);
            source.volume = GetVolume(layer);
            audioSourceData.Add(key, new AudioSourceData { layer = layer, source = source });
            return source;
        }

        public AudioSource AddSource(string key, AudioSource source, int layer = 0)
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

            audioSourceData.Remove(key);
        }

        public PlayAudioFuture Play(string sourceKey, AudioClip[] clips)
        {
            if (clips == null) return null;
            return InnerPlay(sourceKey, null, clips);
        }

        public PlayAudioFuture Play(string sourceKey, AudioClip clip)
        {
            if (clip == null) return null;
            return InnerPlay(sourceKey, clip, null);
        }

        public PlayAudioFuture InnerPlay(string sourceKey, AudioClip clip, AudioClip[] clips)
        {
            if (dropped)
                throw new Exception("Dropped");

            var sourceData = audioSourceData[sourceKey];

            if (clip == null && clips == null && sourceData.source.clip == null)
                return null;

            if (sourceData.current != null)
                sourceData.current.Cancel();

            if (clip != null)
                sourceData.current = new PlayAudioFuture(sourceData.source, clip);
            else
                sourceData.current = new PlayAudioFuture(sourceData.source, clips);

            sourceData.current = clip != null ? new PlayAudioFuture(sourceData.source, clip) : new PlayAudioFuture(sourceData.source, clips);
            sourceData.current.AddListener(f => { sourceData.current = null; });
            sourceData.current.Run();
            futureWatcher.AddFuture(sourceData.current);
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

        public PlayAudioFuture PlayOnFreeSource(AudioClip clip = null)
        {
            string free = GetFreeSource();
            if (free != null)
                return Play(free, clip);

            return null;
        }

        public PlayAudioFuture PlayOnFreeSource(AudioClip [] clips)
        {
            string free = GetFreeSource();
            if (free != null)
                return Play(free, clips);

            return null;
        }

        public override void Drop()
        {
            if (dropped) return;

            futureWatcher.CancelFutures();
            audioSourceData.Clear();
            UnityEngine.Object.Destroy(sourceGroup);
            audioPlayers.Remove(this);
            base.Drop();
        }
    }
}
