using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerBehaviour : MonoBehaviour
    {
        public Animator Anim
        {
            set;
            get;
        }

        public delegate void StateDelegate();
        public StateDelegate ActionHandler;
        public StateDelegate PhysicsHandler;

        public string HorizontalAxis = "Horizontal";
        public string VerticalAxis = "Vertical";

        public Rigidbody body;
        public Vector3 Mdirection;
        public Transform playerTrans;
        public float speed;

        void Start ()
        {
            body = GetComponent<Rigidbody>();
            Anim = GetComponent<Animator>();
            playerTrans = transform;
            new IdleState(this);
        }

        void Update ()
        {
            Mdirection = new Vector3(Input.GetAxis(HorizontalAxis), 0f, Input.GetAxis(VerticalAxis));
            ActionHandler();
        }

        void FixedUpdate()
        {
            if (PhysicsHandler != null)
            {
                PhysicsHandler();
            };
        }

        /*
        void collision{
            new hurtState(Player);
        }*/
    }
}
