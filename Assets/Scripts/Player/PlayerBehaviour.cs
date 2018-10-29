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

        public delegate void StateDelegate();
        public StateDelegate actionHandler;

        public string HorizontalAxis="Horizontal";
        public string VerticalAxis="Vertical";

        public Rigidbody body;
        public Vector3 Mdirection;
        public Transform playerTrans;
        public float speed;

        void Start ()
        {
            state = new IdleState(this);
            body = GetComponent<Rigidbody>();
            playerTrans= transform;
        }

        void Update ()
        {
            Mdirection = new Vector3(Input.GetAxis(HorizontalAxis), 0f, Input.GetAxis(VerticalAxis));
            state = state.TheListener();
        }

        void FixedUpdate()
        {
            if (actionHandler != null)
            {
                actionHandler();
            };
        }
    }
}
