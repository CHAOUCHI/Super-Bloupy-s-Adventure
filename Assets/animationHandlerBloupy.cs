using System.Dynamic;
using UnityEngine;
using UnityEngine.UIElements;

public class animationHandlerBloupy : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private float noActionT;

    private float x_movement;

    public float dragDecleration;
    private GameObject bloupy;
    private movementBloupy movementBloupy;
    private touchHandle touchHandle;
    private Touch lastTouch;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        bloupy = GameObject.Find("Bloupy");
        movementBloupy = bloupy.GetComponent<movementBloupy>();
        touchHandle = GameObject.Find("Bloupy").GetComponent<touchHandle>();
    }

    void FixedUpdate()
    {
        //x_movement = Input.GetAxis("Horizontal");         //Get horizontal input of the user
        if (Input.touchCount > 0)
        {
            lastTouch = touchHandle.GetLastTouch();


        }
        SpeedHandler();
        NoActionHandler();
        JumpHandlerMobile();
        HorizontalInputHandlerMobile();

        SplashMurHandler();
        SplashMurAerienHandler();

        // HorizontalInputHandlerPC();

        // FlipSpriteHandlerPC();
        FlipSpriteHandlerMobile();

        //  BrakeHandlerPC();
        BrakeHandlerMobile();
        
        
    }

    private void SplashMurAerienHandler()
    {

    }

    private void SplashMurHandler()
    {
        animator.SetBool("splashMur", movementBloupy.GetIsSplashMur());
    }

    private void SpeedHandler()
    {
        //Share to the animator Tspeed of bloupy-----------------//
       // Debug.Log(rb.velocity.magnitude);
        animator.SetFloat("speed", rb.velocity.magnitude);
    }

    private void HorizontalInputHandlerPC()
    {
        //Player have send horizontal input ?--------------------------//
        if (x_movement != 0)
            animator.SetBool("movementInput", true);             //YES      
        else
            animator.SetBool("movementInput", false);           //NO
    }

    private void HorizontalInputHandlerMobile()
    {
        //Player have send horizontal input ?--------------------------//
        if (Input.touchCount != 0)
            animator.SetBool("movementInput", true);
        else
            animator.SetBool("movementInput", false);
    }

    private void FlipSpriteHandlerPC()
    {
        if (x_movement > 0 && rb.velocity.x > 0)        //bloupy face to the right
            sr.flipX = false;
        if (x_movement < 0 && rb.velocity.x < 0)        //bloupy face to the left
            sr.flipX = true;
    }
    private void FlipSpriteHandlerMobile()
    {
        if (
            touchHandle.IsInRightSide(lastTouch.position) 
            &&
            rb.velocity.x > 0 && movementBloupy.GetIsGrounded()
            )        //bloupy face to the right
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        if (touchHandle.IsInLeftSide(lastTouch.position) && rb.velocity.x < 0 && movementBloupy.GetIsGrounded())        //bloupy face to the left
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    private void BrakeHandlerPC()
    {
        if (
            (x_movement > 0 && rb.velocity.x < 0)       //brake the the right
            || 
            (x_movement < 0 && rb.velocity.x > 0)       //brake to the left
            )
            animator.SetBool("reverseVectorInput", true);
        else
            animator.SetBool("reverseVectorInput", false);
    }
    private void BrakeHandlerMobile()
    {
        if (
            (touchHandle.IsInRightSide(lastTouch.position) && rb.velocity.x < 0)       //brake the the right
            ||
            (touchHandle.IsInLeftSide(lastTouch.position) && rb.velocity.x > 0)       //brake to the left
            )
            animator.SetBool("reverseVectorInput", true);
        else
            animator.SetBool("reverseVectorInput", false);
    }

    private void NoActionHandler()
    {
        if (Input.anyKey)       //bLOUPY IS MOVING
        { 
            noActionT = Time.time;
        }
        else                       //Bloupy is steady
        {
            animator.SetFloat("timeNoAction", Time.time - noActionT);
        }
    }

    private void JumpHandlerPC()
    {
        animator.SetBool("inputJump", Input.GetButtonDown("Jump"));
        animator.SetBool("grounded", movementBloupy.GetIsGrounded());
    }
    private void JumpHandlerMobile()
    {
        animator.SetBool("grounded", movementBloupy.GetIsGrounded());
        animator.SetBool("inputJump", movementBloupy.GetIsJumping());      
        animator.SetBool("classicJump", movementBloupy.GetIsClassicJumping());      
        animator.SetBool("neutralJump", movementBloupy.GetIsNeutralJumping());      
        animator.SetBool("prepaNeutralJump", movementBloupy.GetIsPrepaNeutralJumping());
        animator.SetBool("superJump", movementBloupy.GetIsSuperJumping());
        animator.SetBool("isHeadache", movementBloupy.GetIsHeadTriggered());
    }



}
