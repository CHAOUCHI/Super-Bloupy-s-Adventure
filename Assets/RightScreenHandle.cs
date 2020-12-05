using System;
using System.Collections;
using UnityEngine;


public class RightScreenHandle : MonoBehaviour
{
    public float pressTime;
    public float releaseTime;
    private LeftScreenHandle lefttouch;
    private touchHandle touchHandle;
    private movementBloupy movementBloupy;
    public Touch currenttouch;


    // Use this for initialization
    void Start()
    {
        lefttouch = GameObject.Find("Bloupy").GetComponent<LeftScreenHandle>();
        touchHandle = GameObject.Find("Bloupy").GetComponent<touchHandle>();
        movementBloupy = GameObject.Find("Bloupy").GetComponent<movementBloupy>();

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(movementBloupy.GetIsPrepaNeutralJumping());

        if (Input.touchCount != 0)
        {
            foreach (var it in Input.touches)
            {
                if (touchHandle.IsInRightSide(it.position)) { 
                    currenttouch = it; 
                    //Debug.Log("RIGHT");
                    switch (currenttouch.phase)
                    {
                        case TouchPhase.Began:
                        //     Debug.Log("RIGHT BEGAN");
                            pressTime = Time.time;

                            break;

                        case TouchPhase.Moved:
                          //    Debug.Log("RIGHT MOVED");
                              DoubleTapHandler();

                            break;

                        case TouchPhase.Stationary:
                          //    Debug.Log("RIGHT STATIONARY");
                              DoubleTapHandler();

                            break;

                        case TouchPhase.Ended:
                            releaseTime = Time.time;

                            if (
                                currenttouch.phase == TouchPhase.Ended
                                &&
                                Math.Abs(lefttouch.pressTime - releaseTime) >= movementBloupy.neutralJumpMinimumHolding
                                &&
                                Math.Abs(lefttouch.releaseTime - releaseTime) < movementBloupy.doubleTapAcceptance
                                )
                            {
                                //DOUBLE TAP RELEASE
                                movementBloupy.NeutralJump();
                                return;
                            }
                            movementBloupy.SetIsPrepaNeutralJumping(false);

                            break;

                        case TouchPhase.Canceled:
                          //   Debug.Log("RIGHT CANCELED");

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
            Math.Abs(lefttouch.pressTime - pressTime) < movementBloupy.doubleTapAcceptance
            &&
            (
                lefttouch.currenttouch.phase == TouchPhase.Stationary 
                ||
                lefttouch.currenttouch.phase == TouchPhase.Moved
            )
            &&
            (
                currenttouch.phase == TouchPhase.Stationary
                ||
                currenttouch.phase == TouchPhase.Moved
            )
            )
        {
            movementBloupy.SetIsPrepaNeutralJumping(true);
            Debug.Log("RIGHT DOUBLE TAP TIME :" + Math.Abs(pressTime - lefttouch.pressTime));

            movementBloupy.GetRigidbody2D().velocity = new Vector2(0f, 0f);
        }
        else
        {
            movementBloupy.SetIsPrepaNeutralJumping(false);
        }
    }
}