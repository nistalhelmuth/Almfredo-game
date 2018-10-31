using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {


	public Transform camera;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		//Mathf.RoundToInt(10.0f)
		//print(player.position.x);
		
		if (Input.GetKeyUp(KeyCode.UpArrow))
		{
				transform.position = transform.position + new Vector3(0f,0f,20f);
		}
		if (Input.GetKeyUp(KeyCode.RightArrow))
		{
				transform.position = transform.position + new Vector3(38f,0f,0f);
		}
		
	}
}
