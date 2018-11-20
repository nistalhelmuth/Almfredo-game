﻿using System.Collections;
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
        public int playerState 
        {
            set;
            get;
        }
        public float playerDeltaTime
        {
            set;
            get;
        }
        public Vector3 Mdirection
        {
            set;
            get;
        }
        public delegate void StateDelegate();
        public AudioClip shootSound;
        public AudioClip hurtSound;
        public AudioClip biteSound;
        public AudioSource MusicSource;
        public StateDelegate ActionHandler;
        public StateDelegate PhysicsHandler;
        public string HorizontalAxis = "Horizontal";
        public string VerticalAxis = "Vertical";
        public GameObject fireBallPrefab;
        public RectTransform powerBar;
        public GameObject powerCanvas;
        public float speed;
        private float invicibilityCounter;
        private float flashCounter;

        public Rigidbody body;
        
        public Renderer playerRender;
        public bool eating;

        void Start ()
        {
            body = GetComponent<Rigidbody>();
            Anim = GetComponent<Animator>();
            powerCanvas.SetActive(false);
            playerState = -1; //no tiene poderes
            eating=false;
            new IdleState(this);
        }

        void Update ()
        {
            Mdirection = new Vector3(Input.GetAxis(HorizontalAxis), 0f, Input.GetAxis(VerticalAxis));
            playerDeltaTime = Time.deltaTime;
            ActionHandler();
        }

        void FixedUpdate()
        {
            if (PhysicsHandler != null)
            {
                PhysicsHandler();
            };
            
        }

        public void takeDmg(Vector3 _hitDirection)
        {
            if (invicibilityCounter <= 0 && !eating)
            {    
                MusicSource.PlayOneShot(hurtSound, 1F);
                body.velocity = Vector3.zero;
                body.AddForce(_hitDirection * 3f, ForceMode.VelocityChange);
                transform.forward = _hitDirection;
                invicibilityCounter = 1;
                playerRender.enabled = false;
                flashCounter = 0.1f;
                PhysicsHandler += HurtAction;
            }
        }

        public void HurtAction()
        {
            if (invicibilityCounter > 0)
            {
                invicibilityCounter -= Time.deltaTime;
                flashCounter -= Time.deltaTime;
                if (flashCounter <= 0) 
                {
                    playerRender.enabled = !playerRender.enabled;
                    flashCounter = 0.1f;
                }
            } 
            else 
            {
                playerRender.enabled = true;
                PhysicsHandler -= HurtAction;
            }
        }
    }
}
