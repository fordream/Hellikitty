using System;
using UnityEngine;

namespace BulletLogic
{
    public class BulletLogicBase : MonoBehaviour
    {
        [HideInInspector] public Entity entity;

        public void destroy_all()
        {
            if (transform.gameObject) Destroy(transform.gameObject);
        }

        public T add_logic<T>() where T : Component
        {
            return gameObject.AddComponent<T>();
        }
    }
};