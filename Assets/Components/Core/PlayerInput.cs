using System;
using UnityEngine;

namespace RogueLike.Components.Core
{
    public static class PlayerInput
    {
        public const KeyCode MoveUpKey = KeyCode.W;
        public const KeyCode MoveDownKey = KeyCode.S;
        public const KeyCode MoveLeftKey = KeyCode.A;
        public const KeyCode MoveRightKey = KeyCode.D;
        public const KeyCode ReturnKey = KeyCode.Return;
        public const KeyCode AcceptKey = KeyCode.Y;
        public const KeyCode RejectKey = KeyCode.N;

        public static (int, int) InputToDirection(KeyCode key)
        {
            return key switch
            {
                MoveLeftKey => (-1, 0),
                MoveUpKey => (0, 1),
                MoveRightKey => (1, 0),
                MoveDownKey => (0, -1),
                _ => (0,0)
            };
        }

        public static KeyCode DirectionToInput((int, int) direction)
        {
            return direction switch
            {
                (0, 1) => MoveUpKey,
                (-1,0) => MoveLeftKey,
                (0,-1) => MoveDownKey,
                (1, 0) => MoveRightKey,
                _ => ReturnKey
            };
        }

        public static bool AcceptableInput((int, int) direction)
        {
            return direction switch
            {
                (-1,0) => true,
                (0, 1) => true,
                (1, 0) => true,
                (0,-1) => true,
                _ => false
            };
        }
    }
}