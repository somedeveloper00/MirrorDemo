using System;
using UnityEngine;

namespace MirrorDemo.Common.Core
{
    /// <summary>
    /// input for a player
    /// </summary>
    [Serializable]
    public struct GameInput
    {
        public Vector2 move;
        public bool running;

        public static GameInput operator +(GameInput left, GameInput right)
        {
            return new()
            {
                move = left.move + right.move,
                running = left.running | right.running
            };
        }

        public static GameInput operator -(GameInput left, GameInput right)
        {
            return new()
            {
                move = left.move - right.move,
                running = left.running && !right.running
            };
        }
    }
}