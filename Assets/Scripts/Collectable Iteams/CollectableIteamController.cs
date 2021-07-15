using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableIteamController : MonoBehaviour
{
    [SerializeField] private string iteamType;
    float maxDoubleScoreDuration = 10.0f;//Double score duration;
    float maxDoubleJumpDuration = 10.0f;//Double jump duration;
    float maxDoubleCoinDuration = 10.0f;
    float maxMagnetDuration = 10.0f;
    float rotationSpeed = 100;
    public GameObject coinDetectorObj;
  

    void Start()
    {
        rotationSpeed += Random.Range(0, rotationSpeed / 4.0f);


    }

    // Update is called once per frame
    private void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (iteamType == "doubleScore")
            {
                Score.Instance.ActivateDoubleScore(maxDoubleScoreDuration); //Double Score power up
            }
            else if (iteamType == "magnet") // if player pick up magnet
            {
                CoinDetector.Instance.ActivateMagnet(maxMagnetDuration);
            }
            else if (iteamType == "doubleJumping")
            {
                PlayerController.Instance.ActivateDoubleJumping(maxDoubleJumpDuration);//Double Jumping power up
            }
            else if (iteamType == "doubleCoin")
            {
                PlayerController.Instance.ActivateDoubleCoin(maxDoubleCoinDuration); //Double coin power up
            }
            else if (iteamType == "shield")
            {
                Debug.Log("shield");
            }
            transform.parent.gameObject.SetActive(false);
        }
    }
}
