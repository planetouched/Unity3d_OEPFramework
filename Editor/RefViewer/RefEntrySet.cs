#if REFVIEW
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

namespace Editor.RefViewer
{
    public class RefEntrySet
    {
        private readonly IList<RefEntry> _entries = new List<RefEntry>();
        private readonly IList<RefEntry> _toDelete = new List<RefEntry>();

        public string name { get; }
        
        private Vector2 _scrollPosition;

        public RefEntrySet(string id, bool load = true)
        {
            name = "Set " + id;
            
            if (load)
            {
                LoadEntries();
            }
            else
            {
                DeleteEntries();
            }
        }

        public void PreAddEntry()
        {
            DeleteEntries();
        }
        
        public void PostAddEntry()
        {
            SetEntries();
        }
        
        public void AddEntry(RefEntry entry)
        {
            _entries.Add(entry);
        }

        public void MemorizeAll()
        {
            foreach (var entryView in _entries)
            {
                entryView.Memorize(true);
            }
        }

        public void RemoveAllEntries()
        {
            DeleteEntries();
            _entries.Clear();
        }
        
        public void DrawEntities()
        {
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, false, false);
            
            foreach (var entryView in _entries)
            {
                entryView.Draw();

                if (entryView.deleteMe)
                {
                    _toDelete.Add(entryView);
                }
            }

            if (_toDelete.Count > 0)
            {
                DeleteEntries();
                
                foreach (var toDel in _toDelete)
                {
                    _entries.Remove(toDel);
                }

                _toDelete.Clear();
                
                SetEntries();
            }
            
            GUILayout.EndScrollView();
        }

        private void LoadEntries()
        {
            int i = 0;
            
            var cache = new Dictionary<string, Assembly>();
            
            while (PlayerPrefs.HasKey("ref-view-set-" + name + "-" + i))
            {
                var arr = PlayerPrefs.GetString("ref-view-set-" + name + "-" + i).Split('+');

                Assembly assembly;
                
                if (cache.ContainsKey(arr[0]))
                {
                    assembly = cache[arr[0]];
                }
                else
                {
                    assembly = Assembly.LoadFile(arr[0]);
                    cache.Add(arr[0], assembly);
                }
                
                var targetType = RefViewerUtils.FindType(arr[1], assembly);
                
                if (targetType != null)
                {
                    _entries.Add(new RefEntry(targetType));
                }
                
                PlayerPrefs.DeleteKey("ref-view-set-" + name + "-" + i);
                
                i++;
            }

            SetEntries();
        }
        
        private void DeleteEntries()
        {
            for (int i = 0; i < _entries.Count; i++)
            {
                PlayerPrefs.DeleteKey("ref-view-set-" + name + "-" + i);
            }
        }
        
        private void SetEntries()
        {
            for (int i = 0; i < _entries.Count; i++)
            {
                var entryView = _entries[i];
                PlayerPrefs.SetString("ref-view-set-" + name + "-" + i, entryView.assembly.Location + "+" + entryView.targetType);
            }
        }
    }
}
#endif