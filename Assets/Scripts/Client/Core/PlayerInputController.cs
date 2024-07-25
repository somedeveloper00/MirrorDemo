using MirrorDemo.Common.Core;
using UnityEngine;

namespace MirrorDemo.Client.Core
{
    public sealed class PlayerInputController : MonoBehaviour
    {
        [Tooltip("Camera to use for detecting directions")]
        public Camera playerCamera;
        public PlayerSide playerSide;

        private void Update()
        {
            if (!playerSide.GameState.HasFlagFast(GameModel.GameState.Started))
            {
                return;
            }

            // movement
            var moveX = Input.GetAxis("Horizontal");
            var moveY = Input.GetAxis("Vertical");
            var dir = playerCamera.transform.rotation * new Vector3(moveX, 0, moveY);
            dir.y = 0;
            var running = Input.GetKey(KeyCode.LeftShift);
            playerSide.SendInput(new()
            {
                move = new(dir.x, dir.z),
                running = running
            });

            // attack
            if (Input.GetKeyDown(KeyCode.Space))
            {
                playerSide.Shoot();
            }
        }
    }
}