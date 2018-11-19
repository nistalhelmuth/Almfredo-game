using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Player;

namespace Enemies
{
    public class Enemy: MonoBehaviour
    {
        public int Life;

        protected GameObject playerToFollow;
        protected NavMeshAgent navAgent;
        protected List<Vector3> keyPositions = new List<Vector3>();

        void Start()
        {
            GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");
            playerToFollow = allPlayers[Random.Range(0, allPlayers.Length - 1)];
            navAgent = GetComponent<NavMeshAgent>();
        }

        public virtual void Update()
        {
            transform.LookAt(playerToFollow.transform.position);
        }

        public void AddKeyPosition(Vector3 position)
        {
            keyPositions.Add(position);
        }

        public Vector3 GetRandomKeyPosition()
        {
            return keyPositions[Random.Range(0, keyPositions.Count)];
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
    }
}
