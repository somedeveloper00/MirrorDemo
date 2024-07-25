using UnityEngine;

namespace MirrorDemo.Common.Core
{
    /// <summary>
    /// Provides easy functionalities and shortcuts for a player, given its token.
    /// </summary>
    public sealed class PlayerSide : MonoBehaviour
    {
        [SerializeField] private CoreGameManager coreGameManager;
        [SerializeField] private GameServer gameServer;
        public int token;

        public GameModel.GameState GameState => coreGameManager.model.state;

        public ref GameModel.Player Player => ref coreGameManager.model.GetPlayerAtIndex(coreGameManager.model.GetPlayerIndexByToken(token));

        public void SendInput(GameInput input) => gameServer.Move(input);

        public void Shoot() => gameServer.Shoot();
    }
}