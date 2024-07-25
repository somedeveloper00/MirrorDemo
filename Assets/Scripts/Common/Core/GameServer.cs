using UnityEngine;

namespace MirrorDemo.Common.Core
{
    /// <summary>
    /// Responsible for delivering inputs to <see cref="CoreGameManager"/>
    /// </summary>
    public abstract class GameServer : MonoBehaviour
    {
        [SerializeField]
        protected CoreGameManager coreGameManager;

        public abstract void Move(GameInput gameInput);

        public abstract void Shoot();
    }
}