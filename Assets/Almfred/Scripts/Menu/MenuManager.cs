using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    public delegate void MenuAction();
    public event MenuAction GoToMainMenu;
    public GameObject MenuCanvas;

    // Use this for initialization
    void Start ()
    {
        Time.timeScale = 1;
        StartCoroutine("MainMenu");
    }

    // Update is called once per frame
    void Update ()
    {

    }

    IEnumerator MainMenu()
    {
        yield return new WaitForSeconds(3f);
        GoToMainMenu();
        StartCoroutine("ShowMenu");
    }

    IEnumerator ShowMenu()
    {
        yield return new WaitForSeconds(1f);
        MenuCanvas.SetActive(true);
    }

    public void StartGame()
    {
        MenuCanvas.SetActive(false);
        SceneManager.LoadScene("AssetTest");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
