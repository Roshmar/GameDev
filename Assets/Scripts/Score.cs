using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : Singleton<Score>
{
    [SerializeField] private Transform player;
    [SerializeField] private Text scoreText; // output score text
    [SerializeField] private Text coinText; // output score text
    private bool isDoubleScore = false;// double score flag
    private bool isDoubleScoreActive = false;

    private float score = 0; // player score
    public void ResetScore()
    {
        score = 0;
        isDoubleScore = false;
    }
    private void FixedUpdate()
    {
        if (RoadGenerator.Instance != null)
        {
            StartCoroutine(StopScoreCoroutine(RoadGenerator.Instance.speed));
        }
        scoreText.text = ((int)(score)).ToString(); // output text score 
        coinText.text = (PlayerController.Instance.GetCoins()).ToString(); //coin count output
    }
    public void ActivateDoubleScore(float duration) //Function activate double score power up
    {
        isDoubleScoreActive = !isDoubleScoreActive;
        StartCoroutine(StopPowerUpTimerCoroutine(duration));
    }
    IEnumerator StopPowerUpTimerCoroutine(float duration)
    {
        isDoubleScore = true;
        yield return new WaitForSeconds(duration);

        if (isDoubleScoreActive)
        {
            isDoubleScore = false;
        }
        else
        {
            isDoubleScoreActive = !isDoubleScoreActive;
        }
    }
    IEnumerator StopScoreCoroutine(float speed)
    {
        if (isDoubleScore)
        {
            score += (speed * 0.01f) * 2;
        }// if true we have x2 speed
        else
        {
            score += (speed * 0.01f);
        }//if false we have ordinary speed

        yield return new WaitForSeconds(0.1f);

    }
}
