using UnityEngine;
using UnityEngine.XR;

public class movementBloupy : MonoBehaviour
{
    public float maxSpeed = 10f;        //Maximum speed player can go
    public float acceleration = 500f;   //Acceleration rate of the player
    public float jumpStrenght = 330f;   //Jump strenght rate of the player
    public float aircontrol = 330f;     //Air control rate, How fast he can move when he is in the air 
    public int nbFrames =0;

    private int bufFrames =0;
    private int framesCount =0;

    private Rigidbody2D rb;              //Player's rigidBody
    private SpriteRenderer sr;           //Player's Sprite Renderer.
    private Animator animator;           //Player's Animator Renderer.
    private Vector2 firstPosBloupy;     //First position of the player
    private bool isGrounded = false;    //If true the player is on the ground




    private void Start()
    {    
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>(); 

        firstPosBloupy = transform.position;            //Init player's first position
        rb.freezeRotation = true;             //Setting on or off the rotation of the player
    }

    private void Update()                          
    {
        //framesCount++;
        
        HandleHorizontalMovements();
        HandleJump();
        HandleReset();
        
    }

    private void HandleHorizontalMovements()
    {
        //Run --------------------------------------------------------------------------------------------------------//
        float x_movement = Input.GetAxis("Horizontal");         //if the player move horizontal  x_movement != 0f

        if (x_movement > 0)     // player go right
        {
            if (bufFrames == 0) bufFrames = framesCount;
            MoveRight(x_movement);
        }
        else if (x_movement < 0)     // player go left
        {
            if(bufFrames ==0)bufFrames = framesCount;
            MoveLeft(x_movement);
        }

        //Idle---------------------------------------------------------------------------------------------------------//
        if (x_movement == 0)    // player don't move
        {
            bufFrames = 0;
            //TO DO
        }

        //Annulation de l'inertie HORIZONTAL
        if (Input.GetButtonUp("Horizontal"))    rb.AddForce(new Vector2(-rb.velocity.magnitude, 0));
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }
    }
    private void HandleReset()
    {
        if (Input.GetButton("Reset"))
        {
            ResetPosition();
        }
    }

    private void MoveLeft(float force) 
    {
       // sr.flipX = true;    //Flip the sprite
        //Max velocity check + movement
        if (rb.velocity.magnitude < maxSpeed) 
        {
            rb.AddForce(new Vector2(force * acceleration * Time.deltaTime, 0));
        }
    }
    private void MoveRight(float force) 
    {
       // sr.flipX = false;    //Don't flip the sprite
        //Max velocity check + movement
        if (rb.velocity.magnitude < maxSpeed)   rb.AddForce(new Vector2(force * acceleration * Time.deltaTime, 0));

    }

    private void Jump() 
    {

        if (rb.velocity.y ==0 * Time.deltaTime)
            rb.AddForce(new Vector2(0, jumpStrenght * Time.deltaTime), ForceMode2D.Impulse);
    }
    private void Jump(float force)  
    { 
        rb.AddForce(new Vector2(0, force * Time.deltaTime), ForceMode2D.Impulse);    
    }

    private void ResetPosition()
    {
            transform.SetPositionAndRotation(firstPosBloupy, new Quaternion(0, 0, 0, 0));
            rb.velocity = new Vector2(0f, 0f);
    }

    public bool GetIsGrounded() { return isGrounded; }

    //Check if Gloupy is on the ground
    private void OnTriggerEnter2D(Collider2D collision) {   if (collision.tag == "ground")  isGrounded = true;  }

    private void OnTriggerStay2D(Collider2D collision)  {   if (collision.tag == "ground")  isGrounded = true;  }

    private void OnTriggerExit2D(Collider2D collision)  {   if (collision.tag == "ground")  isGrounded = false; }



}
