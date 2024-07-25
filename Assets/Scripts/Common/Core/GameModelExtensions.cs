using UnityEngine;
using UnityEngine.Assertions;

namespace MirrorDemo.Common.Core
{
    public static class GameStateExtensions
    {
        public static bool HasFlagFast(this GameModel.GameState state, GameModel.GameState flag) => (state & flag) == flag;
    }

    public static class GameModelExtensions
    {
        public static int FindUniqueMissileId(this in GameModel gameModel)
        {
        findNewRandom:
            int id = Random.Range(0, int.MaxValue);
            for (int i = 0; i < gameModel.missiles.Length; i++)
            {
                if (gameModel.missiles[i].id == id)
                {
                    goto findNewRandom;
                }
            }
            return id;
        }

        /// <summary>
        /// Find the number of players
        /// </summary>
        public static int GetPlayerCount(this in GameModel model) =>
            (model.player1.valid ? 1 : 0) +
            (model.player2.valid ? 1 : 0) +
            (model.player3.valid ? 1 : 0) +
            (model.player4.valid ? 1 : 0);

        /// <summary>
        /// find player by <see cref="GameModel.Player.token"/>
        /// </summary>
        /// <exception cref="System.Exception"></exception>
        public static int GetPlayerIndexByToken(this ref GameModel model, int token)
        {
            if (token == model.player1.token)
                return 0;
            if (token == model.player2.token)
                return 1;
            if (token == model.player3.token)
                return 2;
            if (token == model.player4.token)
                return 3;
            throw new System.Exception("no players found with the token " + token);
        }

        public static int GetPlayersCapacity(this in GameModel model) => 4;

        public static ref GameModel.Player GetPlayerAtIndex(this ref GameModel model, int index)
        {
            Assert.IsTrue(index >= 0 && index < 4);
            switch (index)
            {
                case 0: return ref model.player1;
                case 1: return ref model.player2;
                case 2: return ref model.player3;
                case 3: return ref model.player4;
            }
            throw new System.Exception(); // won't reach here
        }
    }
}