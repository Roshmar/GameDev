using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    // private bool isGamePaused;
    public void Resume()
    {
        // isGamePaused = false;
        MenuManager.Instance.ClosePauseMenu();
        Time.timeScale = 1f;
    }
    public void Pause()
    {
        MenuManager.Instance.OpenPauseMenu();
        // isGamePaused = true;
        Time.timeScale = 0f;
    }
}
