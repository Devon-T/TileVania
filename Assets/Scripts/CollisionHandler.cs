using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class CollisionHandler : MonoBehaviour
{

    [SerializeField] bool collisionDisabled;
    [SerializeField] float levelLoadDelay = 3f;

    bool isDead = false;

    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;

    // Use this for initialization
    void Start()
    {
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        EnemyCollision();
     //   if (Debug.isDebugBuild)  todo figure out why the debug build rule is failing???
     //   {
            debugControls();
     //   }

    }
    private void EnemyCollision()
    {
        if (collisionDisabled) { return; }
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")) || myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Hazards")))
        {
            if (isDead) { return; }
            StartDeathSequence();
            isDead = true;
        }
        if (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Enemy")))
        {
            KillEnemy();
        }
    }
    private void StartDeathSequence()
    {
        SendMessage("OnPlayerDeath");
        Invoke("RestartGame", levelLoadDelay);
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    private void debugControls()
    {
        if (CrossPlatformInputManager.GetButtonDown("Fire1"))
        {
            if (collisionDisabled)
            {
                collisionDisabled = false;
            }
            else
            {
                collisionDisabled = true;
            }
        }
        if (CrossPlatformInputManager.GetButtonDown("Back"))
        {
            SceneManager.LoadScene(0);
        }
    }

    private void KillEnemy()
    {
        print("die!");
    }
}
