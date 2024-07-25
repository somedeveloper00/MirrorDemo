using System;
using System.Collections.Generic;
using UnityEngine;

namespace MirrorDemo.Common
{
    /// <summary>
    /// Ensures the game object does not have duplicates
    /// </summary>
    public sealed class SingletonGameObject : MonoBehaviour
    {
        [HideInInspector]
        public string uniqueId;
        private bool _wasAdded;

        static readonly HashSet<string> s_instances = new();

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!Application.isPlaying && string.IsNullOrEmpty(uniqueId))
            {
                uniqueId = Guid.NewGuid().ToString();
            }
        }
#endif

        private void Awake()
        {
            if (!s_instances.Add(uniqueId))
            {
                Debug.LogFormat("duplicate of \"{0}\" successfully destroyed.", name);
                Destroy(gameObject);
                return;
            }
            _wasAdded = true;
        }

        private void OnDestroy()
        {
            if (_wasAdded && s_instances.Remove(uniqueId))
            {
                Debug.LogFormat("sole instance of \"{0}\" destroyed.", name);
            }
        }
    }
}