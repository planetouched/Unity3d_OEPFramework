#if REFVIEW
using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace Editor.RefViewer
{
    public class RefViewerWindow : EditorWindow
    {
        enum RunMode
        {
            Play = 0,
            Edit = 1
        }

        private static RefViewerWindow _instance;

        private static RunMode _mode = RunMode.Edit;

        private readonly IList<RefEntrySet> _entrySetViews = new List<RefEntrySet>();

        private RefEntrySet _currentSet;

        private bool _adding;

        private bool _derived;

        private string _typeToAdd;

        [MenuItem("DevTools/Reference Viewer/Start")]
        private static void CreateInstance()
        {
            _instance = GetWindow<RefViewerWindow>();
            _instance.autoRepaintOnSceneChange = true;
            _instance.titleContent.text = "ReferenceViewer";
            _instance.Init(true);
        }

        [MenuItem("DevTools/Reference Viewer/Clean And Start")]
        private static void CreateCleanInstance()
        {
            _instance = GetWindow<RefViewerWindow>();
            _instance.autoRepaintOnSceneChange = true;
            _instance.titleContent.text = "ReferenceViewer";
            _instance.Init(false);
        }

        private void Init(bool load)
        {
            for (int i = 0; i < 8; i++)
            {
                _entrySetViews.Add(new RefEntrySet(i.ToString(), load));
            }

            _currentSet = _entrySetViews[0];
        }

        private void DrawSetHeaders()
        {
            GUILayout.BeginHorizontal();

            foreach (var item in _entrySetViews)
            {
                var style = new GUIStyle(GUI.skin.button)
                {
                    fontStyle = _currentSet == item ? FontStyle.BoldAndItalic : FontStyle.Normal
                };
                
                if (GUILayout.Button(item.name, style))
                {
                    _currentSet = item;
                }
            }

            GUILayout.EndHorizontal();
        }

        private void DrawAddCancel()
        {
            GUILayout.BeginHorizontal();

            if (_adding)
            {
                GUILayout.Label("Type", GUILayout.Width(40));

                _typeToAdd = GUILayout.TextField(_typeToAdd);

                if (GUILayout.Button("Add", GUILayout.Width(40)) && !string.IsNullOrEmpty(_typeToAdd))
                {
                    try
                    {
                        _currentSet.PreAddEntry();
                        var targetType = RefViewerUtils.FindType(_typeToAdd);

                        if (targetType != null)
                        {
                            if (_derived)
                            {
                                foreach (var type in RefViewerUtils.FindAllDescendants(targetType))
                                {
                                    _currentSet.AddEntry(new RefEntry(type));
                                    _adding = false;
                                }
                            }
                            else
                            {
                                _currentSet.AddEntry(new RefEntry(targetType));
                                _adding = false;
                            }
                        }

                        if (_adding)
                        {
                            EditorUtility.DisplayDialog("Error", "Type does not inherit UnityEngine.Object or ReferenceCounter", "Ok");
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }
                    finally
                    {
                        _currentSet.PostAddEntry();
                    }
                }

                if (GUILayout.Button("Cancel", GUILayout.Width(80)))
                {
                    _adding = false;
                }
            }
            else
            {
                if (GUILayout.Button($"Add Type ({_currentSet.name})", GUILayout.Width(140)))
                {
                    _adding = true;
                    _derived = false;
                    _typeToAdd = "";
                }

                if (GUILayout.Button($"Add All Derived Types ({_currentSet.name})", GUILayout.Width(200)))
                {
                    _adding = true;
                    _derived = true;
                    _typeToAdd = "";
                }
            }

            GUILayout.EndHorizontal();
        }

        private void DrawClearRefresh()
        {
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Memorize set"))
            {
                _currentSet.MemorizeAll();
            }

            if (GUILayout.Button("Clear set"))
            {
                if (EditorUtility.DisplayDialog("Warning", "Do you want to clear all entries?", "Ok", "Cancel"))
                {
                    _currentSet.RemoveAllEntries();
                }
            }

            if (GUILayout.Button("Refresh"))
            {
                _entrySetViews.Clear();
                CreateInstance();
            }

            if (GUILayout.Button("GC.Collect"))
            {
                GC.Collect();
            }

            GUILayout.EndHorizontal();
        }

        private void OnGUI()
        {
            if (Application.isPlaying)
            {
                if (_mode == RunMode.Edit)
                {
                    CreateInstance();
                    _mode = RunMode.Play;
                }
            }
            else
            {
                if (_mode == RunMode.Play)
                {
                    CreateInstance();
                    _mode = RunMode.Edit;
                }
            }

            DrawSetHeaders();
            DrawClearRefresh();
            _currentSet.DrawEntities();
            DrawAddCancel();
        }
    }
}
#endif