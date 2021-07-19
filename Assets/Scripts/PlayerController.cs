using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    private Animator animator;//Player animator
    private Vector3 startGamePosition;
    private Quaternion startGameRotation;

    private bool isGameEnd = false;
    private bool isSound = true;  // Sound
    private bool isDamage = false; // Is hero intersect with NotLose Tag prefab

    private bool isDoubleJumping = false; // Power up Double Jump
    private bool isDoubleJumpingActive = false;
    private int coinCount = 0; // Coin count

    private bool isDoubleCoin = false;// Power up Double Coin
    private bool isDoubleCoinActive = false;
    private bool isMagnetActive; // Magnet power up
    private float laneOffset;
    private float laneChangeSpeed = 15;
    private Rigidbody rb;
    private float pointStart;
    private float pointFinish;
    private bool isMoving = false; // Is hero Move
    private Coroutine movingCoroutine;
    private float lastVectorX;
    private bool isJumping = false;

    private bool isRolling = false;
    private float jumpPower = 15;
    private float jumpGravity = -40;
    private float realGravity = -9.8f;


    private void Start()
    {
        laneOffset = MapGenerator.Instance._laneOffset;
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        startGamePosition = transform.position;
        startGameRotation = transform.rotation;
        SwipeManager.Instance.MoveEvent += MovePlayer;

    }
    // Update is called once per frame
    private void MovePlayer(bool[] swipes)
    {
        if (swipes[(int)SwipeManager.Direction.Left] && pointFinish > -laneOffset)
        {
            MoveHorizontal(-laneChangeSpeed);
        }
        if (swipes[(int)SwipeManager.Direction.Right] && pointFinish < laneOffset)
        {
            MoveHorizontal(laneChangeSpeed);
        }
        if (swipes[(int)SwipeManager.Direction.Up] && isJumping == false)
        {
            Jump();
        }
        if (swipes[(int)SwipeManager.Direction.Down] && isRolling == false)
        {
            Rolling();
        }
    }
    private void Jump()
    {
        isJumping = true;
        animator.SetTrigger("jumping");

        if (isDoubleJumping)
        {
            rb.AddForce(Vector3.up * (jumpPower + 5), ForceMode.Impulse);
        } // if DoubleJumping true 
        else
        {
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }

        Physics.gravity = new Vector3(0, jumpGravity, 0);
        StartCoroutine(StopjumpCoroutine());
    }
    public void ActivateDoubleJumping(float duration)
    {
        isDoubleJumpingActive = !isDoubleJumpingActive;
        StartCoroutine(StopPowerUpTimerCoroutine(duration));
    }
    IEnumerator StopPowerUpTimerCoroutine(float duration)
    {
        isDoubleJumping = true;
        yield return new WaitForSeconds(duration);

        if (isDoubleJumpingActive)
        {
            isDoubleJumping = false;
        }
        else
        {
            isDoubleJumpingActive = !isDoubleJumpingActive;
        }
    }
    private void Rolling()
    {
        isRolling = true;

        animator.ResetTrigger("jumping");
        animator.SetBool("rolling", true);

        StartCoroutine(StopRollCoroutine());
    }
    IEnumerator StopRollCoroutine()
    {
        var capsuleColider = gameObject.GetComponent("CapsuleCollider") as CapsuleCollider;
        capsuleColider.height = 1;

        Physics.gravity = new Vector3(0, -100, 0);
        yield return new WaitForSeconds(1.5f);
        capsuleColider.height = 2;
        // yield return new WaitForSeconds(0.4f);
        isRolling = false;
        animator.SetBool("rolling", false);
    }
    IEnumerator StopjumpCoroutine()
    {
        do
        {
            if (isRolling)
            {

            }
            yield return new WaitForSeconds(0.02f);
        } while (rb.velocity.y != 0);
        isJumping = false;
        Physics.gravity = new Vector3(0, realGravity, 0);
    }

    private void MoveHorizontal(float speed)
    {

        animator.applyRootMotion = false;
        pointStart = pointFinish;
        pointFinish += Mathf.Sign(speed) * laneOffset;

        if (isMoving)
        {
            StopCoroutine(movingCoroutine);
            isMoving = false;
        }
        movingCoroutine = StartCoroutine(MoveCoroutine(speed));
    }
    IEnumerator MoveCoroutine(float vectorX)
    {
        isMoving = true;
        while (Mathf.Abs(pointStart - transform.position.x) < laneOffset)
        {
            yield return new WaitForFixedUpdate();

            rb.velocity = new Vector3(vectorX, rb.velocity.y, 0);
            lastVectorX = vectorX;

            float x = Mathf.Clamp(transform.position.x, Mathf.Min(pointStart, pointFinish), Mathf.Max(pointStart, pointFinish));
            transform.position = new Vector3(x, transform.position.y, transform.position.z);
        }
        rb.velocity = Vector3.zero;
        transform.position = new Vector3(pointFinish, transform.position.y, transform.position.z);
        if (transform.position.y > 1)
        {
            rb.velocity = new Vector3(rb.velocity.x, -10, rb.velocity.z);
        }
        isMoving = false;
    }
    public void StartGame()
    {
        isGameEnd = false;
        animator.SetTrigger("turnAround");
        // animator.SetTrigger("run");
    }
    public void StartLevel()
    {
        transform.RotateAround(transform.position, transform.up, 180f);
        RoadGenerator.Instance.StartLevel();
    }

    public void ResetGame()
    {
        isGameEnd = true;
        isDamage = false;
        rb.velocity = Vector3.zero;
        pointStart = 0;
        pointFinish = 0;

        // animator.applyRootMotion = true;
        // animator.SetTrigger("standing");
        transform.position = startGamePosition;
        transform.rotation = startGameRotation;
        RoadGenerator.Instance.ResetLevel();
        animator.SetTrigger("endGame");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ramp")
        {
            rb.constraints |= RigidbodyConstraints.FreezePositionZ;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Ramp")
        {
            rb.constraints &= ~RigidbodyConstraints.FreezePositionZ;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        }
        if (collision.gameObject.tag == "NotLose")
        {
            if (isDamage)
            {
                MenuManager.Instance.OpenDeathMenu();
            }
            else
            {
                isDamage = true;
                MoveHorizontal(-lastVectorX);
                StartCoroutine(StopGameCoroutine());
            }

        }
        if (collision.gameObject.tag == "Lose")
        {
            MenuManager.Instance.OpenDeathMenu();
        }
    }

    IEnumerator StopGameCoroutine()
    {
        yield return new WaitForSeconds(5.0f);
        isDamage = false;
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "RampPlane")
        {
            if (rb.velocity.x == 0 && isJumping == false)
            {
                rb.velocity = new Vector3(rb.velocity.x, -10, rb.velocity.z);
            }
        }
    }

    public void AddCoin()
    {
        if (isDoubleCoin)
        {
            coinCount += 2;
        }
        else
        {
            coinCount += 1;
        }
    }
    public int GetCoins()
    {
        return coinCount;
    }
    public void ActivateDoubleCoin(float duration)
    {
        isDoubleCoinActive = !isDoubleCoinActive;
        StartCoroutine(StopDoubleCoinCoroutine(duration));
    }
    IEnumerator StopDoubleCoinCoroutine(float duration)
    {
        isDoubleCoin = true;
        yield return new WaitForSeconds(duration);

        if (isDoubleCoinActive)
        {
            isDoubleCoin = false;
        }
        else
        {
            isDoubleCoinActive = !isDoubleCoinActive;
        }
    }

    public void SetSound()
    {
        isSound = !isSound;
    }
    public bool GetSound()
    {
        return isSound;
    }
    public bool GetIsGameEnd()
    {
        return isGameEnd;
    }
}
