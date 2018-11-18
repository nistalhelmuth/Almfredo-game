using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

	public class HurtPlayer : MonoBehaviour 
	{
		void Start () 
		{
			
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

	
