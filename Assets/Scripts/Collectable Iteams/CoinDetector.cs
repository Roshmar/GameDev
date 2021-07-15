using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinDetector : Singleton<CoinDetector>
{
    // Start is called before the first frame update
    private bool isMagnetActive = false;
    private bool isMagnet = false;
  
   
    public void ActivateMagnet(float duration) //Function activate  magnet
    {   
        isMagnetActive = !isMagnetActive;
        StartCoroutine(ActivateMagnetCoroutine(duration));
    }
    IEnumerator ActivateMagnetCoroutine(float duration)
    {
        isMagnet = true;
        yield return new WaitForSeconds(duration);

        if (isMagnetActive)
        {
            isMagnet = false;
        }
        else
        {
            isMagnetActive = !isMagnetActive;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Coin" && isMagnet)
        {
            other.gameObject.GetComponent<CoinController>().CoinMagnetState();
            
        }
    }

}
