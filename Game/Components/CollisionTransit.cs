using System;
using UnityEngine;

namespace Game.Components
{
    public class CollisionTransit : MonoBehaviour
    {
        public Action<GameObject, Collision2D> onCollisionEnter2D;
        public Action<GameObject, Collision> onCollisionEnter;
        public Action<GameObject, Collider2D> onTriggerEnter2D;
        public Action<GameObject, Collider> onTriggerEnter;

        public Action<GameObject, Collision2D> onCollisionExit2D;
        public Action<GameObject, Collision> onCollisionExit;
        public Action<GameObject, Collider2D> onTriggerExit2D;
        public Action<GameObject, Collider> onTriggerExit;

        public Action<GameObject, Collision2D> onCollisionStay2D;
        public Action<GameObject, Collision> onCollisionStay;
        public Action<GameObject, Collider2D> onTriggerStay2D;
        public Action<GameObject, Collider> onTriggerStay;

        void OnTriggerStay(Collider c)
        {
            if (onTriggerStay != null)
                onTriggerStay(gameObject, c);
        }

        void OnTriggerStay2D(Collider2D c)
        {
            if (onTriggerStay2D != null)
                onTriggerStay2D(gameObject, c);
        }

        void OnCollisionStay2D(Collision2D c)
        {
            if (onCollisionStay2D != null)
                onCollisionStay2D(gameObject, c);
        }

        void OnCollisionStay(Collision c)
        {
            if (onCollisionStay != null)
                onCollisionStay(gameObject, c);
        }

        void OnTriggerExit(Collider c)
        {
            if (onTriggerExit != null)
                onTriggerExit(gameObject, c);
        }

        void OnTriggerExit2D(Collider2D c)
        {
            if (onTriggerExit2D != null)
                onTriggerExit2D(gameObject, c);
        }

        void OnCollisionExit2D(Collision2D c)
        {
            if (onCollisionExit2D != null)
                onCollisionExit2D(gameObject, c);
        }

        void OnCollisionExit(Collision c)
        {
            if (onCollisionExit != null)
                onCollisionExit(gameObject, c);
        }

        void OnTriggerEnter(Collider c)
        {
            if (onTriggerEnter != null)
                onTriggerEnter(gameObject, c);
        }

        void OnTriggerEnter2D(Collider2D c)
        {
            if (onTriggerEnter2D != null)
                onTriggerEnter2D(gameObject, c);
        }

        void OnCollisionEnter2D(Collision2D c)
        {
            if (onCollisionEnter2D != null)
                onCollisionEnter2D(gameObject, c);
        }

        void OnCollisionEnter(Collision c)
        {
            if (onCollisionEnter != null)
                onCollisionEnter(gameObject, c);
        }

        void OnDestroy()
        {
            onCollisionEnter2D = null;
            onCollisionEnter = null;
            onTriggerEnter2D = null;
            onTriggerEnter = null;

            onCollisionExit2D = null;
            onCollisionExit = null;
            onTriggerExit2D = null;
            onTriggerExit = null;

            onCollisionStay2D = null;
            onCollisionStay = null;
            onTriggerStay2D = null;
            onTriggerStay = null;
        }
    }
}