﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallBehaviour : MonoBehaviour {

	void OnCollisionEnter(Collision collision)
    {
			if (collision.gameObject.tag == "Enemy") 
			{
				Destroy(collision.gameObject);
			}
			Destroy(gameObject);
    }
}
