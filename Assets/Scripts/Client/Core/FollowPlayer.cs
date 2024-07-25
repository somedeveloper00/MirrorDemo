using MirrorDemo.Common.Core;
using UnityEngine;

namespace MirrorDemo.Client.Core
{
    public sealed class FollowPlayer : MonoBehaviour
    {
        public Vector2 offset;
        public Vector3 constOffset;
        public PlayerSide playerSide;
        public float orbitSpeed;
        public Animator camAnim;
        private float _orbitX;

        private static readonly int IdleAnimHash = Animator.StringToHash("Idle");
        private static readonly int WalkingAnimHash = Animator.StringToHash("Walking");
        private static readonly int RunningAnimHash = Animator.StringToHash("Running");

        private void Update()
        {
            var inputX = Input.GetAxis("Mouse X");
            _orbitX += inputX * orbitSpeed;
            var pos = playerSide.Player.position + constOffset;
            pos.x += Mathf.Sin(_orbitX) * offset.x;
            pos.z += Mathf.Cos(_orbitX) * offset.x;
            pos.y += offset.y;

            var rot = Quaternion.LookRotation(playerSide.Player.position + constOffset - pos);
            transform.SetPositionAndRotation(pos, rot);

            ref var player = ref playerSide.Player;
            camAnim.SetBool(IdleAnimHash, player.animationState is GameModel.Player.AnimationState.Idle);
            camAnim.SetBool(WalkingAnimHash, player.animationState is GameModel.Player.AnimationState.Walking);
            camAnim.SetBool(RunningAnimHash, player.animationState is GameModel.Player.AnimationState.Running);
        }
    }
}