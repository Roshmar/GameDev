using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathMenu : MonoBehaviour
{
    // private bool isGameEnd; 
    public void Respawn()
    {
        // isGameEnd = false;
        MenuManager.Instance.CloseDeathMenu();
        Time.timeScale = 1f;
    }
    public void Death()
    {
         MenuManager.Instance.OpenDeathMenu();
        // isGameEnd = true;
        Time.timeScale = 0f;

    }
}
