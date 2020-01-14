using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float jumpSpeed = 4f;
    [SerializeField] float timeBeforeTurning = 4f;
    float walkTime;
    [SerializeField] float timeBeforeJumping = 3f;
    float jumpTime;
    Vector2 skeletonScale;

    
    //Cached component references
    Rigidbody2D myRigidBody;

    // Start is called before the first frame update
    void Start()
    {
        //Store a reference to the Rigidbody2D component of the skeleton
        myRigidBody = GetComponent<Rigidbody2D>();
        walkTime = timeBeforeTurning;
        jumpTime = timeBeforeJumping;
        skeletonScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        //HandleMovement causes the skeleton to move back and forth at moveSpeed for walkTime seconds before turning around
        WalkTimerChange();
        JumpTimerChange();
        if (transform.localScale.x > 0) //Check if the skeleton object is facing right
        {
            //If so, set its velocity to be moveSpeed, i.e. moving to the right
            myRigidBody.velocity = new Vector2(moveSpeed, myRigidBody.velocity.y);
        }
        else
        {
            //Otherwise, set the velocity to be -moveSpeed, i.e. moving to the left
            myRigidBody.velocity = new Vector2(-moveSpeed, myRigidBody.velocity.y);
        }
    }

    private void WalkTimerChange()
    {
        //Decrements walkTime and determines whether or not the skeleton needs to turn around
        walkTime -= Time.deltaTime;
        if(walkTime <= Mathf.Epsilon)
        {
            FlipSprite();
        }
    }

    private void FlipSprite()
    {
        //FlipSprite flips the Skeleton game object on the x axis to match its movement direction
        transform.localScale = new Vector2(-(Mathf.Sign(myRigidBody.velocity.x)), 1f) * skeletonScale;
        walkTime = timeBeforeTurning;
    }

    private void JumpTimerChange()
    {
        //Decrements walkTime and determines whether or not the skeleton needs to turn around
        jumpTime -= Time.deltaTime;
        if (jumpTime <= Mathf.Epsilon)
        {
            Jump();
        }
    }

    private void Jump()
    {
        jumpTime = timeBeforeJumping;
        myRigidBody.velocity += new Vector2(0f , jumpSpeed);
    }

}

