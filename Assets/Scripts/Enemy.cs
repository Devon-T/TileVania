﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float deathDelay = 1f;

    Rigidbody2D myRigidBody;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    Animator myAnimator;

    bool isAlive = true;

	// Use this for initialization
	void Start () {
        myRigidBody = GetComponent<Rigidbody2D>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myAnimator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        if (isAlive)
        {
            Walk();
        }
        PlayerAttack();

    }



    private void Walk()
    {
        // if enemy collides with a wall on body collider or do not collide with ground with feet collider then moveSpeed * -1
        if (IsFacingRight())
        {

            myRigidBody.velocity = new Vector2(moveSpeed, 0);
        }
        else
        {
            myRigidBody.velocity = new Vector2(-moveSpeed, 0);
        }

    }

    bool IsFacingRight()
    {
        return transform.localScale.x > 0;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        transform.localScale = new Vector2(-(Mathf.Sign(myRigidBody.velocity.x)), 1f);
    }

    public void PlayerAttack()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Player Feet")))
        {
            KillEnemy();
        }
    }

    public void KillEnemy()
    {
        myAnimator.SetTrigger("Die");
        isAlive = false;
        myRigidBody.velocity = new Vector2(0, 0);
        Invoke("Destroy", deathDelay);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

}
