using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeManager : Singleton<SwipeManager>
{

    public enum Direction { Left, Right, Up, Down }; // Enum of Direction
    private bool[] swipe = new bool[4];
    private Vector2 startTouch;
    private bool touchMoved;
    private Vector2 swipeDelta; // Length of swipe
    const float SWIPE_THRESHOLD = 30;

    public delegate void MoveDelegate(bool[] swipe);
    public MoveDelegate MoveEvent;

    public delegate void ClickDelegate(Vector2 pos);
    private ClickDelegate ClickEvent;

    private Vector2 TouchPosition() { return (Vector2)Input.mousePosition; }
    private bool TouchBegan() { return Input.GetMouseButtonDown(0); }
    private bool TouchEnded() { return Input.GetMouseButtonUp(0); }
    private bool GetTouch() { return Input.GetMouseButton(0); }

    // Update is called once per frame
    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return; //If our cursor is over any ui object then our swipeManager will not work

        if (TouchBegan())
        {
            startTouch = TouchPosition();
            touchMoved = true;
        }   // strart touching
        else if (TouchEnded() && touchMoved == true)
        {
            SendSwipe();
            touchMoved = false;
        }   //End of touching

        swipeDelta = Vector2.zero;
        if (touchMoved && GetTouch())
        {
            swipeDelta = TouchPosition() - startTouch;
        }   //Calculate distance of swipe

        if (swipeDelta.magnitude > SWIPE_THRESHOLD)
        {
            if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
            {
                //Left/Right swipe
                swipe[(int)Direction.Left] = swipeDelta.x < 0;
                swipe[(int)Direction.Right] = swipeDelta.x > 0;
            }
            else
            {
                //Down/Up swipe
                swipe[(int)Direction.Down] = swipeDelta.y < 0;
                swipe[(int)Direction.Up] = swipeDelta.y > 0;
            }
            SendSwipe();
        }//Check swipe
    }

    private void SendSwipe()
    {
        if (swipe[0] || swipe[1] || swipe[2] || swipe[3])
        {
            MoveEvent?.Invoke(swipe); // if MoveEvent not null send swipe to MoveEvent
        }
        else
        {
            ClickEvent?.Invoke(TouchPosition());
        }
        Reset();
    }

    private void Reset()
    {
        startTouch = swipeDelta = Vector2.zero;
        touchMoved = false;
        for (int i = 0; i < 4; i++)
        {
            swipe[i] = false;
        }
    }
}
