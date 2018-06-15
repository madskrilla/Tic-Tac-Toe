using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuMB : MonoBehaviour
{
    [SerializeField]
    private GameObject titleSplash = null;
    [SerializeField]
    private GameObject mainMenu = null;
    [SerializeField]
    private GameObject optionsMenu;

    // Use this for initialization
    void Start()
    {
        titleSplash.SetActive(true);
        mainMenu.SetActive(false);
        optionsMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (titleSplash.activeSelf)
        {
            WaitForTouch();
        }
    }

    private void WaitForTouch()
    {
        if (Input.anyKey)
        {
            titleSplash.SetActive(false);
            mainMenu.SetActive(true);
        }
    }

    public void StartTwoPlayerGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OpenOptionsMenu()
    {
        optionsMenu.SetActive(true);
    }
}
