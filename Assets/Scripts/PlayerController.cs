using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


public class PlayerController : MonoBehaviour {

    //config
    [Header("General")]
    [Tooltip("In ms^-1")] [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpStrength = 8f;
    [SerializeField] float climbSpeed = 5f;

    //State
    bool isAlive = true;
    

    //Cached component references
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    Collider2D myCollider;
    float gravityScaleAtStart;

    //Message the methods  
    void Start () {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCollider = GetComponent<Collider2D>();
        gravityScaleAtStart = myRigidBody.gravityScale;

    }

    public void OnPlayerDeath()
    {
        isAlive = false;
    }

    // Update is called once per frame
    void Update () {
        if (!isAlive) { return; }
        Run();
        Jump();
        FlipSprite();
        Climbing();
    }

    private void Run()
    {
        
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        Vector2 playerVelocity = new Vector2(runSpeed * controlThrow, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("Running", playerHasHorizontalSpeed);

     }

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            //use sign to get a value of +1 or -1 to get the local scale
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f);
        }
    }

    private void Jump()
    {
        bool onGroundCheck = myCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        if (CrossPlatformInputManager.GetButtonDown("Jump") && onGroundCheck)
        {
            Vector2 jumpVelocityToAdd = new Vector2(0f, jumpStrength);
            myRigidBody.velocity += jumpVelocityToAdd;
        }
    }

    private void Climbing()
    {
        float verticalInputCheck = CrossPlatformInputManager.GetAxis("Vertical");
        bool touchingLadderCheck = myCollider.IsTouchingLayers(LayerMask.GetMask("Ladders"));
        if (!touchingLadderCheck)
        {
            myRigidBody.gravityScale = gravityScaleAtStart;
            myAnimator.SetBool("Climbing", false);
            return;
        }
        myRigidBody.gravityScale = 0f;
        Vector2 climbVelocity = new Vector2(myRigidBody.velocity.x, climbSpeed * verticalInputCheck);
        myRigidBody.velocity = climbVelocity;

        bool playerHasVerticalSpeed = Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("Climbing", playerHasVerticalSpeed);

    }
}
