using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace OEPFramework.unityEngine.behaviour
{
    public abstract class GUIBehaviour : ControlLoopBehaviour
    {
        private readonly string prefabPath;
        private RectTransform parent;
        private readonly bool external;

        public GameObject gameObject { get; private set; }
        public RectTransform rectTransform { get; private set; }
        private Dictionary<string, GameObject> map;
        private string startWith;
        private GameObject template;

        protected GUIBehaviour(GameObject go)
        {
            external = true;
            gameObject = go;
        }

        protected GUIBehaviour(string prefabPath, RectTransform parent)
        {
            this.parent = parent;
            this.prefabPath = prefabPath;
        }
        protected GUIBehaviour(GameObject template, RectTransform parent)
        {
            this.parent = parent;
            this.template = template;
        }

        public void ResetRectTransform(bool resetLocalScale = false, bool resetSizeDelta = false, bool resetLocalRotation = false)
        {
            gameObject.SetActive(false);
            rectTransform.anchoredPosition = Vector3.zero;
            rectTransform.localPosition = Vector3.zero;

            if (resetLocalRotation)
                rectTransform.localRotation = Quaternion.Euler(Vector3.zero);

            if (resetLocalScale)
                rectTransform.localScale = Vector3.one;

            if (resetSizeDelta)
                rectTransform.sizeDelta = Vector3.zero;

            gameObject.SetActive(true);
        }

        public virtual void Create()
        {
            if (initialized) return;

            if (!external)
            {
                gameObject = Object.Instantiate(template ?? Resources.Load<GameObject>(prefabPath));
                template = null;
                rectTransform = gameObject.GetComponent<RectTransform>();

                if (parent != null)
                {
                    gameObject.SetActive(false);
                    gameObject.transform.SetParent(parent);
                    gameObject.SetActive(true);
                }
            }
            else
                rectTransform = gameObject.GetComponent<RectTransform>();

            Initialize();
        }

        public void CreateElementsMap(string prefix = "_")
        {
            startWith = prefix;
            map = new Dictionary<string, GameObject>();
            InnerCreateMap(gameObject);
        }

        public GameObject GetElement(string elementName)
        {
            return map[elementName];
        }

        public T GetElementComponent<T>(string elementName)
        {
            return map[elementName].GetComponent<T>();
        }

        public RectTransform GetRectTransform(string name)
        {
            return map[name].GetComponent<RectTransform>();
        }

        public void InnerCreateMap(GameObject go)
        {
            int count = go.transform.childCount;
            for (int i = 0; i < count; i++)
            {
                Transform t = go.transform.GetChild(i);

                if (t.name.StartsWith(startWith))
                    map.Add(t.name, t.gameObject);

                if (t.childCount > 0)
                    InnerCreateMap(t.gameObject);
            }
        }

        public void SetParent(RectTransform transform)
        {
            if (external)
                throw new InvalidOperationException();

            parent = transform;

            rectTransform.gameObject.SetActive(false);
            rectTransform.parent = parent;
            rectTransform.gameObject.SetActive(true);
        }

        public virtual void SetActive(bool active)
        {
            if (gameObject != null && gameObject.activeSelf != active)
                gameObject.SetActive(active);
        }

        protected override void OnUninitialize()
        {
            if (!external)
                Object.Destroy(gameObject);
        }
    }
}
