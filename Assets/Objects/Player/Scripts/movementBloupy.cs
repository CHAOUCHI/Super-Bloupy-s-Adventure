using System;
using System.Linq.Expressions;
using System.Numerics;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;

public class movementBloupy : MonoBehaviour
{
    public float maxSpeed = 9f;        //Maximum speed player can go
    public float acceleration = 55f;   //Acceleration rate of the player

    public float jumpStrenghtY = 15f;   //Jump strenght rate of the player
    public float jumpStrenghtX = 15f;   //Jump strenght rate of the player
    public float neutralJumpStrenght = 15f;   //Jump strenght rate of the player
    public float superJumpStrenghtX = 8f;
    public float superJumpStrenghtY = 20f;
    public float slidePlafond = 4.5f;
    public float neutralJumpMinimumHolding = 0.15f;

    public float aircontrol = 0f;     //Air control rate, How fast he can move when he is in the air 

    public float swipeAcceptance = 0f;
    public float doubleTapAcceptance = 0f;

    private Rigidbody2D rb;              //Player's rigidBody
    private Vector2 firstPosBloupy;     //First position of the player
    private bool isGrounded = false;    //If true the player is on the ground
    private bool isJumping = false;
    private bool isClassicJumping = false;
    private bool isSuperJumping = false;
    private bool isNeutralJumping = false;
    private bool isPrepaNeutralJumping = false;
    private bool isSplashMur = false;
    private bool isFalling = false;
    private SpriteRenderer sr;
    private touchHandle touchHandle;
    private Touch[] touches;
    private Touch lastTouch;
    private Vector2 originalPos;
    private float firstTouchTimeRelease;
    private float lastTouchTimeRelease;
    private float firstTouchTimePress;
    private float lastTouchTimePress;
    private Vector2 firstPosTouch;

