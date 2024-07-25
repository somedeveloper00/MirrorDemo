using System;
using Mirror;
using UnityEngine;

namespace MirrorDemo.Common.Core.Mirror
{
    /// <summary>
    /// Simple player to be spawned per player in Mirror. Works as input
    /// </summary>
    public sealed class MirrorPlayer : NetworkBehaviour
    {
        [SyncVar(hook = nameof(GameInputChanged))]
        public GameInput gameInput;
        public event Action<GameInput> OnGameInputChanged;
        public event Action OnShoot;

        public static event Action<MirrorPlayer> NewInstantiated;

        private void Awake()
        {
            NewInstantiated?.Invoke(this);
        }

        private void GameInputChanged(GameInput _, GameInput input)
        {
            Debug.LogFormat("input changed...");
            OnGameInputChanged?.Invoke(input);
        }

        [Command]
        public void Shoot()
        {
            Debug.LogFormat("shoot...");
            OnShoot?.Invoke();
        }
    }
}