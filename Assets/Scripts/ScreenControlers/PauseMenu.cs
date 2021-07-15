using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private bool isGamePaused;
    private void Start() {
        isGamePaused = false;
        gameObject.SetActive(false);
    }
    public void Resume()
    {
        isGamePaused = false;
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Pause()
    {
        gameObject.SetActive(true);
        isGamePaused = true;
        Time.timeScale = 0f;
    }
}
