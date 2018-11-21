using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Player;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.PostProcessing;

public class GameManager : MonoBehaviour
{

    public int currentSouls;
    public int currentLives;
    public Text soulsText;
    public Sprite[] Lives;
    public Image LivesHUD;
    public GameObject NormalCanvas;
    public GameObject ScoreUI;
    public TextMeshProUGUI ScoreText;
    public PostProcessingProfile Profile;
    public AudioSource MainSource;

    // Use this for initialization
    void Start ()
    {

        currentSouls = 0;
        currentLives = 10;
        soulsText.text =  currentSouls.ToString();
    }

    public void AddSouls()
    {
        currentSouls += 1;
        soulsText.text = currentSouls.ToString();
    }

    public void AddDamage(GameObject player)
    {
        currentLives -= 1;
        if (currentLives < 0)
        {
            Time.timeScale = 0;
            NormalCanvas.SetActive(false);
            ScoreUI.SetActive(true);
            ScoreText.text = "Final Score: " + currentSouls.ToString();
            Profile.vignette.enabled = true;
            Profile.colorGrading.enabled = true;
            MainSource.mute = true;
            // player.GetComponent<PlayerBehaviour>().Spawn();
            // currentSouls = 0;
            // currentLives = 10;
            // soulsText.text =  currentSouls.ToString();
            // LivesHUD.sprite = Lives[currentLives];
        }
        else
        {
            LivesHUD.sprite = Lives[currentLives];
        }
    }

    public void ReturnToMenu()
    {
        NetworkManager.singleton.StopClient();
        NetworkManager.singleton.StopHost();
        SceneManager.LoadScene("MainMenu");
    }
}
