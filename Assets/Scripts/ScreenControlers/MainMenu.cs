using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour
{
    [SerializeField] private Sprite SoundsOn, SoundsOff;
    [SerializeField] private Image SoundBtnImg;
    public void PlayBth()
    {
        MenuManager.Instance.PlayGame();
    }

    public void QuitBtn()
    {
        Application.Quit();
    }

    public void SoundBtn()
    {
        PlayerController.Instance.SetSound();
        SoundBtnImg.sprite = PlayerController.Instance.GetSound() ? SoundsOn : SoundsOff;
    }
}
