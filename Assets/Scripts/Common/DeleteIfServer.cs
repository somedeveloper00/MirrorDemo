using UnityEngine;

namespace MirrorDemo.Server.Core
{
    [DefaultExecutionOrder(-100)]
    public sealed class DeleteIfServer : MonoBehaviour
    {
#if UNITY_SERVER
        private void Awake()
        {
            Destroy(gameObject);
        }
#endif
    }
}