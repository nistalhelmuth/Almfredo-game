using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{

    public delegate void MenuAction();
    public event MenuAction GoToMainMenu;
    public GameObject MenuCanvas;

    // Use this for initialization
    void Start ()
    {
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
}
