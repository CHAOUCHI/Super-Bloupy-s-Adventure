using System;
using System.Collections;
using UnityEngine;


public class LeftScreenHandle : MonoBehaviour
{

    public float pressTime;
    public float releaseTime;
    private RightScreenHandle righttouch;
    private touchHandle touchHandle;
    private movementBloupy movementBloupy;
    public Touch currenttouch;
    // Use this for initialization
    void Start()
    {
        righttouch = GameObject.Find("Bloupy").GetComponent<RightScreenHandle>();
        touchHandle = GameObject.Find("Bloupy").GetComponent<touchHandle>();
        movementBloupy = GameObject.Find("Bloupy").GetComponent<movementBloupy>();

    }

    // Update is called once per frame
    void Update()
    {
       // Debug.Log(movementBloupy.GetIsPrepaNeutralJumping());
       // Debug.Log(movementBloupy.GetIsGrounded());
        if (Input.touchCount != 0)
        {
            foreach (var it in Input.touches)
            {
                if (touchHandle.IsInLeftSide(it.position)) { 
                    currenttouch = it; 
                   // Debug.Log("LEFT");

                    switch (currenttouch.phase)
                    {
                        case TouchPhase.Began:
                           // Debug.Log("LEFT BEGAN");
                            pressTime = Time.time;

                            break;

                        case TouchPhase.Moved:
                         //      Debug.Log("LEFT MOVED");
                            DoubleTapHandler();

                            break;

                        case TouchPhase.Stationary:
                          //      Debug.Log("LEFT STATIONARY");
                            DoubleTapHandler();

                            break;

                        case TouchPhase.Ended:
                            releaseTime = Time.time; 

                            if (
                                righttouch.currenttouch.phase == TouchPhase.Ended
                                &&
                                Math.Abs(pressTime - righttouch.releaseTime) >= movementBloupy.neutralJumpMinimumHolding
                                &&
                                Math.Abs(releaseTime - righttouch.releaseTime) < movementBloupy.doubleTapAcceptance
                                )
                            {
                                //DOUBLE TAP RELEASE
                                movementBloupy.NeutralJump();
                            }
                            movementBloupy.SetIsPrepaNeutralJumping(false);

                            break;

                        case TouchPhase.Canceled:
                           //   Debug.Log("LEFT CANCELED");

                            break;

                        default:
                            break;
                    }

                    break;
                }
            }
            
        }
        
    }

    void DoubleTapHandler()
    {
        if (
            Math.Abs(pressTime - righttouch.pressTime) < movementBloupy.doubleTapAcceptance
            &&
            (
                righttouch.currenttouch.phase == TouchPhase.Stationary
                ||
                righttouch.currenttouch.phase == TouchPhase.Moved
            )
            &&
            (
                currenttouch.phase == TouchPhase.Stationary
                ||
                currenttouch.phase == TouchPhase.Moved
            )
            )
        {
            //DOUBLE TAP WITHIN DOUBLE TAP ACCEPTANCE

            movementBloupy.SetIsPrepaNeutralJumping(true);
            Debug.Log("LEFT DOUBLE TAP TIME :" + Math.Abs(pressTime - righttouch.pressTime));
            movementBloupy.GetRigidbody2D().velocity = new Vector2(0f, 0f);
        }
    }

}