using System.Collections.Generic;
using Basement.OEPFramework.UnityEngine._Base;
using UnityEngine;

namespace Game.Common
{
    public static class MouseEventHandlerType
    {
        public const int Click0 = 0;
        public const int Up0 = 1;
        public const int Down0 = 2;
        public const int Click1 = 3;
        public const int Up1 = 4;
        public const int Down1 = 5;
        public const int Over = 6;
        public const int ResetOver = 7;
    }

    public static class RaycastEvent
    {
        //settings
        public static float maxDistance = float.MaxValue;

        public delegate void GameObjectHandlerDelegate(GameObject go, RaycastHit hit);
        public delegate void HandlerDelegate(RaycastHit hit);

        private static int _overMask, _mask;

        private static readonly Dictionary<int, List<GameObjectHandlerDelegate>> [] _layerHandlers = new Dictionary<int, List<GameObjectHandlerDelegate>>[8];
        private static readonly Dictionary<string, List<GameObjectHandlerDelegate>> [] _tagHandlers  = new Dictionary<string, List<GameObjectHandlerDelegate>>[8];
        private static readonly Dictionary<GameObject, List<HandlerDelegate>> [] _goHandlers  = new Dictionary<GameObject, List<HandlerDelegate>>[8];

        private static bool _down0, _up0, _down1, _up1;

        private static readonly int[] _addedLayers = new int[32];
        private static readonly int[] _addedOverLayers = new int[32];
        private static bool _click0;
        private static float _clickTime;
        private static Vector3 _clickPos;
        private static bool _click1;
        private static GameObject _clickedGo;
        private static bool _active;
        private static GameObject _lastOverGameObject;

        static void AddHandlerList(int ht)
        {
            _layerHandlers[ht] =  new Dictionary<int, List<GameObjectHandlerDelegate>>();
            _tagHandlers[ht] = new Dictionary<string, List<GameObjectHandlerDelegate>>();
            _goHandlers[ht] = new Dictionary<GameObject, List<HandlerDelegate>>();
        }

        static RaycastEvent()
        {
            AddHandlerList(MouseEventHandlerType.Click0);
            AddHandlerList(MouseEventHandlerType.Up0);
            AddHandlerList(MouseEventHandlerType.Down0);
            AddHandlerList(MouseEventHandlerType.Click1);
            AddHandlerList(MouseEventHandlerType.Up1);
            AddHandlerList(MouseEventHandlerType.Down1);
            AddHandlerList(MouseEventHandlerType.Over);
            AddHandlerList(MouseEventHandlerType.ResetOver);
            _active = true;
        }

        static void CreateOverMask()
        {
            _overMask = 0;
            for (int i = 0; i < 32; i++)
            {
                if (_addedOverLayers[i] > 0)
                    _overMask = _overMask | 1 << i;
            }
        }
        
        static void CreateMask()
        {
            _mask = 0;
            for (int i = 0; i < 32; i++)
            {
                if (_addedLayers[i] > 0)
                    _mask = _mask | 1 << i;
            }
        }

        static void ModifyMask(int type, int layer, bool add, bool noUpdate = false)
        {
            if (layer == -1)
            {
                CreateOverMask();
                CreateMask();
                return;
            }

            if (add)
            {
                if (type == MouseEventHandlerType.Over)
                {
                    _addedOverLayers[layer]++;
                    
                    if (!noUpdate)
                        CreateOverMask();
                }
                else
                {
                    _addedLayers[layer]++;
                    
                    if (!noUpdate)
                        CreateMask();
                }

            }
            else
            {
                if (type == MouseEventHandlerType.Over)
                {
                    if (_addedOverLayers[layer] > 0)
                        _addedOverLayers[layer]--;
                    
                    if (!noUpdate)
                        CreateOverMask();
                }
                else
                {
                    if (_addedLayers[layer] > 0)
                        _addedLayers[layer]--;
                    
                    if (!noUpdate)
                        CreateMask();
                }
            }
        }

        static void CallLayerDelegates(int layer, int type, GameObject go, RaycastHit hit)
        {
            var handlers = _layerHandlers[type];
            if (handlers.Count == 0) return;
            List<GameObjectHandlerDelegate> list;
            if (handlers.TryGetValue(layer, out list))
            {
                var copy = new List<GameObjectHandlerDelegate>(list);
                foreach (var d in copy)
                    d(go, hit);
            }
        }

        static void CallTagDelegates(int type, GameObject go, RaycastHit hit)
        {
            var handlers = _tagHandlers[type];
            if (handlers.Count == 0) return;
            List<GameObjectHandlerDelegate> list;
            if (handlers.TryGetValue(go.tag, out list))
            {
                var copy = new List<GameObjectHandlerDelegate>(list);
                foreach (var d in copy)
                    d(go, hit);
            }
        }

