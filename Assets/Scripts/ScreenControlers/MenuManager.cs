using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : Singleton<MenuManager>
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject deathMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameScreen;

    void Start()
    {
        gameScreen.SetActive(false);
        pauseMenu.SetActive(false);
        deathMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
    public void CloseAllMenu()
    {
        gameScreen.SetActive(false);
        pauseMenu.SetActive(false);
        deathMenu.SetActive(false);
        mainMenu.SetActive(false);
    }

    public void OpenDeathMenu()
    {
        deathMenu.SetActive(true);
        Time.timeScale = 0f;
    }
    public void CloseDeathMenu()
    {
        deathMenu.SetActive(false);
        Time.timeScale = 1f;
    }
    public void OpenPauseMenu()
    {
        pauseMenu.SetActive(true);
    }
    public void ClosePauseMenu()
    {
        pauseMenu.SetActive(false);
    }
    public void OpenMainMenu()
    {
        CloseAllMenu();
        mainMenu.SetActive(true);
        Time.timeScale = 1f;
        PlayerController.Instance.ResetGame();
        // DeathMenu.Instance.Death();
    }
    public void CloseMainMenu()
    {
        mainMenu.SetActive(false);
    }
    public void OpenGameScreen()
    {

        gameScreen.SetActive(true);
    }
    public void CloseGameScreen()
    {
        gameScreen.SetActive(false);
    }
    public void PlayGame()
    {
        CloseAllMenu();
        OpenGameScreen();
        PlayerController.Instance.StartGame();
    }
}
