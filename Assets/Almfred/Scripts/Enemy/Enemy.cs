using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Player;
using UnityEngine.Networking;

namespace Enemies
{
    public class Enemy: NetworkBehaviour
    {
        public int Life;
        public float PlayerDistance;
        //public GameObject DeadSoul;
        protected GameObject playerToFollow;
        protected NavMeshAgent navAgent;
        protected List<Vector3> keyPositions = new List<Vector3>();
        protected Animator anim;

        private bool isQuitting = false;

        protected virtual void Start()
        {
            GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");
            playerToFollow = allPlayers[UnityEngine.Random.Range(0, allPlayers.Length - 1)];
            navAgent = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();
        }

        protected virtual void Update()
        {
            transform.LookAt(playerToFollow.transform.position);
        }

        public void AddKeyPosition(Vector3 position)
        {
            keyPositions.Add(position);
        }

        public Nullable<Vector3> GetRandomKeyPosition()
        {
            if (keyPositions.Count != 0)
            {
                return keyPositions[UnityEngine.Random.Range(0, keyPositions.Count)];
            }
            return null;
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                Vector3 hitDirection = collision.transform.position - transform.position;
                hitDirection = hitDirection.normalized;
                collision.gameObject.GetComponent<PlayerBehaviour>().takeDmg(hitDirection);
            }
        }

        void OnApplicationQuit()
        {
            isQuitting = true;
        }

        public int TakeDamage()
        {
            Life -= 1;
            return Life;
        }

        void OnDestroy()
        {
            /*
            Vector3 offset = new Vector3(transform.position.x, 0.15f, transform.position.z);
            Instantiate(DeadSoul, offset, Quaternion.Euler(0f, 180f, 0f));
            */
        }
    }
}