        static void CallGameObjectDelegates(int type, GameObject go, RaycastHit hit)
        {
            var handlers = _goHandlers[type];
            if (handlers.Count == 0) return;
            List<HandlerDelegate> list;
            if (handlers.TryGetValue(go, out list))
            {
                var copy = new List<HandlerDelegate>(list);
                foreach (var d in copy)
                    d(hit);
            }
        }

        #region add/remove handlers
        //layers
        public static void AddLayerHandler(int layer, GameObjectHandlerDelegate handler, IDroppableItem detacher, int type = MouseEventHandlerType.Click0)
        {
            List<GameObjectHandlerDelegate> list;
            if (_layerHandlers[type].TryGetValue(layer, out list))
                list.Add(handler);
            else
                _layerHandlers[type].Add(layer, new List<GameObjectHandlerDelegate> { handler });

            ModifyMask(type, layer, true);

            if (detacher != null)
            {
                detacher.onDrop += obj =>
                {
                    RemoveLayerHandler(layer, handler, type);
                };
            }
        }

        public static void RemoveLayerHandler(int layer, GameObjectHandlerDelegate handler, int type = MouseEventHandlerType.Click0)
        {
            List<GameObjectHandlerDelegate> list;
            if (_layerHandlers[type].TryGetValue(layer, out list))
            {
                list.Remove(handler);
                if (list.Count == 0)
                    _layerHandlers[type].Remove(layer);
                
                ModifyMask(type, layer, false);
            }
        }

        //tags
        public static void AddTagHandler(string tag, GameObjectHandlerDelegate handler, IDroppableItem detacher, int type = MouseEventHandlerType.Click0)
        {
            List<GameObjectHandlerDelegate> list;
            if (_tagHandlers[type].TryGetValue(tag, out list))
                list.Add(handler);
            else
                _tagHandlers[type].Add(tag, new List<GameObjectHandlerDelegate> { handler });

            //все выше стандартных
            for (int i = 8; i < 32; i++)
                ModifyMask(type, i, true, true);
            
            //update masks
            ModifyMask(type, -1, true);

            if (detacher != null)
            {
                detacher.onDrop += obj =>
                {
                    RemoveTagHandler(tag, handler, type);
                };
            }

        }

        public static void RemoveTagHandler(string tag, GameObjectHandlerDelegate handler, int type = MouseEventHandlerType.Click0)
        {
            List<GameObjectHandlerDelegate> list;
            if (_tagHandlers[type].TryGetValue(tag, out list))
            {
                list.Remove(handler);
                if (list.Count == 0)
                    _tagHandlers[type].Remove(tag);

                //все выше стандартных
                for (int i = 8; i < 32; i++)
                    ModifyMask(type, i, false, true);

                //update masks
                ModifyMask(type, -1, false);
            }
        }
        
        //game objects
        public static void AddGameObjectHandler(GameObject go, HandlerDelegate handler, IDroppableItem detacher, int type = MouseEventHandlerType.Click0)
        {
            List<HandlerDelegate> list;
            if (_goHandlers[type].TryGetValue(go, out list))
                list.Add(handler);
            else
                _goHandlers[type].Add(go, new List<HandlerDelegate> { handler });

            ModifyMask(type, go.layer, true);

            if (detacher != null)
            {
                detacher.onDrop += obj =>
                {
                    RemoveGameObjectHandler(go, handler, type);
                };
            }
        }

        public static void RemoveGameObjectHandler(GameObject go, HandlerDelegate handler, int type = MouseEventHandlerType.Click0)
        {
            List<HandlerDelegate> list;
            if (_goHandlers[type].TryGetValue(go, out list))
            {
                list.Remove(handler);
                if (list.Count == 0)
                    _goHandlers[type].Remove(go);

                ModifyMask(type, go.layer, false);
            }
        }

        
        #endregion

