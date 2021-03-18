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
    public float pressedtime = 0;

    // Use this for initialization
    void Start()
    {
        leftTouch = GameObject.Find("Bloupy").GetComponent<LeftScreenHandle>();
        touchHandle = GameObject.Find("Bloupy").GetComponent<touchHandle>();
        movementBloupy = GameObject.Find("Bloupy").GetComponent<movementBloupy>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log(movementBloupy.GetIsPrepaNeutralJumping());
       // Debug.Log(Math.Abs(pressTime - releaseTime));
        InputHandler();
        DoubleTapHandler();
        
    }

    void DoubleReleaseHandler()
    {
        if ( 
            Math.Abs(releaseTime - leftTouch.releaseTime) < movementBloupy.doubleTapAcceptance
            &&
            movementBloupy.GetIsPrepaNeutralJumping()
            &&
            Math.Abs(pressTime - releaseTime) > movementBloupy.neutralJumpMinimumHolding
            &&
            Math.Abs(leftTouch.pressTime - leftTouch.releaseTime) > movementBloupy.neutralJumpMinimumHolding
            )
        {
            Debug.Log("NeutralJUMP");
            movementBloupy.NeutralJump();
        }
    }

    /*
    RETURN VALUE : IsPrepaNeutralJumping  
    */
    Boolean DoubleTapHandler()      
    {   
        if (
            Math.Abs(leftTouch.pressTime - pressTime) < movementBloupy.doubleTapAcceptance  //DOUBLE TAP ACCEPTANCE
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
            &&
            movementBloupy.GetIsGrounded()
            )
        {
            Debug.Log("Set PREPA TO TRUE");
            pressedtime = Math.Abs(pressTime - Time.time);
            movementBloupy.SetIsPrepaNeutralJumping(true);
           // Debug.Log("RIGHT DOUBLE TAP TIME :" + Math.Abs(pressTime - leftTouch.pressTime));

            movementBloupy.GetRigidbody2D().velocity = new Vector2(0f, 0f);
        }
        else
        {
            Debug.Log("Set PREPA TO FALSE");
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
                if (touchHandle.IsInRightSide(it.position))
                {
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
                            // Debug.Log("RIGHT MOVED");

                            if (!movementBloupy.GetIsGrounded())
                            {
                                if (movementBloupy.rb.velocity.y < 0)  //BLOUPY IS FALLING
                                {

                                  //  if (transform.localScale.x > 0 && transform.position.x > movementBloupy.originalPos.x)     //sprite to the right
                                  //  {
                                        // AIR CONTROL RIGHT
                                        movementBloupy.Move(Mathf.Abs(movementBloupy.aircontrol));
                                   // }
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

                            if (!movementBloupy.GetIsGrounded())
                            {
                                if (movementBloupy.rb.velocity.y < 0)  //BLOUPY IS FALLING
                                {

                                //    if (transform.localScale.x < 0 && transform.position.x > movementBloupy.originalPos.x)     //sprite to the right
                                 //   {
                                        // AIR CONTROL LEFT
                                        movementBloupy.Move(Mathf.Abs(movementBloupy.aircontrol));
                                 //   }
                                    break;
                                }
                            }

                            if (movementBloupy.GetIsPrepaNeutralJumping()) break;
                            movementBloupy.MoveRight();

                            break;

                        case TouchPhase.Ended:
                            //  Debug.Log("RIGHT ENDED");
                            releaseTime = Time.time;
                            DoubleReleaseHandler();
                            //   if (DoubleTapHandler()) //IF IS NOT PREPA NEUTRAL JUMP BREAK

                            /*
                            if (
                                leftTouch.currenttouch.phase == TouchPhase.Ended
                                &&
                                Math.Abs(pressTime - releaseTime) >= movementBloupy.neutralJumpMinimumHolding 
                                &&
                                Math.Abs(leftTouch.pressTime - leftTouch.releaseTime) >= movementBloupy.neutralJumpMinimumHolding
                                &&
                                Math.Abs(leftTouch.releaseTime - releaseTime) < movementBloupy.doubleTapAcceptance
                                &&
                                movementBloupy.GetIsPrepaNeutralJumping()
                                )
                            {
                                //DOUBLE TAP RELEASE
                                Debug.Log("NeutralJump right");

                                movementBloupy.NeutralJump();
                                return;
                            }
                            //movementBloupy.SetIsPrepaNeutralJumping(false);
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
}