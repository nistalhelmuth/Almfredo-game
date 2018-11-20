using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

	public void AddDamage(){
		currentLives -= 1;
		if (currentLives <= 0) {
			//Application.Quit();
		} else {
			LivesHUD.sprite = Lives[currentLives];
		}
	}
}
