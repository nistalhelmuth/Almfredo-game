using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Player
{
    public class PlayerBehaviour : NetworkBehaviour
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

        public bool TriggerPressed
        {
            get;
            set;
        }
        public delegate void StateDelegate();
        public StateDelegate ActionHandler;
        public StateDelegate PhysicsHandler;
        public AudioClip shootSound;
        public AudioClip hurtSound;
        public AudioClip biteSound;
        public AudioSource MusicSource;
        public string HorizontalAxis = "Horizontal";
        public string VerticalAxis = "Vertical";
        public string HorizontalViewAxis = "HorizontalView";
        public string VerticalViewAxis = "VerticalView";
        public GameObject fireBallPrefab;
        public GameObject iceSpikePrefab;
        public RectTransform powerBar;
        public GameObject powerCanvas;
        public GameObject cameraPrefab;
	    public GameObject playerCamera;
        public float speed;
        private float invicibilityCounter;
        private float flashCounter;

        public Rigidbody body;

        public Renderer playerRender;
        public bool eating;
        public bool isLocal;
        public Vector3 spawnPoint;
        
        /* 
        void Start ()
        {
            body = GetComponent<Rigidbody>();
            Anim = GetComponent<Animator>();
            powerCanvas.SetActive(false);
            playerState = -1; //no tiene poderes
            eating = false;
            TriggerPressed = false;
            new IdleState(this);
        }*/

        public override void OnStartLocalPlayer()
        {
            playerCamera = Instantiate(cameraPrefab, Vector3.zero, Quaternion.identity);
            isLocal = true;
            body = GetComponent<Rigidbody>();
            Anim = GetComponent<Animator>();
            MusicSource = playerCamera.GetComponent<AudioSource>();
            powerCanvas = GameObject.Find("BarCanvas");
            powerBar = GameObject.Find("FrontBar").GetComponent<Image>().rectTransform;
            powerCanvas.SetActive(false);
            playerState = -1; //no tiene poderes
            eating=false;
            TriggerPressed = false;
            spawnPoint = transform.position;
            new IdleState(this);
        }

        public void Spawn()
        {
            playerCamera.transform.position = Vector3.zero;
            transform.position = spawnPoint;
        }

        void Update ()
        {
            if (!isLocalPlayer)
            {
                return;
            }
            Mdirection = new Vector3(Input.GetAxis(HorizontalViewAxis), 0f, Input.GetAxis(VerticalViewAxis));
            playerDeltaTime = Time.deltaTime;
            if (Input.GetAxis("Attack") == 0 || Input.GetButtonUp("Attack"))
            {
                TriggerPressed = false;
            }
            ActionHandler();
        }

        void FixedUpdate()
        {
            if (!isLocalPlayer)
            {
                return;
            }
            if (PhysicsHandler != null)
            {
                PhysicsHandler();
            };

        }

        public void takeDmg(Vector3 _hitDirection)
        {
            if (!isLocalPlayer)
            {
                return;
            }
            if (invicibilityCounter < 0 && !eating)
            {
                invicibilityCounter = 1;
                MusicSource.PlayOneShot(hurtSound, 1F);
                body.velocity = Vector3.zero;
                body.AddForce(_hitDirection * 3f, ForceMode.VelocityChange);
                transform.forward = _hitDirection;
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

        public void StartOtherCoroutine(IEnumerator coroutineMethod)
        {
            StartCoroutine(coroutineMethod);
        }
    }
}