        static bool Cast(out RaycastHit hit, out GameObject go, int mask)
        {
            go = null;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out hit, maxDistance, mask)) 
                return false;
            go = hit.transform.gameObject;
            return true;
        }
        
        static void InnerOverHandle()
        {
            int lCount = _layerHandlers[MouseEventHandlerType.Over].Count;
            int tCount = _tagHandlers[MouseEventHandlerType.Over].Count;
            int oCount = _goHandlers[MouseEventHandlerType.Over].Count;

            if (lCount == 0 && tCount == 0 && oCount == 0) return;

            RaycastHit hit;
            GameObject go;
            if (!Cast(out hit, out go, _overMask))
            {
                if (_lastOverGameObject != null)
                {
                    int lOutCount = _layerHandlers[MouseEventHandlerType.ResetOver].Count;
                    int tOutCount = _tagHandlers[MouseEventHandlerType.ResetOver].Count;
                    int oOutCount = _goHandlers[MouseEventHandlerType.ResetOver].Count;

                    if (lOutCount > 0)
                        CallLayerDelegates(_lastOverGameObject.layer, MouseEventHandlerType.ResetOver, _lastOverGameObject, hit);

                    if (tOutCount > 0)
                        CallTagDelegates(MouseEventHandlerType.ResetOver, _lastOverGameObject, hit);

                    if (oOutCount > 0)
                        CallGameObjectDelegates(MouseEventHandlerType.ResetOver, _lastOverGameObject, hit);

                }

                _lastOverGameObject = null;
                return;
            }

            if (lCount > 0)
            {
                CallLayerDelegates(go.layer, MouseEventHandlerType.Over, go, hit);
                _lastOverGameObject = go;
            }

            if (tCount > 0)
            {
                CallTagDelegates(MouseEventHandlerType.Over, go, hit);
                _lastOverGameObject = go;
            }

            if (oCount > 0)
            {
                CallGameObjectDelegates(MouseEventHandlerType.Over, go, hit);
                _lastOverGameObject = go;
            }
        }

        static void InnerDownHandle()
        {
            if (!_down0 && !_down1) return;

            RaycastHit hit;
            GameObject go;
            if (!Cast(out hit, out go, _mask)) return;

            if (_down0)
            {
                CallLayerDelegates(go.layer, MouseEventHandlerType.Down0, go, hit);
                CallTagDelegates(MouseEventHandlerType.Down0, go, hit);
                CallGameObjectDelegates(MouseEventHandlerType.Down0, go, hit);
                _click0 = true;
            }

            if (_down1)
            {
                CallLayerDelegates(go.layer, MouseEventHandlerType.Down1, go, hit);
                CallTagDelegates(MouseEventHandlerType.Down1, go, hit);
                CallGameObjectDelegates(MouseEventHandlerType.Down1, go, hit);
                _click1 = true;
            }

            _clickTime = Time.time;
            _clickPos = Input.mousePosition;
            _clickedGo = go;
        }

        static bool CheckClick(bool click, GameObject go)
        {
            return click && _clickedGo == go && (Time.time - _clickTime) < 0.2f &&
                   Vector2.Distance(Input.mousePosition, _clickPos) < Mathf.Sqrt((Screen.width ^ 2) + (Screen.height ^ 2)) / 30;
        }

        static void InnerUpHandle()
        {
            if (!_up0 && !_up1) return;

            RaycastHit hit;
            GameObject go;
            if (!Cast(out hit, out go, _mask)) return;

            if (_up0)
            {
                CallLayerDelegates(go.layer, MouseEventHandlerType.Up0, go, hit);
                CallTagDelegates(MouseEventHandlerType.Up0, go, hit);
                CallGameObjectDelegates(MouseEventHandlerType.Up0, go, hit);

                if (CheckClick(_click0, go))
                {
                    CallLayerDelegates(go.layer, MouseEventHandlerType.Click0, go, hit);
                    CallTagDelegates(MouseEventHandlerType.Click0, go, hit);
                    CallGameObjectDelegates(MouseEventHandlerType.Click0, go, hit);
                }
            }

            if (_up1)
            {
                CallLayerDelegates(go.layer, MouseEventHandlerType.Up1, go, hit);
                CallTagDelegates(MouseEventHandlerType.Up1, go, hit);
                CallGameObjectDelegates(MouseEventHandlerType.Up1, go, hit);

                if (CheckClick(_click1, go))
                {
                    CallLayerDelegates(go.layer, MouseEventHandlerType.Click1, go, hit);
                    CallTagDelegates(MouseEventHandlerType.Click1, go, hit);
                    CallGameObjectDelegates(MouseEventHandlerType.Click1, go, hit);
                }
            }

            _click0 = false;
            _click1 = false;
        }

        public static void SetActive(bool active)
        {
            RaycastEvent._active = active;
        }
        
        public static void Process()
        {
            if (!_active || Camera.main == null)
                return;

            if (_mask == 0 && _overMask == 0) return;
            
            if (_overMask > 0)
                InnerOverHandle();

            if (_mask == 0) return;
            
            _up0 = Input.GetMouseButtonUp(0);
            _up1 = Input.GetMouseButtonUp(1);
            _down0 = Input.GetMouseButtonDown(0);
            _down1 = Input.GetMouseButtonDown(1);

            InnerDownHandle();
            InnerUpHandle();
        }
    }
}
