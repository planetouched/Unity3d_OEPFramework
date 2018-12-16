using System.Collections.Generic;
using OEPFramework.unityEngine._base;
using UnityEngine;

namespace game.common
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

        private static int overMask, mask;

        private static readonly Dictionary<int, List<GameObjectHandlerDelegate>> [] layerHandlers = new Dictionary<int, List<GameObjectHandlerDelegate>>[8];
        private static readonly Dictionary<string, List<GameObjectHandlerDelegate>> [] tagHandlers  = new Dictionary<string, List<GameObjectHandlerDelegate>>[8];
        private static readonly Dictionary<GameObject, List<HandlerDelegate>> [] goHandlers  = new Dictionary<GameObject, List<HandlerDelegate>>[8];

        private static bool down0, up0, down1, up1;

        private static readonly int[] addedLayers = new int[32];
        private static readonly int[] addedOverLayers = new int[32];
        private static bool click0;
        private static float clickTime;
        private static Vector3 clickPos;
        private static bool click1;
        private static GameObject clickedGo;
        private static bool active;
        private static GameObject lastOverGameObject;

        static void AddHandlerList(int ht)
        {
            layerHandlers[ht] =  new Dictionary<int, List<GameObjectHandlerDelegate>>();
            tagHandlers[ht] = new Dictionary<string, List<GameObjectHandlerDelegate>>();
            goHandlers[ht] = new Dictionary<GameObject, List<HandlerDelegate>>();
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
            active = true;
        }

        static void CreateOverMask()
        {
            overMask = 0;
            for (int i = 0; i < 32; i++)
            {
                if (addedOverLayers[i] > 0)
                    overMask = overMask | 1 << i;
            }
        }
        
        static void CreateMask()
        {
            mask = 0;
            for (int i = 0; i < 32; i++)
            {
                if (addedLayers[i] > 0)
                    mask = mask | 1 << i;
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
                    addedOverLayers[layer]++;
                    
                    if (!noUpdate)
                        CreateOverMask();
                }
                else
                {
                    addedLayers[layer]++;
                    
                    if (!noUpdate)
                        CreateMask();
                }

            }
            else
            {
                if (type == MouseEventHandlerType.Over)
                {
                    if (addedOverLayers[layer] > 0)
                        addedOverLayers[layer]--;
                    
                    if (!noUpdate)
                        CreateOverMask();
                }
                else
                {
                    if (addedLayers[layer] > 0)
                        addedLayers[layer]--;
                    
                    if (!noUpdate)
                        CreateMask();
                }
            }
        }

        static void CallLayerDelegates(int layer, int type, GameObject go, RaycastHit hit)
        {
            var handlers = layerHandlers[type];
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
            var handlers = tagHandlers[type];
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
            var handlers = goHandlers[type];
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
            if (layerHandlers[type].TryGetValue(layer, out list))
                list.Add(handler);
            else
                layerHandlers[type].Add(layer, new List<GameObjectHandlerDelegate> { handler });

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
            if (layerHandlers[type].TryGetValue(layer, out list))
            {
                list.Remove(handler);
                if (list.Count == 0)
                    layerHandlers[type].Remove(layer);
                
                ModifyMask(type, layer, false);
            }
        }

        //tags
        public static void AddTagHandler(string tag, GameObjectHandlerDelegate handler, IDroppableItem detacher, int type = MouseEventHandlerType.Click0)
        {
            List<GameObjectHandlerDelegate> list;
            if (tagHandlers[type].TryGetValue(tag, out list))
                list.Add(handler);
            else
                tagHandlers[type].Add(tag, new List<GameObjectHandlerDelegate> { handler });

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
            if (tagHandlers[type].TryGetValue(tag, out list))
            {
                list.Remove(handler);
                if (list.Count == 0)
                    tagHandlers[type].Remove(tag);

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
            if (goHandlers[type].TryGetValue(go, out list))
                list.Add(handler);
            else
                goHandlers[type].Add(go, new List<HandlerDelegate> { handler });

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
            if (goHandlers[type].TryGetValue(go, out list))
            {
                list.Remove(handler);
                if (list.Count == 0)
                    goHandlers[type].Remove(go);

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
            int lCount = layerHandlers[MouseEventHandlerType.Over].Count;
            int tCount = tagHandlers[MouseEventHandlerType.Over].Count;
            int oCount = goHandlers[MouseEventHandlerType.Over].Count;

            if (lCount == 0 && tCount == 0 && oCount == 0) return;

            RaycastHit hit;
            GameObject go;
            if (!Cast(out hit, out go, overMask))
            {
                if (lastOverGameObject != null)
                {
                    int lOutCount = layerHandlers[MouseEventHandlerType.ResetOver].Count;
                    int tOutCount = tagHandlers[MouseEventHandlerType.ResetOver].Count;
                    int oOutCount = goHandlers[MouseEventHandlerType.ResetOver].Count;

                    if (lOutCount > 0)
                        CallLayerDelegates(lastOverGameObject.layer, MouseEventHandlerType.ResetOver, lastOverGameObject, hit);

                    if (tOutCount > 0)
                        CallTagDelegates(MouseEventHandlerType.ResetOver, lastOverGameObject, hit);

                    if (oOutCount > 0)
                        CallGameObjectDelegates(MouseEventHandlerType.ResetOver, lastOverGameObject, hit);

                }

                lastOverGameObject = null;
                return;
            }

            if (lCount > 0)
            {
                CallLayerDelegates(go.layer, MouseEventHandlerType.Over, go, hit);
                lastOverGameObject = go;
            }

            if (tCount > 0)
            {
                CallTagDelegates(MouseEventHandlerType.Over, go, hit);
                lastOverGameObject = go;
            }

            if (oCount > 0)
            {
                CallGameObjectDelegates(MouseEventHandlerType.Over, go, hit);
                lastOverGameObject = go;
            }
        }

        static void InnerDownHandle()
        {
            if (!down0 && !down1) return;

            RaycastHit hit;
            GameObject go;
            if (!Cast(out hit, out go, mask)) return;

            if (down0)
            {
                CallLayerDelegates(go.layer, MouseEventHandlerType.Down0, go, hit);
                CallTagDelegates(MouseEventHandlerType.Down0, go, hit);
                CallGameObjectDelegates(MouseEventHandlerType.Down0, go, hit);
                click0 = true;
            }

            if (down1)
            {
                CallLayerDelegates(go.layer, MouseEventHandlerType.Down1, go, hit);
                CallTagDelegates(MouseEventHandlerType.Down1, go, hit);
                CallGameObjectDelegates(MouseEventHandlerType.Down1, go, hit);
                click1 = true;
            }

            clickTime = Time.time;
            clickPos = Input.mousePosition;
            clickedGo = go;
        }

        static bool CheckClick(bool click, GameObject go)
        {
            return click && clickedGo == go && (Time.time - clickTime) < 0.2f &&
                   Vector2.Distance(Input.mousePosition, clickPos) < Mathf.Sqrt((Screen.width ^ 2) + (Screen.height ^ 2)) / 30;
        }

        static void InnerUpHandle()
        {
            if (!up0 && !up1) return;

            RaycastHit hit;
            GameObject go;
            if (!Cast(out hit, out go, mask)) return;

            if (up0)
            {
                CallLayerDelegates(go.layer, MouseEventHandlerType.Up0, go, hit);
                CallTagDelegates(MouseEventHandlerType.Up0, go, hit);
                CallGameObjectDelegates(MouseEventHandlerType.Up0, go, hit);

                if (CheckClick(click0, go))
                {
                    CallLayerDelegates(go.layer, MouseEventHandlerType.Click0, go, hit);
                    CallTagDelegates(MouseEventHandlerType.Click0, go, hit);
                    CallGameObjectDelegates(MouseEventHandlerType.Click0, go, hit);
                }
            }

            if (up1)
            {
                CallLayerDelegates(go.layer, MouseEventHandlerType.Up1, go, hit);
                CallTagDelegates(MouseEventHandlerType.Up1, go, hit);
                CallGameObjectDelegates(MouseEventHandlerType.Up1, go, hit);

                if (CheckClick(click1, go))
                {
                    CallLayerDelegates(go.layer, MouseEventHandlerType.Click1, go, hit);
                    CallTagDelegates(MouseEventHandlerType.Click1, go, hit);
                    CallGameObjectDelegates(MouseEventHandlerType.Click1, go, hit);
                }
            }

            click0 = false;
            click1 = false;
        }

        public static void SetActive(bool active)
        {
            RaycastEvent.active = active;
        }
        
        public static void Process()
        {
            if (!active || Camera.main == null)
                return;

            if (mask == 0 && overMask == 0) return;
            
            if (overMask > 0)
                InnerOverHandle();

            if (mask == 0) return;
            
            up0 = Input.GetMouseButtonUp(0);
            up1 = Input.GetMouseButtonUp(1);
            down0 = Input.GetMouseButtonDown(0);
            down1 = Input.GetMouseButtonDown(1);

            InnerDownHandle();
            InnerUpHandle();
        }
    }
}
