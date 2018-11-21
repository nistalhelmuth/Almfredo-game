using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Player;

public class GameManager : MonoBehaviour {

	public int currentSouls;
	public int currentLives;
	public Text soulsText;
	public Sprite[] Lives;
	public Image LivesHUD;

	// Use this for initialization
	void Start () {
		
		currentSouls = 0;
		currentLives = 10;
		soulsText.text =  currentSouls.ToString();
	}
	
	public void AddSouls(){
		currentSouls += 1;
		soulsText.text = currentSouls.ToString();
	}

	public void AddDamage(GameObject player){
		currentLives -= 1;
		if (currentLives < 0) {
			player.GetComponent<PlayerBehaviour>().Spawn();
			currentSouls = 0;
			currentLives = 10;
			soulsText.text =  currentSouls.ToString();
			LivesHUD.sprite = Lives[currentLives];
		} else {
			LivesHUD.sprite = Lives[currentLives];
		}
	}
}
