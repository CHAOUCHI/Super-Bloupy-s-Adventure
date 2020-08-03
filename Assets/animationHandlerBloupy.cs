using System.Dynamic;
using UnityEngine;

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
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        bloupy = GameObject.Find("Bloupy");
        movementBloupy = bloupy.GetComponent<movementBloupy>();
        movementBloupy.GetIsGrounded();
    }

    void FixedUpdate()
    {
        x_movement = Input.GetAxis("Horizontal");         //Get horizontal input of the user
        JumpHandler();
        NoActionHandler();
        SpeedHandler();
        HorizontalInputHandler();
        FlipSpriteHandler();
        BrakeHandler();
    }

    private void SpeedHandler()
    {
        //Share to the animator Tspeed of bloupy-----------------//
        animator.SetFloat("speed", rb.velocity.magnitude);
    }

    private void HorizontalInputHandler()
    {
        //Player have send horizontal input ?--------------------------//
        if (x_movement != 0)
            animator.SetBool("movementInput", true);             //YES      
        else
        {
            animator.SetBool("movementInput", false);           //NO
        }

        
    }

    private void FlipSpriteHandler()
    {
        if (x_movement > 0 && rb.velocity.x > 0)        //bloupy face to the right
            sr.flipX = false;
        if (x_movement < 0 && rb.velocity.x < 0)        //bloupy face to the left
            sr.flipX = true;
    }

    private void BrakeHandler()
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

    private void NoActionHandler()
    {
        if (Input.anyKey)       //bLOUPY IS MOVING
        { 
            noActionT = Time.time;
            Debug.Log("IF :"+noActionT.ToString());
            //animator.SetFloat("timeNoAction", Time.time - noActionT);
        }
        else                       //Bloupy is steady
        {

            Debug.Log((Time.time - noActionT).ToString());
            animator.SetFloat("timeNoAction", Time.time - noActionT);
        }
    }

    private void JumpHandler()
    {
        animator.SetBool("inputJump", Input.GetButtonDown("Jump"));
        animator.SetBool("grounded", movementBloupy.GetIsGrounded());
    }
}
