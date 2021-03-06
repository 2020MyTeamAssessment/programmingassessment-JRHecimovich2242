﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour
{
    [Header("Movement Variables")]
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float jumpSpeed = 4f;

    [Header("Delay Variables")]
    [SerializeField] float timeBeforeTurning = 4f;
    [SerializeField] float attackDelay = 5f;
    [SerializeField] float timeBeforeJumping = 3f;

    [Header("GameObject References")]
    [SerializeField] GameObject arrow;
    [SerializeField] GameObject bow;


    bool attacking = false;
    bool alive = true;
    float walkTime;
    float jumpTime;
    float attackTime;
    int arrowsBeforeDeath = 3;
    int arrowsFired = 0;
    Vector2 skeletonScale;

    

    
    //Cached component references
    Rigidbody2D myRigidBody;
    Animator myAnimator;

    // Start is called before the first frame update
    void Start()
    {
        //Store a reference to the Rigidbody2D component of the skeleton
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        walkTime = timeBeforeTurning;
        jumpTime = timeBeforeJumping;
        attackTime = attackDelay;
        skeletonScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (alive)
        {
            if (!attacking)
            {
                //When the skeleton attacks, we want to freeze it so it is not shooting and moving at the same time
                //I am choosing to have the skeleton be stationary while attacking for the purposes of this assessment
                HandleMovement();
                AttackTimerChange();
            }
        }
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

    private void AttackTimerChange()
    {
        attackTime -= Time.deltaTime;
        if(attackTime < Mathf.Epsilon)
        {
            myAnimator.SetTrigger("Attack");
        }
    }

    public void Attack()
    {
        arrowsFired++;
        //Instantiate the arrow
        myRigidBody.velocity = new Vector2(0f, 0f);
        Instantiate(arrow, bow.transform.position, transform.rotation);
    }

    public void StartAttacking()
    {
        //Freeze the skeleton so it does not move and attack at the same time
        attacking = true;
        myRigidBody.velocity = new Vector2(0f, 0f);
    }


    public void StopAttacking()
    {
        //Set attacking to false so that HandleMovement() will begin being called again
        attacking = false;
        attackTime = attackDelay;
        if(arrowsFired >= arrowsBeforeDeath)
        {
            //After the set amount of arrows are fired, the skeleton will die
            Death();
        }
    }

    private void Death()
    {
        myAnimator.SetBool("Dead", true);
        alive = false;
        myRigidBody.velocity = new Vector2(0f, 0f);
    }
    public void DestroySkeleton()
    {
        //Called by death animation. Will destroy the game object after the animation completes
        Destroy(gameObject);
    }

}

