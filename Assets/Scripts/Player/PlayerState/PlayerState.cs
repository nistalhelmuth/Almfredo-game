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

        public abstract PlayerState TheListener();

        
    }
}