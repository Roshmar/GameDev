using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    private float rotationSpeed = 100;
    public Transform playerTransform;
    Vector3 startPos;
    private bool isCoinMagnet = false;
    private bool isCoinMagnetIntersect = false;
    public float moveSpeed = 17f;
    static public CoinController instance;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        rotationSpeed += Random.Range(0, rotationSpeed / 4.0f);

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        startPos = this.transform.position;
        
    }
    private void Reset()
    {
        isCoinMagnetIntersect = false;
        isCoinMagnet = false;

    }

    // Update is called once per frame

    private void FixedUpdate()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        if (transform.parent.gameObject.activeSelf == true)
        {
            transform.gameObject.SetActive(true);
        }
        if (isCoinMagnet == false)
        {
            Reset();
        }
        if (isCoinMagnetIntersect && isCoinMagnet)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);
        }
    }
    public void CoinMagnetState()
    {
        isCoinMagnet = !isCoinMagnet;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerController.Instance.AddCoin();
            Reset();
            transform.position = transform.parent.position;
            transform.parent.gameObject.SetActive(false);
        }
        if (other.gameObject.tag == "Coin Detector")
        {
            isCoinMagnetIntersect = true;
        }

    }
}
