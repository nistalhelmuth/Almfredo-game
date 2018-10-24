using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerBehaviour : MonoBehaviour
    {
        public PlayerState state
        {
            get;
            set;
        }

        private List<PlayerState> stateBox;

        void Start ()
        {
            stateBox = new List<PlayerState>();

        }

        void Update ()
        {
            foreach (PlayerState state in stateBox)
            {
                state.HandleInput();
            }
        }
    }
}
