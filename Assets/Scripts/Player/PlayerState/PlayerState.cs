using UnityEngine;
using System;

namespace Player
{
    public abstract class PlayerState
    {
        public PlayerBehaviour Player;

        public PlayerState(PlayerBehaviour player)
        {
            Player = player;
        }
        public abstract void HandleInput();
    }
}
