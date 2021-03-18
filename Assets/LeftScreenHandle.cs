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
    void FixedUpdate    ()
    {

        superJumpSpeedCap = rightTouch.superJumpSpeedCap;       //left touch superJumpSpeedCap is the same than rightTouch.superJumpSpeedCap  
        InputHandler();
       // DoubleTapHandler();
        //DoubleReleaseHandler();
    }

    void DoubleReleaseHandler()
    {
        if (
            Math.Abs(releaseTime - rightTouch.releaseTime) < movementBloupy.doubleTapAcceptance
            &&
            movementBloupy.GetIsPrepaNeutralJumping()
            &&
            Math.Abs(pressTime - releaseTime) >= movementBloupy.neutralJumpMinimumHolding
            &&
            Math.Abs(rightTouch.pressTime - rightTouch.releaseTime) >= movementBloupy.neutralJumpMinimumHolding
            )
        {
            movementBloupy.NeutralJump();
        }
    }

    /*
    RETURN VALUE : IsPrepaNeutralJumping  
    */
    Boolean DoubleTapHandler()
    {

        if (
            Math.Abs(pressTime - rightTouch.pressTime) < movementBloupy.doubleTapAcceptance  //DOUBLE TAP ACCEPTANCE
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
            &&
            movementBloupy.GetIsGrounded()
            )
        {
            //DOUBLE TAP WITHIN DOUBLE TAP ACCEPTANCE

            movementBloupy.SetIsPrepaNeutralJumping(true);
           // Debug.Log("LEFT DOUBLE TAP TIME :" + Math.Abs(pressTime - rightTouch.pressTime));
            movementBloupy.GetRigidbody2D().velocity = new Vector2(0f, 0f);
        }
        else
        {
            movementBloupy.SetIsPrepaNeutralJumping(false);
        }

        return movementBloupy.GetIsPrepaNeutralJumping();

    }

    void InputHandler()
    {
        if (Input.touchCount != 0)
        {
            foreach (var it in Input.touches)
            {
                if (touchHandle.IsInLeftSide(it.position))
                {
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
                            //      Debug.Log("LEFT MOVED");
                            if (!movementBloupy.GetIsGrounded())
                            {
                                if (movementBloupy.rb.velocity.y < 0)  //BLOUPY IS FALLING
                                {

                               //     if (transform.localScale.x < 0 && transform.position.x > movementBloupy.originalPos.x)     //sprite to the right
                                 //   {
                                        // AIR CONTROL LEFT
                                        movementBloupy.Move(-Mathf.Abs(movementBloupy.aircontrol));
                                //    }
                                    break;
                                }
                            }
                            if (movementBloupy.GetIsPrepaNeutralJumping()) break;

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
                                if (movementBloupy.GetSpeed() < movementBloupy.maxSpeed * superJumpSpeedCap)
                                {
                                    movementBloupy.Jump(-1);
                                }
                                else
                                {
                                    movementBloupy.SuperJump(-1);
                                }
                            }
                            else    //move left
                            {
                                movementBloupy.MoveLeft();
                            }
                            /*------------------------------------------------------------------*/


                            break;

                        case TouchPhase.Stationary:
                            //   Debug.Log("LEFT STATIONARY");

                            if (!movementBloupy.GetIsGrounded())
                            {
                                if (movementBloupy.rb.velocity.y < 0)  //BLOUPY IS FALLING
                                {

                                    //if (transform.localScale.x < 0 && transform.position.x < movementBloupy.originalPos.x)     //sprite to the left
                                    //{
                                        // AIR CONTROL LEFT
                                        movementBloupy.Move(-Mathf.Abs(movementBloupy.aircontrol));
                                   // }
                                    break;
                                }
                            }

                            if (movementBloupy.GetIsPrepaNeutralJumping()) break;
                            movementBloupy.MoveLeft();
                            break;

                        case TouchPhase.Ended:
                            //  Debug.Log("LEFT ENDED");
                            releaseTime = Time.time;
                            /*
                            if (
                                rightTouch.currenttouch.phase == TouchPhase.Ended
                                &&
                                Math.Abs(rightTouch.pressTime - rightTouch.releaseTime) >= movementBloupy.neutralJumpMinimumHolding
                                &&
                                Math.Abs(pressTime - releaseTime) >= movementBloupy.neutralJumpMinimumHolding
                                &&
                                Math.Abs(rightTouch.releaseTime - releaseTime) <= movementBloupy.doubleTapAcceptance
                                &&
                                movementBloupy.GetIsPrepaNeutralJumping()
                                )
                            {
                                //DOUBLE TAP RELEASE
                                Debug.Log("NeutralJumpLeft");
                                movementBloupy.NeutralJump();
                                return;
                            }
                            //movementBloupy.SetIsPrepaNeutralJumping(false);
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

}