using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    static public MenuManager instance;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject deathMenu;
    private void Awake() {
        instance = this;
    }
    void Start()
    {
        deathMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
    public void OpenDeathMenu()
    {   
        deathMenu.SetActive(true);
    }
    public void CloseDeathMenu()
    {
        deathMenu.SetActive(false);
    }
}
