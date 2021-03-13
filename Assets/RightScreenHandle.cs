using System;
using System.Collections;
using UnityEngine;


public class RightScreenHandle : MonoBehaviour
{
    public float pressTime;
    public float releaseTime;
    public Vector2 pressPos;
    public float superJumpSpeedCap = 0.90F;
    private LeftScreenHandle leftTouch;
    private touchHandle touchHandle;
    private movementBloupy movementBloupy;
    
    public Touch currenttouch;


    // Use this for initialization
    void Start()
    {
        leftTouch = GameObject.Find("Bloupy").GetComponent<LeftScreenHandle>();
        touchHandle = GameObject.Find("Bloupy").GetComponent<touchHandle>();
        movementBloupy = GameObject.Find("Bloupy").GetComponent<movementBloupy>();

    }

    // Update is called once per frame
    void Update()
    {
       // Debug.Log(movementBloupy.GetIsPrepaNeutralJumping());

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
                            // Debug.Log("RIGHT BEGAN");
                            pressTime = Time.time;
                            pressPos = it.position;

                            break;

                        case TouchPhase.Moved:
                           //   Debug.Log("RIGHT MOVED");
                            // DoubleTapHandler();

                            /*SWIPE HANDLER---------------------------------------------------*/
                            float xZero = movementBloupy.PosTouchRatioX(pressPos.x);
                            float xUn = movementBloupy.PosTouchRatioX(it.position.x);

                            float yZero = movementBloupy.PosTouchRatioX(pressPos.y);
                            float yUn = movementBloupy.PosTouchRatioX(it.position.y);

                            float deltaX = Math.Abs(xUn - xZero);
                            float deltaY = Math.Abs(yUn - yZero);

                            float swipe = (float)Math.Sqrt(deltaX* deltaX + deltaY* deltaY);
                            
                            if ( swipe > movementBloupy.swipeAcceptance)
                            {
                                if (movementBloupy.GetSpeed() < movementBloupy.maxSpeed*superJumpSpeedCap)
                                {
                                    movementBloupy.Jump(1);
                                }
                                else
                                {
                                    movementBloupy.SuperJump(1);
                                }

                            }
                            else
                            {
                                movementBloupy.MoveRight();
                            }
                            /*------------------------------------------------------------------*/
                            break;

                        case TouchPhase.Stationary:
                            //  Debug.Log("RIGHT STATIONARY");
                            //  DoubleTapHandler();
                            movementBloupy.MoveRight();

                            break;

                        case TouchPhase.Ended:
                          //  Debug.Log("RIGHT ENDED");
                            

                            releaseTime = Time.time;
                            /*
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
                            */
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
            Math.Abs(leftTouch.pressTime - pressTime) < movementBloupy.doubleTapAcceptance
            &&
            (
                leftTouch.currenttouch.phase == TouchPhase.Stationary 
                ||
                leftTouch.currenttouch.phase == TouchPhase.Moved
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
            Debug.Log("RIGHT DOUBLE TAP TIME :" + Math.Abs(pressTime - leftTouch.pressTime));

            movementBloupy.GetRigidbody2D().velocity = new Vector2(0f, 0f);
        }
        else
        {
            movementBloupy.SetIsPrepaNeutralJumping(false);
        }
    }
}