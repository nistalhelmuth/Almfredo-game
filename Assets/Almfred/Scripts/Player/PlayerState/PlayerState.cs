using UnityEngine;
using System;

namespace Player
{
    public abstract class PlayerState
    {
        public PlayerBehaviour Player
        {
            get;
            set;
        }

        public PlayerState(PlayerBehaviour player)
        {
            this.Player = player;
        }

        public abstract void TheListener();
    }
}
