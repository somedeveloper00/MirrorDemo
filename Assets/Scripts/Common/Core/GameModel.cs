using System;
using Unity.Collections;
using UnityEngine;

namespace MirrorDemo.Common.Core
{
    /// <summary>
    /// Current state of the game. Consists of unmanaged data.
    /// </summary>
    [Serializable]
    public struct GameModel
    {
        public Player player1, player2, player3, player4;
        public FixedList128Bytes<Missile> missiles;
        public Statistics statistics;
        public GameState state;

        [Serializable]
        public struct Statistics : IEquatable<Statistics>
        {
            public float time;

            public readonly bool Equals(Statistics other) => time == other.time;
        }

        [Flags]
        public enum GameState
        {
            Started = 1 << 0,
        }

        [Serializable]
        public struct Player : IEquatable<Player>
        {
            /// <summary>
            /// Whether or not this player is valid in this match
            /// </summary>
            public bool valid;

            public readonly bool alive => valid && hp > 0;

            /// <summary>
            /// Whether or not this player is connected to the server at this moment
            /// </summary>
            public bool connected;

            public FixedString128Bytes username;

            /// <summary>
            /// unique token. can be used to identify players
            /// </summary>
            public int token;

            public Vector3 position;
            public Quaternion rotation;
            public AnimationState animationState;

            /// <summary>
            /// indicates how long (in seconds) its been at attacking state
            /// </summary>
            public float attackingDuration;

            public float maxHp;
            public float hp;

            public enum AnimationState
            {
                Idle,
                Walking,
                Running,
                Attacking,
            }

            public readonly bool Equals(Player other) => GetHashCode() == other.GetHashCode();
        }

        [Serializable]
        public struct Missile
        {
            /// <summary>
            /// unique id of this missile
            /// </summary>
            public int id;

            /// <summary>
            /// token of the player who shot this missile
            /// </summary>
            public int ownerToken;

            public Vector3 position;
            public Quaternion rotation;

            /// <summary>
            /// remaining time (in second) for this missile
            /// </summary>
            public float lifeTime;
        }
    }
}