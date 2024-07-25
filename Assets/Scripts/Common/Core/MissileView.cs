using UnityEngine;

namespace MirrorDemo.Common.Core
{
    [RequireComponent(typeof(FastCollider))]
    public sealed class MissileView : MonoBehaviour
    {
        public float speed;
        public float damage;
        private FastCollider _fastCollider;

        private void Awake()
        {
            _fastCollider = GetComponent<FastCollider>();
        }

        public void View(in GameModel.Missile model)
        {
            transform.SetPositionAndRotation(model.position, model.rotation);
        }

        public void Tick(ref GameModel.Missile model, float dt, CoreGameManager coreGameManager)
        {
            model.position += model.rotation * Vector3.forward * speed * dt;
            if ((model.lifeTime -= dt) <= 0)
            {
                coreGameManager.RemoveMissile(model.id);
            }

            // check for collision
            if (_fastCollider.TryGetCollidingCollider(out var other))
            {
                if (other.TryGetComponent<PlayerView>(out var player))
                {
                    if (player.LastViewedPlayerToken != model.ownerToken) // ignore owner
                    {
                        // damage & destroy
                        coreGameManager.DamagePlayer(player.LastViewedPlayerToken, damage);
                        coreGameManager.RemoveMissile(model.id);
                    }
                }
            }
        }
    }
}