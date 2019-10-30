#if REFVIEW
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.editor.refViewer
{
    enum Mode
    {
        Play = 0,
        Edit = 1
    }
    
    public class ReferenceViewer : EditorWindow
    {
        public static ReferenceViewer Instance { get; private set; }
        private RefEntry _tmpEntry;

        readonly Dictionary<int, List<RefEntry>> _profilerEntries = new Dictionary<int, List<RefEntry>>();

        public int EntryCount { get { return PlayerPrefs.GetInt(_currentSet + "_profilerEntryCount", 0); }}
        
        static Mode _mode = Mode.Edit;
        private int _currentSet;
        private Vector2 _scrollPosition;

        [MenuItem("DevTools/ReferenceViewer")]
        static void CreateInstance()
        {
            //_mode = Application.isPlaying ? Mode.Play : Mode.Edit;
            Instance = GetWindow<ReferenceViewer>();
            Instance.autoRepaintOnSceneChange = true;
            Instance.titleContent.text = "ReferenceViewer";
            Instance.Start();
        }


        void Start()
        {
            int lastCurrentSet = _currentSet;
            
            _profilerEntries.Clear();
            for (int set = 0; set < 8; set++)
            {
                _currentSet = set;
                _profilerEntries.Add(set, new List<RefEntry>());

                for (int i = 0; i < EntryCount; i++)
                {
                    if (!PlayerPrefs.HasKey(set + "_profiler_" + i)) continue;
                    var data = PlayerPrefs.GetString(set + "_profiler_" + i).Split('+');
                    var entry = new RefEntry(data[0], data[1]);
                    if (entry.Added)
                    {
                        _profilerEntries[set].Add(entry);
                    }
                }
            }

            _currentSet = lastCurrentSet;
        }


        void SetEntriesCount(int currentSet, int count)
        {
            PlayerPrefs.SetInt(currentSet + "_profilerEntryCount", count);
        }

        private void SetEnties()
        {
            for (int set = 0; set < 8; set++)
            {
                if (!_profilerEntries.ContainsKey(set))
                    _profilerEntries.Add(set, new List<RefEntry>());

                var list = _profilerEntries[set];
                for (int i = 0; i < list.Count; i++)
                {
                    var profilerEntry = list[i];
                    //if (PlayerPrefs.HasKey(_currentSet + "_profiler_" + i))
                    PlayerPrefs.DeleteKey(set + "_profiler_" + i);
                    PlayerPrefs.SetString(set + "_profiler_" + i, profilerEntry.assembly1.Location + "+" + profilerEntry.TargetType);
                }

                SetEntriesCount(set, list.Count);
            }
        }


        private void AddEntry(string assembly, string type)
        {
            var entry = new RefEntry(assembly, type);
            if (entry.Added)
            {
                if (!_profilerEntries.ContainsKey(_currentSet))
                    _profilerEntries.Add(_currentSet, new List<RefEntry>());

                _profilerEntries[_currentSet].Add(entry);
                SetEnties();
            }
        }

        private void AddEntry(RefEntry entry)
        {
            if (!entry.Added) return;
            //AddEntry(entry.Assembly.Location, entry.TargetType.ToString());

            if (!_profilerEntries.ContainsKey(_currentSet))
                _profilerEntries.Add(_currentSet, new List<RefEntry>());

            _profilerEntries[_currentSet].Add(entry);
            SetEnties();

            if (entry.Derived.Count > 0)
            {
                
                foreach (var type in entry.Derived)
                {
                    var entryDerived = new RefEntry(type);
                    _profilerEntries[_currentSet].Add(entryDerived);
                }
                SetEnties();
            }
        }

        void RemoveEntry(int index)
        {
            var list = _profilerEntries[_currentSet];
            list.RemoveAt(index);
            SetEnties();
        }



        void DrawSets()
        {
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Set 0"))
                _currentSet = 0;

            if (GUILayout.Button("Set 1"))
                _currentSet = 1;

            if (GUILayout.Button("Set 2"))
                _currentSet = 2;

            if (GUILayout.Button("Set 3"))
                _currentSet = 3;

            if (GUILayout.Button("Set 4"))
                _currentSet = 4;

            if (GUILayout.Button("Set 5"))
                _currentSet = 5;

            if (GUILayout.Button("Set 6"))
                _currentSet = 6;

            if (GUILayout.Button("Set 7"))
                _currentSet = 7;

            GUILayout.EndHorizontal();
        }
        

        void DrawClearRefresh()
        {
            GUILayout.BeginHorizontal();
            
            if (GUILayout.Button("Memory All"))
            {
                var list = _profilerEntries[_currentSet];
                foreach (var profilerEntry in list)
                {
                    profilerEntry.Memorize(true);
                }

            }
            if (GUILayout.Button("Clear All"))
            {
                if (EditorUtility.DisplayDialog("Внимание", "Уверен?", "Ok", "Cancel"))
                {
                    _profilerEntries.Clear();
                    SetEnties();
                }
            }
            if (GUILayout.Button("Refresh"))
            {
                CreateInstance();
            }
            if (GUILayout.Button("GC.Collect"))
            {
                GC.Collect();
            }
            GUILayout.EndHorizontal();            
        }


        void DrawAddCancel()
        {
            GUILayout.BeginHorizontal();
            if (_tmpEntry != null)
            {
                if (GUILayout.Button("Cancel", new[] { GUILayout.Width(80) }))
                {
                    _tmpEntry = null;
                }
            }
            else
            {
                if (GUILayout.Button("Add Type (Set " + _currentSet + ")", new[] { GUILayout.Width(140) }))
                {
                    _tmpEntry = new RefEntry();
                }
                if (GUILayout.Button("Add All Derived Types (Set " + _currentSet + ")", new[] { GUILayout.Width(200) }))
                {
                    _tmpEntry = new RefEntry(true);
                }

            }
            GUILayout.EndHorizontal();
        }

        
        void DrawEntries()
        {
            
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, false, false);

            if (_profilerEntries.ContainsKey(_currentSet))
            {
                var list = _profilerEntries[_currentSet];
                for (int i = 0; i < list.Count; i++)
                {
                    var profilerEntry = list[i];
                    profilerEntry.Show();
                    if (profilerEntry.Deleted)
                    {
                        RemoveEntry(i);
                        i--;
                    }
                }
            }

            if (_tmpEntry != null)
            {
                _tmpEntry.ShowInit();
                if (_tmpEntry.Added)
                {
                    AddEntry(_tmpEntry);
                    _tmpEntry = null;
                }
            }
            GUILayout.EndScrollView();
            
        }
        
        void OnGUI()
        {
            
            if (Application.isPlaying)
            {
                if (_mode == Mode.Edit)
                {
                    CreateInstance();
                    _mode = Mode.Play;
                }
            }
            else
            {
                if (_mode == Mode.Play)
                {
                    CreateInstance();
                    _mode = Mode.Edit;
                }
            }

            DrawSets();
            DrawClearRefresh();
            DrawEntries();            
            DrawAddCancel();
        }
    }
}
#endif