using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PlayerMovement : MonoBehaviour
{
    public GameObject cam;
    public float minimumY = -30f;
    public float lookSpeed;
    public float angleToSnap;
    private Collider playerCollider;
    private Rigidbody rb;
    private Vector3 ledgeMemory;
    public Animator animator;

    public AudioClip jumpClip;
    AudioSource audioSource;

    #region Jump Parm
    [SerializeField]
    private bool grounded = true;
    private bool jumpHeld = false;

    [Header("Jump Info")]
    public float hangTime = 1f;
    public float fallSpeedCap = 10;
    public float fallCoefficent = 1;
    public float jumpStrength = 20f;
    public float jumpControl = 1;

    public float slopeSize = 0;
    private bool lastFrameJump;
    private bool thisFrameJump;
    #endregion

    #region Move Param
    [Header("Move Info")]
    public float moveSpeedCap = 10;
    public float runSpeed = 2.0f;
    public float frictionCoefficient = 1.2f;
    private Vector3 force;
    
    #endregion
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<Collider>();
        audioSource = GetComponent<AudioSource>();
        jumpClip = Resources.Load("jumpClip2") as AudioClip;
    }

    private void Update()
    {
        if(PauseManager.singleton?.Paused!=true){
            thisFrameJump = OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger);
            if(!PlayerState.singleton.pointerMode && !UIDisplay.singleton.Active) Move();
            Animations();

            // At the end of each frame we set grounded to false so that
            // OnCollisionStay needs to verify that we are still grounded
            // Obviously it would be better to use OnCollisionExit 
            // but we can't check the normal
            if(grounded && !Physics.Raycast(playerCollider.bounds.center, Vector3.down, playerCollider.bounds.extents.y + 0.5f)) grounded = false;

            lastFrameJump = thisFrameJump;
        }
    }
    
    private void Animations() 
    {
        animator.SetFloat("Speed", 1 + Mathf.Sqrt(Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2)) / moveSpeedCap );
        animator.SetBool("Grounded", grounded);
        animator.SetBool("Walking", xAxis != 0 || zAxis != 0);
    }


    void OnCollisionEnter(Collision collision) 
    {
        // TODO: Detect if it is a valid platform (Not a moving object)
        // Note: This can be accomplished by checking collision.other

        // Check if grounded and handle some other behavior that happens we we ground
        if(Vector3.Dot(collision.contacts[0].normal, Vector3.up ) > slopeSize ) {
            grounded = true;
            jumpHeld = false;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if(!jumpHeld && Vector3.Dot(collision.contacts[0].normal, Vector3.up ) > slopeSize ) {
            grounded = true;
        } 
    }

    float xAxisOld = 0;
    float zAxisOld = 0;
    float xAxis = 0;
    float zAxis = 0;

    /// <summary>
    /// This controls the basic aspects of the players ground and jump movement
    /// </summary>
    private void Move()
    {
        // Movement Input
        xAxis = 0;
        zAxis = 0;

        Vector2 primaryTouchpad = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);
        xAxis += primaryTouchpad.x + Input.GetAxis("Horizontal");
        zAxis += primaryTouchpad.y + Input.GetAxis("Vertical");

        xAxis = 2 * Mathf.Clamp(xAxis, -0.5f, 0.5f);
        zAxis = 2 * Mathf.Clamp(zAxis, -0.5f, 0.5f);

        // If the player falls off of the map then set the player on the last ledge
        if (transform.position.y < minimumY)
        {
            rb.velocity = new Vector3(0, 1, 0);
            transform.position = ledgeMemory;
        }

    # region Jump 
        //if( OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger) || Input.GetKeyUp(KeyCode.Space)) {jumpHeld = false; Debug.Log("fuck");}
        if (grounded) {
            // Update the last on ledge position of the player
            ledgeMemory = transform.position; 
            // Handle a jump input
            if ( (thisFrameJump && !lastFrameJump) || Input.GetKeyDown(KeyCode.Space) )
            {
                animator.SetTrigger("Jump");
                rb.velocity = new Vector3(rb.velocity.x, jumpStrength, rb.velocity.z);
                grounded = false;
                jumpHeld = true;
                audioSource.PlayOneShot(jumpClip, 0.7F);
            }
        }
        else {
           /* // Check if the player is still holding jump from the button and from the hang time
            if( rb.velocity.y < -hangTime) 
                jumpHeld = false;
            // Only use the fall coefficent if we're less then the max fall speed */
            float ySpeed = rb.velocity.y - (!(OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger)||Input.GetKey(KeyCode.Space) ) ? fallCoefficent : 0.1f);
            if (ySpeed > -fallSpeedCap) 
            rb.velocity = new Vector3(rb.velocity.x, ySpeed, rb.velocity.z); 
            if(ySpeed < 0) jumpHeld = false;
        }
    # endregion

        // Calculate force from input, angle, and speed
        var direction = new Vector2(xAxis, zAxis).normalized;
        var forward = cam.transform.forward; forward.y = 0;
        var right = cam.transform.right; right.y = 0;
        force = forward.normalized * direction.y * runSpeed + right.normalized * direction.x * runSpeed;
        force.y = 0;
        
        // Apply ground friction
        rb.velocity  /= ((grounded) ? frictionCoefficient : 1);

        // check if we are going faster then the cap, if not we don't add our foce (other things can still push the player faster)
        if(Mathf.Sqrt(Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2)) < moveSpeedCap) {
            // While in the air our force is reduced to give the player less control and preserve momentum in the jump
            rb.AddForce( (grounded) ? force * frictionCoefficient : (force * jumpControl)/frictionCoefficient, ForceMode.Impulse );
        }
        // If we're off the ground rotate to our jump direction
        rotatePlayer( (grounded) ? xAxis : xAxisOld, (grounded) ? zAxis : zAxisOld);
        
        // Store the previous force for jump momentum 
        if(grounded) {
            xAxisOld = xAxis;
            zAxisOld = zAxis;
        }
    }
    
    /// <summary>
    /// This rotates the player according to
    /// the camera position and player input
    /// </summary>
    private void rotatePlayer (float xAxis, float zAxis) {
        // If statement only if input is received and the player is on the ground
        if ((xAxis != 0 || zAxis != 0))
        {
            // The y rotation of the player and the camera
            float playerRotation = transform.eulerAngles.y;
            
            // Find the rotation for our player input
            Vector2 camForward = new Vector2 (cam.transform.forward.x, cam.transform.forward.z);
            float inputRotation = Vector2.SignedAngle(camForward, new Vector2(-xAxis, zAxis));

            Quaternion inputLook = Quaternion.AngleAxis(inputRotation, Vector3.up);

            // Rotate gently until the snap threshold
            if (Mathf.Abs(playerRotation - inputRotation) > angleToSnap)
                transform.rotation = Quaternion.Lerp(this.transform.rotation, inputLook, lookSpeed * Time.deltaTime);
            else 
                transform.rotation = inputLook;
        }
        
        // Makes sure that the x and z rotations are 0
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }
}
