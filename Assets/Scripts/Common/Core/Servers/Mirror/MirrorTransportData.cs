using Mirror;
using UnityEngine;

namespace MirrorDemo.Common.Core.Mirror
{
    public sealed class MirrorTransportData : NetworkBehaviour
    {
        [SyncVar(hook = nameof(Updated_Players))] public GameModel.Player[] players;
        [SyncVar(hook = nameof(Updated_Missiles))] public GameModel.Missile[] missiles;
        [SyncVar(hook = nameof(Updated_Statistics))] public GameModel.Statistics statistics;
        [SyncVar(hook = nameof(Updated_GameState))] public GameModel.GameState gameState;

        public void Apply(ref GameModel model)
        {
            for (int i = 0; i < players.Length; i++)
                model.GetPlayerAtIndex(i) = players[i];
            for (int i = players.Length; i < model.GetPlayersCapacity(); i++)
                model.GetPlayerAtIndex(i).valid = false;

            model.missiles.Length = missiles.Length;
            for (int i = 0; i < missiles.Length; i++)
                model.missiles[i] = missiles[i];

            model.statistics = statistics;
            model.state = gameState;
        }

        public void From(ref GameModel model)
        {
            int playersCount = model.GetPlayerCount();
            players ??= new GameModel.Player[playersCount];
            if (players.Length != playersCount)
                players = new GameModel.Player[playersCount];
            for (int i = 0; i < playersCount; i++)
                players[i] = model.GetPlayerAtIndex(i);

            int missilesCount = model.missiles.Length;
            missiles ??= new GameModel.Missile[missilesCount];
            if (missiles.Length != missilesCount)
                missiles = new GameModel.Missile[missilesCount];
            for (int i = 0; i < missilesCount; i++)
                missiles[i] = model.missiles[i];

            statistics = model.statistics;
            gameState = model.state;
        }

        private void Updated_Players(GameModel.Player[] old, GameModel.Player[] @new)
        {
            // Debug.LogFormat("players value updated");
        }

        private void Updated_Missiles(GameModel.Missile[] old, GameModel.Missile[] @new)
        {
            // Debug.LogFormat("missiles value updated");
        }

        private void Updated_Statistics(GameModel.Statistics old, GameModel.Statistics @new)
        {
            // Debug.LogFormat("statistics value updated");
        }

        private void Updated_GameState(GameModel.GameState old, GameModel.GameState @new)
        {
            // Debug.LogFormat("game state value updated");
        }

        [ClientRpc]
        public void HelloRpc()
        {
            Debug.LogFormat("Hello!");
        }
    }
}