    private bool inputPressed = false;
    private bool isHeadTriggered = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        firstPosBloupy = transform.position;            //Init player's first position
        rb.freezeRotation = true;             //Setting on or off the rotation of the player
        sr = GetComponent<SpriteRenderer>();
        touchHandle = GameObject.Find("Bloupy").GetComponent<touchHandle>();

    }

    private void Update()
    {
        Debug.Log(rb.velocity.magnitude);
        touches = touchHandle.GetTouches();
        if (Input.touchCount > 0)
        {
            lastTouch = touchHandle.GetLastTouch();
          //  InputHandler();
        }
    }

    private void FixedUpdate()
    {
       // NeutralJumpHandler();
        SlidePlafondHandler();
        //AirPositionHandle();
        FallingHandler();

       // HandleHorizontalMovementsPC();
      //  HandleJumpPC();
    }

    
    private void InputHandler()
    {
       /* HANDLE most recent touch---------------------------------------------------------- -*/
        switch (lastTouch.phase)
        {
            case TouchPhase.Began:

                firstPosTouch = lastTouch.position;
                break;

            case TouchPhase.Moved:

                if (
                    Mathf.Abs(PosTouchRatioX(firstPosTouch.x) - PosTouchRatioX(lastTouch.position.x)) > swipeAcceptance
                    ||
                    Mathf.Abs(PosTouchRatioY(firstPosTouch.y) - PosTouchRatioY(lastTouch.position.y)) > swipeAcceptance
                    )
                {
                    if (touchHandle.IsInLeftSide(lastTouch.position))
                    {
                        // TO DO SLIDE LEFT
                       // isPrepaNeutralJumping = false;
                        inputPressed = true;

                        if (isGrounded)
                        {
                            if (rb.velocity.magnitude >= maxSpeed)
                            {
                                isSuperJumping = true;
                                SuperJump(-1);
                                break;
                            }
                            isClassicJumping = true;
                            Jump(-1);
                            break;
                        }
                    }

                    if (touchHandle.IsInRightSide(lastTouch.position))
                    {
                        // TO DO SLIDE RIGHT
                       // isPrepaNeutralJumping = false;
                        inputPressed = true;
                        if (isGrounded)
                        {
                            if (rb.velocity.magnitude >= maxSpeed)
                            {
                                isSuperJumping = true;
                                SuperJump(1);
                                break;
                            }
                            isClassicJumping = true;
                            Jump(1);
                            break;
                        }
                    }
                }
                break;

            case TouchPhase.Stationary:
                if (Input.touchCount > 1 && touches[Input.touchCount - 2].phase == TouchPhase.Stationary)
                {
                    //DOUBLE PRESS
                    // Debug.Log("DOUBLE PRESS");
                    // Debug.Log("DOUBLE PRESS WITHIN double Tap Acceptance(" + doubleTapAcceptance + ")  tlast: " + lastTouchTimePress + "  tfirst: " + firstTouchTimePress);

                    if (
                        (touchHandle.IsInLeftSide(lastTouch.position) && touchHandle.IsInRightSide(touches[Input.touchCount - 2].position))
                        ||
                        (touchHandle.IsInRightSide(lastTouch.position) && touchHandle.IsInLeftSide(touches[Input.touchCount - 2].position))
                        &&
                        Mathf.Abs(lastTouchTimePress - firstTouchTimePress) <= doubleTapAcceptance
                        )
                    {
                        //TO DO DOUBLE PRESS
                      //  Debug.Log("DOUBLE PRESS WITHIN double Tap Acceptance(" + doubleTapAcceptance + ")  tlast: " + lastTouchTimePress + "  tfirst: " + firstTouchTimePress);
                        // inputPressed = true;
                        // DoubleTapPressHandler();
                        break;
                    }
                }

                if (touchHandle.IsInLeftSide(lastTouch.position))
                {
                    //TO DO PRESS LEFT
                    //isPrepaNeutralJumping = false;
                    inputPressed = true;
                    if (!isGrounded)
                    {
                        if (rb.velocity.y < 0)// AIR CONTROL LEFT
                        {

                            if (transform.localScale.x > 0 && transform.position.x > originalPos.x)     //sprite to the right
                            {
                                Move(-Mathf.Abs(aircontrol));
                            }
                            break;
                        }
                    }
                    else
                    {
                        MoveLeft();
                    }
                    break;
                }
                if (touchHandle.IsInRightSide(lastTouch.position))
                {
                    //TO DO PRESS RIGHT
                   // isPrepaNeutralJumping = false;
                    inputPressed = true;
                    if (!isGrounded)
                    {
                        if (rb.velocity.y < 0)  // AIR CONTROL RIGHT
                        {
                            if (transform.localScale.x < 0 && transform.position.x < originalPos.x)     //sprite to the left
                            {
                                Move(Mathf.Abs(aircontrol));
                            }
                            break;
                        }
                    }
                    else
                    {
                        MoveRight();
                    }
                    break;
                }
                break;

            case TouchPhase.Ended:
                lastTouchTimeRelease = Time.time;

                /*
                if (Mathf.Abs(touchTimeReleaseLeft - touchTimeReleaseRight) <= doubleTapAcceptance)
                {
                    if ((Time.time - touchTimePressRight > neutralJumpMinimumHolding) || (Time.time - touchTimePressLeft > neutralJumpMinimumHolding))
                    {
                        DoubleTapReleaseHandler();
                    }
                    isPrepaNeutralJumping = false;
                    break;
                }


                //touchTimesRelease[0] = Time.time;

                if (Input.touchCount > 1)
                {
                    if (
                        (touchHandle.IsInLeftSide(lastTouch.position) && touchHandle.IsInRightSide(touches[Input.touchCount - 2].position))
                        ||
                        (touchHandle.IsInLeftSide(touches[Input.touchCount - 2].position) || touchHandle.IsInRightSide(lastTouch.position))
                        &&
                        Mathf.Abs(touchTimesPress[0] - touchTimesPress[1]) <= doubleTapAcceptance
                        )
                    {
                        lastTouchTimeRelease = Time.time;

                        if (Mathf.Abs(lastTouchTimeRelease - firstTouchTimeRelease) <= doubleTapAcceptance)
                        {
                            DoubleTapReleaseHandler();
                        }
                        break;
                    }
                    isPrepaNeutralJumping = false;
                    break;
                }

                if (Time.time - firstTouchTimeRelease < doubleTapAcceptance)
                {
                    // DOUBLE TAPP RELEASE
                    DoubleTapReleaseHandler();
                    break;
                }
                if (touchHandle.IsInLeftSide(lastTouch.position))
                {
                    //TO DO RELEASE LEFT
                    //    Debug.Log("RELEASE LEFT ");
                    isPrepaNeutralJumping = false;
                    break;
                }
                if (touchHandle.IsInRightSide(lastTouch.position))
                {
                    //TO DO RELEASE RIGHT
                    //   Debug.Log("RELEASE RIGHT ");
                    isPrepaNeutralJumping = false;

                    break;
                }*/
                break;
            case TouchPhase.Canceled:
                break;
            default:
                inputPressed = false;
                break;
        }
    }

    private void SplashMurAerienHandler() { 
    }


    private void NeutralJumpHandler()
    {
        if (
            Math.Abs(touchHandle.lefttouch.pressTime - touchHandle.righttouch.pressTime) < doubleTapAcceptance
            &&
            touchHandle.lefttouch.currenttouch.phase == TouchPhase.Stationary
            &&
            touchHandle.righttouch.currenttouch.phase == TouchPhase.Stationary
            )
        {
            isPrepaNeutralJumping = true;
        }
    }

    public float PosTouchRatioX(float posx)
    {
        return (posx*100/Screen.width);
    }

    public float PosTouchRatioY(float posy)
    {
        return (posy*100/Screen.height);
    }

    private void DoubleTapPressHandler()
    {
        isPrepaNeutralJumping = true;
    }

    private void DoubleTapReleaseHandler()
    {
        //NEUTRAL JUMP
        isPrepaNeutralJumping = false;
        NeutralJump();
       // isPrepaNeutralJumping = false;
      //  isNeutralJumping = true;
      //  NeutralJump();
    }

    private void SlidePlafondHandler()
    {
        if (isHeadTriggered )
        {
           // Debug.Log("isHeadTriggered && !isGrounded");
            if (transform.localScale.x > 0)     
            {
                //Bloupy face to the right
              //  Debug.Log("SLide plafond right");
                rb.velocity = new Vector2(slidePlafond, 0f);
            }
            else
            {
                //Bloupy face to the left
              //  Debug.Log("SLide plafond left");

                rb.velocity = new Vector2(-slidePlafond, 0f);

            }
        }
    }

    public void MoveLeft()
    {
        // sr.flipX = true;    //Flip the sprite
        //Max velocity check + movement
        if (rb.velocity.magnitude < maxSpeed && isGrounded)
        {
            rb.AddForce(new Vector2(-acceleration, 0));
        }
    }
    public void MoveRight()
    {
        // sr.flipX = false;    //Don't flip the sprite
        //Max velocity check + movement
        if (rb.velocity.magnitude < maxSpeed && isGrounded) rb.AddForce(new Vector2(acceleration, 0));

    }
    private void Move(float force)
    {
        // sr.flipX = false;    //Don't flip the sprite
        //Max velocity check + movement
        if (rb.velocity.magnitude < maxSpeed) rb.AddForce(new Vector2(force, 0));
        //if (rb.velocity.magnitude > maxSpeed) rb.velocity. = maxSpeed;
    }

    private void AirPositionHandle()
    {
        if (!isGrounded
            && transform.localScale.x > 0   //BLOUPY fait face a droite
            && transform.position.x <= originalPos.x        //Bloupy atteint le recule max vers la gauche
            && rb.velocity.x < 0                            //Bloupy va a gauche
            && isNeutralJumping == false                    //Bloupy ne fait pas un Neutral Jump
            && isFalling == false)                          //Bloupy ne tombe pas
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
        if (!isGrounded
            && transform.localScale.x < 0   //BLOUPY va a gauche
            && transform.position.x >= originalPos.x
            && rb.velocity.x > 0
            && isNeutralJumping == false
            && isFalling == false)
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
    }

    public void Jump(int direction)
    {
        if (!isGrounded) return;
        originalPos = transform.position;
        rb.velocity = new Vector2(0f, 0f);
        if (direction < 0)   //LEFT
            rb.velocity = new Vector2(-(jumpStrenghtX), jumpStrenghtY);
        else                //RIGHT
            rb.velocity = new Vector2(jumpStrenghtX, jumpStrenghtY);
        isJumping = true;
    }

    public void NeutralJump()
    {
        if (isGrounded)
        {
            originalPos = transform.position;
            rb.velocity = new Vector2(0f, 0f);
            rb.velocity = new Vector2(0, neutralJumpStrenght);
            isNeutralJumping = true;
            isPrepaNeutralJumping = false;
            isJumping = true;
        }
    }

    private void SuperJump(int direction)
    {
        originalPos = transform.position;
        rb.velocity = new Vector2(0f, 0f);
        if (direction < 0)   //LEFT
            rb.velocity = new Vector2(-(superJumpStrenghtX), superJumpStrenghtY);
        else                //RIGHT
            rb.velocity = new Vector2(superJumpStrenghtX, superJumpStrenghtY);
        isJumping = true;
    }

    private void FallingHandler()
    {
        if (!isGrounded && !isJumping)
        {
            isFalling = true;
        }
    }

    public void ResetPosition()
    {
        transform.SetPositionAndRotation(firstPosBloupy, new Quaternion(0, 0, 0, 0));
        rb.velocity = new Vector2(0f, 0f);
    }


    //FORMER METHODS------------------------------------*/
    private void HandleHorizontalMovementsPC()
    {
        //PC MOUVEMENT
        //Run --------------------------------------------------------------------------------------------------------//
        float x_movement = Input.GetAxis("Horizontal");         //if the player move horizontal  x_movement != 0f

        if (x_movement > 0)     // player go right
        {

            inputPressed = true;
            MoveRight();
        }
        else if (x_movement < 0)     // player go left
        {

            inputPressed = true;
            MoveLeft();
        }
        //Annulation de l'inertie HORIZONTAL
        if (Input.GetButtonUp("Horizontal"))
        {
            inputPressed = true; 
            rb.AddForce(new Vector2(-rb.velocity.magnitude, 0));
        }
    }

    private void HandleJumpPC()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            isPrepaNeutralJumping = false;

            inputPressed = true;

            isClassicJumping = true;

            Jump((int)(transform.localScale.x));
        }
    }
    /*
    private void HandleResetPC()
    {
        if (Input.GetButton("Reset"))
        {
            ResetPosition();
        }
    }*/
    /*FORMER METHODS---------------------------------------------------------------------------------------*/

    public Rigidbody2D GetRigidbody2D() { return rb; }

    public float GetVelocityX() { return rb.velocity.x; }

    public float GetVelocityY() { return rb.velocity.y; }

    public void SetIsGrounded(bool b) { isGrounded = b; }
    public bool GetIsGrounded() { return isGrounded; }

    public void SetIsJumping(bool b) { isJumping = b; }
    public bool GetIsJumping() { return (isJumping || isNeutralJumping || isClassicJumping || isSuperJumping); }
    public void SetIsNeutralJumping(bool b) { isNeutralJumping = b; }
    public bool GetIsNeutralJumping() { return isNeutralJumping; }
    public void SetIsSplashMur(bool b) { isSplashMur = b; }
    public bool GetIsSplashMur() { return isSplashMur; }
    public bool GetIsPrepaNeutralJumping() { return isPrepaNeutralJumping; }
    public void SetIsPrepaNeutralJumping(bool b) { 
        isPrepaNeutralJumping = b; 
        
    }
    public void SetIsClassicJumping(bool b) { isClassicJumping = b; }
    public bool GetIsClassicJumping() { return isClassicJumping; }
    public void SetIsSuperJumping(bool b) { isSuperJumping = b; }
    public bool GetIsSuperJumping() { return isSuperJumping; }
    
    public void SetIsHeadTriggered(bool b)
    {
        isHeadTriggered = b;
    }
    public bool GetIsHeadTriggered()
    {
        return isHeadTriggered;
    }

}
