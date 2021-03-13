using System;
using System.Collections;
using UnityEngine;


public class LeftScreenHandle : MonoBehaviour
{

    private RightScreenHandle rightTouch;
    private touchHandle touchHandle;
    private movementBloupy movementBloupy;

    public float pressTime;
    public float releaseTime;
    public float superJumpSpeedCap;

    public Vector2 pressPos;

    public Touch currenttouch;
    // Use this for initialization+

    void Start()
    {
        rightTouch = GameObject.Find("Bloupy").GetComponent<RightScreenHandle>();
        touchHandle = GameObject.Find("Bloupy").GetComponent<touchHandle>();
        movementBloupy = GameObject.Find("Bloupy").GetComponent<movementBloupy>();

    }

    // Update is called once per frame
    void Update()
    {

        superJumpSpeedCap = rightTouch.superJumpSpeedCap;       //left touch superJumpSpeedCap is the same than rightTouch.superJumpSpeedCap  

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
                         //   Debug.Log("LEFT BEGAN");
                            pressTime = Time.time;
                            pressPos = it.position;

                            break;

                        case TouchPhase.Moved:
                         //   Debug.Log("LEFT MOVED");
                            // DoubleTapHandler();

                            /*SWIPE HANDLER---------------------------------------------------*/
                            float xZero = movementBloupy.PosTouchRatioX(pressPos.x);
                            float xUn = movementBloupy.PosTouchRatioX(it.position.x);

                            float yZero = movementBloupy.PosTouchRatioX(pressPos.y);
                            float yUn = movementBloupy.PosTouchRatioX(it.position.y);

                            float deltaX = Math.Abs(xUn - xZero);
                            float deltaY = Math.Abs(yUn - yZero);

                            float swipe = (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY);

                            if (swipe > movementBloupy.swipeAcceptance)
                            {
                                if (movementBloupy.GetSpeed() < movementBloupy.maxSpeed* superJumpSpeedCap)
                                {
                                    movementBloupy.Jump(-1);
                                }
                                else
                                {
                                    movementBloupy.SuperJump(-1);
                                }
                            }
                            else
                            {
                                movementBloupy.MoveLeft();
                            }
                            /*------------------------------------------------------------------*/


                            break;

                        case TouchPhase.Stationary:
                         //   Debug.Log("LEFT STATIONARY");
                            //  DoubleTapHandler();
                            movementBloupy.MoveLeft();
                            break;

                        case TouchPhase.Ended:
                          //  Debug.Log("LEFT ENDED");
                            releaseTime = Time.time; 
                            /*
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
                            */
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
            Math.Abs(pressTime - rightTouch.pressTime) < movementBloupy.doubleTapAcceptance
            &&
            (
                rightTouch.currenttouch.phase == TouchPhase.Stationary
                ||
                rightTouch.currenttouch.phase == TouchPhase.Moved
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
            Debug.Log("LEFT DOUBLE TAP TIME :" + Math.Abs(pressTime - rightTouch.pressTime));
            movementBloupy.GetRigidbody2D().velocity = new Vector2(0f, 0f);
        }
    }

}