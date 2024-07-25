using System;
using System.Collections.Generic;
using UnityEngine;

namespace MirrorDemo.Common.Core
{
    /// <summary>
    /// Components implementing this interface will receive <see cref="View(in GameModel.Player)"/> 
    /// for their parent <see cref="PlayerView"/> component.
    /// </summary>
    public interface IPlayerViewHandler
    {
        void View(in GameModel.Player player);
    }

    public sealed class PlayerView : MonoBehaviour
    {
        /// <summary>
        /// Token of the last <see cref="GameModel.Player"/> this component viewed.
        /// </summary>
        public int LastViewedPlayerToken { get; private set; }

        [SerializeField] private Animator animator;
        [SerializeField] private float walkSpeed;
        [SerializeField] private float runSpeed;

        [Tooltip("Point in attacking animation when the player actually attacks")]
        [SerializeField] private float attackAnimPoint;
        [SerializeField] private Transform missileSpawnPoint;
        [SerializeField] private float missileLifetime;

        private static readonly int AnimHash_Idle = Animator.StringToHash("Idle");
        private static readonly int AnimHash_Walking = Animator.StringToHash("Walking");
        private static readonly int AnimHash_Running = Animator.StringToHash("Running");
        private static readonly int AnimHash_Attacking = Animator.StringToHash("Attacking");
        private static readonly int AnimHash_Dying = Animator.StringToHash("Dying");

        private IPlayerViewHandler[] _viewHandlers;
        private bool _wasDead;

        private void Awake()
        {
            _viewHandlers = GetComponentsInChildren<IPlayerViewHandler>() ?? Array.Empty<IPlayerViewHandler>();
        }

        public void View(in GameModel.Player player)
        {
            for (int i = 0; i < _viewHandlers.Length; i++)
            {
                _viewHandlers[i].View(player);
            }

            if (!player.alive)
            {
                if (!_wasDead)
                {
                    _wasDead = true;
                    animator.SetTrigger(AnimHash_Dying);
                    enabled = false; // don't trigger GetComponent(s)
                    return;
                }
            }

            if (_wasDead)
            {
                enabled = true;
                animator.WriteDefaultValues();
            }

            transform.SetPositionAndRotation(player.position, player.rotation);
            animator.SetBool(AnimHash_Idle, player.animationState is GameModel.Player.AnimationState.Idle);
            animator.SetBool(AnimHash_Walking, player.animationState is GameModel.Player.AnimationState.Walking);
            animator.SetBool(AnimHash_Running, player.animationState is GameModel.Player.AnimationState.Running);
            animator.SetBool(AnimHash_Attacking, player.animationState is GameModel.Player.AnimationState.Attacking);
            LastViewedPlayerToken = player.token;
            _wasDead = false;
        }

        public void Tick(ref GameModel.Player player, in GameInput gameInput, float dt, CoreGameManager coreGameManager)
        {
            // movement
            if (player.animationState != GameModel.Player.AnimationState.Attacking)
            {
                var prevPos = player.position;
                player.position.x += gameInput.move.x * (gameInput.running ? runSpeed : walkSpeed) * dt;
                player.position.z += gameInput.move.y * (gameInput.running ? runSpeed : walkSpeed) * dt;

                if (player.animationState != GameModel.Player.AnimationState.Attacking)
                {
                    player.animationState = prevPos == player.position
                    ? GameModel.Player.AnimationState.Idle
                    : gameInput.running
                        ? GameModel.Player.AnimationState.Running
                        : GameModel.Player.AnimationState.Walking;
                }
                if (player.animationState is
                    GameModel.Player.AnimationState.Walking or
                    GameModel.Player.AnimationState.Running)
                {
                    player.rotation = Quaternion.LookRotation(player.position - prevPos);
                }
            }

            // attacking
            if (player.animationState is GameModel.Player.AnimationState.Attacking)
            {
                player.attackingDuration += dt;
                if (player.attackingDuration > attackAnimPoint)
                {
                    // attack
                    player.animationState = GameModel.Player.AnimationState.Idle;
                    coreGameManager.SpawnMissile(player.token, missileSpawnPoint.position, player.rotation, missileLifetime);
                }
            }
        }
    }
}