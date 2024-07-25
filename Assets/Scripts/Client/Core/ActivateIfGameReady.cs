using System.Threading.Tasks;
using MirrorDemo.Common.Core;
using UnityEngine;

namespace MirrorDemo.Client.Core
{
    /// <summary>
    /// A component that activates the game object only if the game is ready
    /// </summary>
    public sealed class ActivateIfGameReady : MonoBehaviour
    {
        public PlayerSide playerSide;

        private void Awake() => _ = CheckTask();

        private async Task CheckTask()
        {
            while (!destroyCancellationToken.IsCancellationRequested)
            {
                gameObject.SetActive(playerSide.GameState.HasFlagFast(GameModel.GameState.Started));
                await Task.Yield();
            }
        }
    }
}