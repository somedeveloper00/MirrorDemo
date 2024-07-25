using UnityEngine;

namespace MirrorDemo.Common
{
    /// <summary>
    /// Marks the game object to not destroy on load on <see cref="Awake"/>
    /// </summary>
    public sealed class MarkDontDestroyOnLoad : MonoBehaviour
    {
        private void Awake() => DontDestroyOnLoad(gameObject);
    }
}