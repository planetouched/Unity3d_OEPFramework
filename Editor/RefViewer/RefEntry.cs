#if REFVIEW
using System;
using System.Reflection;
using Basement.Common.Util;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Editor.RefViewer
{
    public class RefEntry
    {
        private bool _memorize;
        private int _saveCount;
        private int _count;
        
        public Type targetType { get; }
        public Assembly assembly { get; }
        
        public bool deleteMe { get; private set; }

        public RefEntry(Type type)
        {
            targetType = type;
            assembly = type.Assembly;
        }        
        
        public void Memorize(bool memorize)
        {
            _memorize = memorize;
            _saveCount = _count;
        }
        
        public void Draw()
        {
            var isPro = EditorGUIUtility.isProSkin;

            _count = 0;
            
            if (RefViewerUtils.IsDescendant(targetType, typeof(Object)))
            {
                _count = Object.FindObjectsOfType(targetType).Length;
            }
            else if (RefViewerUtils.IsDescendant(targetType, typeof(ReferenceCounter)))
            {
                _count = ReferenceCounter.GetCount(targetType);
            }
            
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("M"))
            {
                Memorize(!_memorize);
            }

            Color color = isPro ? Color.white : new Color(0, 0.4f, 0);
            var color1 = color;

            if (_memorize && _count != _saveCount)
            {
                color = isPro ? Color.red : new Color(0.6f, 0, 0);
            }
            
            var style = new GUIStyle("Label") { normal = { textColor = color } };
            
            GUILayout.Label(_count.ToString(), style);
            
            if (_memorize)
            {
                GUILayout.Label("[" + _saveCount + "]", new GUIStyle("Label") { normal = { textColor = color1 } });
            }
            
            GUILayout.Label(targetType.ToString(), new GUIStyle("Label") {normal = {textColor = isPro ? Color.green : Color.black}});
            
            GUILayout.FlexibleSpace();
            
            if (GUILayout.Button("Delete", GUILayout.Width(60)))
            {
                deleteMe = true;
            }
            
            GUILayout.EndHorizontal();
        }
    }
}
#endif