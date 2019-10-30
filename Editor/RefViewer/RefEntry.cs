#if REFVIEW
using System;
using System.Collections.Generic;
using System.Reflection;
using Basement.Common.Util;
using UnityEditor;
using UnityEngine;

namespace Assets.editor.refViewer
{
    class RefEntry
    {
        public bool Added { get; private set; }
        public bool Deleted { get; private set; }
        public Type TargetType { get; private set; }
        public Assembly assembly1 { get; private set; }
        public List<Type> Derived = new List<Type>();
        private string _type = "";
        private int _saveCount;
        private readonly bool _getAllDerived;
        private bool _memorize;
        private bool _first;

        static IEnumerable<Type> GetTypes(Assembly assembly)
        {
            Type[] allTypes = null;
            try
            {
                allTypes = assembly.GetTypes();
            }
            catch (Exception)
            {
            }

            if (allTypes != null)
            {
                foreach (var type in allTypes)
                {
                    yield return type;
                }

            }
        }        
        public void ShowInit()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Type", new[] { GUILayout.Width(40) });
            _type = GUILayout.TextField(_type);
            if (GUILayout.Button("Add", new[] { GUILayout.Width(40) }))
            {
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();  //AssemblyHelper.GetNamesOfAssembliesLoadedInCurrentDomain();
                foreach (var assembly in assemblies)
                {
                    assembly1 = assembly;//Assembly.LoadFile(assembly.Name);
                    foreach (var type in GetTypes(assembly1))
                    {
                        if (type.ToString().EndsWith(_type) || type.ToString().EndsWith(_type + "`1[T]") || type.ToString().EndsWith(_type + "`2[T]") || type.ToString().EndsWith(_type + "`3[T]"))
                        {
                            TargetType = type;

                            //смотрим есть ли в родителях UnityEngine.Object
                            Type testType = TargetType;
                            while (testType != null && testType != typeof(UnityEngine.Object))
                            {
                                testType = testType.BaseType;
                            }

                            if (testType != null)
                                Added = true;

                            if (!Added)
                            {
                                //проверим наследован ли класс от referenceCounter
                                testType = TargetType;
                                while (testType != null && testType != typeof(ReferenceCounter))
                                {
                                    testType = testType.BaseType;
                                }
                                if (testType != null)
                                    Added = true;

                            }

                            GUILayout.EndHorizontal();
                            if (!Added)
                            {
                                EditorUtility.DisplayDialog("Error", "Данный тип не является наследником UnityEngine.Object или ReferenceCounter ", "Ok");
                            }
                            goto DN001;
                        }
                    }
                }

                DN001:
                if (Added && _getAllDerived)
                {
                    //добавим всех наследников
                    //Derived
                    foreach (var assembly in assemblies)
                    {
                        assembly1 = assembly;//Assembly.LoadFile(assembly.);
                        foreach (var type in GetTypes(assembly1))
                        {
                            Type testType = type;
                            while (testType != null)
                            {
                                testType = testType.BaseType;
                                if (testType == TargetType) //testType наследник
                                    break;
                            }

                            if (testType != null)
                                Derived.Add(type);
                        }
                    }
                }



                if (!Added)
                {
                    EditorUtility.DisplayDialog("Error", "Данный тип не определен ни в одной сборке или не является наследником UnityEngine.Object или ReferenceCounter ", "Ok");
                }
            }
            GUILayout.EndHorizontal();
        }

        public RefEntry()
        { }
        
        public RefEntry(Type type)
        {
            TargetType = type;
            assembly1 = type.Assembly;
            Added = true;
        }

        public RefEntry(bool getAllDerived)
        {
            _getAllDerived = getAllDerived;
        }
        
        public RefEntry(string assembly, string type)
        {
            assembly1 = Assembly.LoadFile(assembly);
            foreach (var t in GetTypes(assembly1))
            {
                if (t.ToString().EndsWith(type) || t.ToString().EndsWith(type + "`1[T]") || t.ToString().EndsWith(type + "`2[T]") || t.ToString().EndsWith(type + "`3[T]"))
                {
                    TargetType = t;
                    Added = true;
                    break;
                }
            }
        }


        public void Memorize(bool mem)
        {
            _memorize = mem;
            _first = true;
        }
        
        public void Show()
        {
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("M"))
            {
                Memorize(true);
            }
            
            GUILayout.Label("Type: ");
            GUILayout.Label(TargetType.ToString(), new GUIStyle("Label") {normal = {textColor = Color.green}});

            //смотрим есть ли в родителях UnityEngine.Object
            Type testType = TargetType;
            while (testType != null && testType != typeof(UnityEngine.Object))
            {
                testType = testType.BaseType;
            }

            if (testType != null)
            {
                var objects = UnityEngine.Object.FindObjectsOfType(TargetType);
                int refCount = objects.Length;
                Color color = Color.white;
                if (!_first && _memorize && refCount != _saveCount)
                    color = Color.red;
                var style = new GUIStyle("Label") { normal = { textColor = color } };
                GUILayout.Label("" + refCount, style);
                if (_memorize && _first)
                {
                    _saveCount = refCount;
                    _first = false;
                }

                if (_memorize)
                {
                    GUILayout.Label("(" + _saveCount + ")");
                }
            }
            else
            {
                int refCount = ReferenceCounter.GetCount(TargetType);
                Color color = Color.white;
                if (!_first && _memorize && refCount != _saveCount)
                    color = Color.red;
                var style = new GUIStyle("Label") { normal = { textColor = color } };
                GUILayout.Label("" + refCount, style);

                if (_memorize && _first)
                {
                    _saveCount = ReferenceCounter.GetCount(TargetType);
                    _first = false;
                }

                if (_memorize)
                {
                    GUILayout.Label("(" + _saveCount + ")");
                }

            }


            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Del", new[] {GUILayout.Width(40)}))
            {
                Deleted = true;
            }

            GUILayout.EndHorizontal();
        }

    }
}
#endif