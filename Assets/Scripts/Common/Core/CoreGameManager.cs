using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Mirror;
using UnityEngine;

namespace MirrorDemo.Common.Core
{
    /// <summary>
    /// The entry of the core game. Handles core game, without caring if this is a 
    /// PVE or whether this is server or client.
    /// </summary>
    public sealed class CoreGameManager : MonoBehaviour
    {
        public GameModel model;
        [SerializeField] private Transform playerViewParent;
        [SerializeField] private PlayerView playerPrefab;
        [SerializeField] private MissileView missilePrefab;

        /// <summary>
        /// player inputs. will reset after each <see cref="Tick(float)"/>
        /// </summary>
        private GameInput[] _gameInputs = new GameInput[4];

        private List<PlayerView> _playerViews = new();
        private List<MissileView> _missileViews = new();
        private Action postTick;

        public void ViewAndTick(float dt)
        {
            View();
            Tick(Time.deltaTime);
        }

        public ref GameInput GetPlayerInput(int token) => ref _gameInputs[model.GetPlayerIndexByToken(token)];

        public void RemoveMissile(int id)
        {
            postTick += () =>
            {
                Debug.LogFormat("removed missile with id {0}", id);
                // remove model
                for (int i = 0; i < model.missiles.Length; i++)
                {
                    if (model.missiles[i].id == id)
                    {
                        model.missiles.RemoveAt(i);
                        break;
                    }
                }
            };
        }

        public void SpawnMissile(int ownerToken, Vector3 position, Quaternion rotation, float lifeTime)
        {
            postTick += () =>
            {
                int id = model.FindUniqueMissileId();
                var missile = new GameModel.Missile()
                {
                    id = id,
                    lifeTime = lifeTime,
                    ownerToken = ownerToken,
                    position = position,
                    rotation = rotation
                };
                Debug.LogFormat("launched missile with id {0} at {1}", missile.id, position);
                model.missiles.Add(missile);
            };
        }

        public void StartAttacking(ref GameModel.Player player)
        {
            if (player.animationState != GameModel.Player.AnimationState.Attacking)
            {
                player.animationState = GameModel.Player.AnimationState.Attacking;
                player.attackingDuration = 0;
                Debug.LogFormat("player {0} shot", player.token);
            }
        }

        public void DamagePlayer(int token, float damage)
        {
            postTick += () =>
            {
                Debug.LogFormat("damaging player {0} by {1}", token, damage);
                ref var player = ref model.GetPlayerAtIndex(model.GetPlayerIndexByToken(token));
                player.hp -= damage;
            };
        }

        private void View()
        {
            // player
            int playersCount = model.GetPlayerCount();
            UpdatePlayerViewReferences(playersCount);
            for (int i = 0; i < playersCount; i++)
            {
                _playerViews[i].View(model.GetPlayerAtIndex(i));
            }

            // missile
            int missileCount = model.missiles.Length;
            UpdateMissileViewReferences(missileCount);
            for (int i = 0; i < missileCount; i++)
            {
                _missileViews[i].View(model.missiles[i]);
            }
        }

        private void Tick(float deltaTime)
        {
            // player & input
            int playersCount = model.GetPlayerCount();
            EnsureInputsCount(playersCount);
            for (int i = 0; i < playersCount; i++)
            {
                _playerViews[i].Tick(ref model.GetPlayerAtIndex(i), in _gameInputs[i], deltaTime, this);
                _gameInputs[i] = default;
            }

            // missile
            int missilesCount = model.missiles.Length;
            for (int i = 0; i < missilesCount; i++)
            {
                _missileViews[i].Tick(ref model.missiles.ElementAt(i), Time.deltaTime, this);
            }

            postTick?.Invoke();
            postTick = null;
        }

        private void EnsureInputsCount(int playersCount)
        {
            if (playersCount != _gameInputs.Length)
            {
                Array.Resize(ref _gameInputs, playersCount);
            }
        }

        private void UpdatePlayerViewReferences(int count)
        {
            bool added = false;
            // add
            for (int i = 0; i < count; i++)
            {
                if (_playerViews.Count <= i)
                {
                    _playerViews.Add(Instantiate(playerPrefab, playerViewParent));
                    added = true;
                }
            }
            // remove
            if (!added && _playerViews.Count != count)
            {
                for (int i = _playerViews.Count - 1; i >= count; i--)
                {
                    Destroy(_playerViews[i].gameObject);
                }
                _playerViews.RemoveRange(count, _playerViews.Count - count);
            }
        }

        public void UpdateMissileViewReferences(int count)
        {
            bool added = false;
            // add
            for (int i = 0; i < count; i++)
            {
                if (_missileViews.Count <= i)
                {
                    _missileViews.Add(Instantiate(missilePrefab, playerViewParent));
                    added = true;
                }
            }
            // remove
            if (!added && _missileViews.Count != count)
            {
                for (int i = _missileViews.Count - 1; i >= count; i--)
                {
                    Destroy(_missileViews[i].gameObject);
                }
                _missileViews.RemoveRange(count, _missileViews.Count - count);
            }
        }
    }
}