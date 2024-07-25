using System;
using UnityEngine;

namespace MirrorDemo.Common.Core
{
    /// <summary>
    /// A <see cref="GameServer"/> that works locally
    /// </summary>
    public sealed class LocalGameServer : GameServer
    {
        public int token;

        [Header("Start Params")]
        public User[] users;

        private void Awake()
        {
            StartGameManager();
            Debug.LogFormat("game initials setup");
        }

        private void Update()
        {
            coreGameManager.ViewAndTick(Time.deltaTime);
        }

        public override void Move(GameInput gameInput)
        {
            // apply input directly
            ref var inp = ref coreGameManager.GetPlayerInput(token);
            inp += gameInput;
        }

        public override void Shoot()
        {
            ref var model = ref coreGameManager.model;
            ref GameModel.Player player = ref model.GetPlayerAtIndex(model.GetPlayerIndexByToken(token));
            coreGameManager.StartAttacking(ref player);
        }

        private void StartGameManager()
        {
            for (int i = 0; i < users.Length; i++)
            {
                var user = users[i];
                ref var player = ref coreGameManager.model.GetPlayerAtIndex(i);
                player.valid = true;
                player.username = user.username;
                player.token = user.token;
                player.position = user.startPosition;
                player.maxHp = player.hp = 100;
            }
            coreGameManager.model.state = GameModel.GameState.Started;
            coreGameManager.enabled = true;
        }

        [Serializable]
        public struct User
        {
            public string username;
            public int token;
            public Vector3 startPosition;
        }
    }
}