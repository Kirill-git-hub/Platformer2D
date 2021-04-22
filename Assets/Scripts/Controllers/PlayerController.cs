﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float jumpForce = 300f;
    private float moveInput;
    private float speed = 4.5f;
    private float deathTime = 0.5f;
    private Animator anim;
    private Rigidbody2D playerRb;
    private Vector2 enemyOrigin;
    private Vector2 enemyRayDirection;
    private float enemyRayDistance;

    [SerializeField] private PlayerData playerData;  

    public bool isOnGround = true;
    public bool facingRight = true;
    public bool canDoubleJump = true;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        
    }
    
    void Movement()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        playerRb.velocity = new Vector2(moveInput * speed, playerRb.velocity.y);

        anim.SetFloat("Speed", Mathf.Abs(moveInput));

        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isOnGround = false;
            canDoubleJump = true;
            anim.SetBool("Jump", true);
        }
        else if(Input.GetKeyDown(KeyCode.Space) && !isOnGround && canDoubleJump)
        {
            playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            anim.SetBool("Jump", true);
            canDoubleJump = false;
        }
        
        if(moveInput > 0 && !facingRight || moveInput < 0 && facingRight)
        {
            Flip();
        }
        
        if (Input.GetKeyDown(KeyCode.F) && moveInput != 0 || Input.GetKeyDown(KeyCode.F) && moveInput == 0)
        {
            anim.SetBool("Attack", true);
            HitEnemy();
        }
        else
        {
            anim.SetBool("Attack", false);
        }
    }

    public void HitEnemy()
    {
        enemyOrigin = new Vector2(transform.position.x, transform.position.y - 0.6f);
        enemyRayDirection = transform.right;
        enemyRayDistance = 1f;

        //Debug.DrawRay(enemyOrigin, transform.right * 1f, Color.red);

        RaycastHit2D hit = Physics2D.Raycast(enemyOrigin, enemyRayDirection, enemyRayDistance, LayerMask.GetMask("Enemy"));

        if(hit.collider != null)
        {
            Enemy collidedObject = hit.collider.gameObject.GetComponent<Enemy>();

            if (collidedObject.CanBeKilled)
            {
                collidedObject.DealDamage(playerData.Damage);
            }
        }
    }

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    GameObject collidedObject = collision.gameObject;

    //    if (collidedObject.CompareTag("Enemy"))
    //    {
    //        if (Input.GetKeyUp(KeyCode.F))
    //        {
    //            if (collision.gameObject.GetComponent<Enemy>().CanBeKilled)
    //            {
    //                collision.gameObject.GetComponent<Enemy>().DealDamage(playerData.Damage);
    //            }
    //        }
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collidedObject = collision.gameObject;

        if (collidedObject.CompareTag("Coin"))
        {
            Coin coin = collidedObject.GetComponent<Coin>();
            if (!coin.IsCollected)
            {
                coin.IsCollected = true;                
                GameController.instance.IncreasePlayerCoins(coin.CoinDenomination);
                Destroy(collidedObject);
                UserInterfaceController.instance.ShowCoins();
            }            
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isOnGround)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                isOnGround = true;
                canDoubleJump = true;
                anim.SetBool("Jump", false);
            }
        }
        
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();

            if (enemy.InstantKill)
            {
                anim.SetBool("Die", true);
                GameController.instance.KillPlayer(deathTime);
            }
            else
            {
                GameController.instance.DealDamageToPlayer(enemy.Damage);

                if (playerData.PlayerHealth <= 0)
                {
                    anim.SetBool("Die", true);
                    GameController.instance.KillPlayer(deathTime);
                }
                else
                {
                    anim.SetBool("Hurt", true);
                    
                }
            }
        }
        else
        {
            anim.SetBool("Hurt", false);
        }
    }
    
    void Flip()
    {
        facingRight = !facingRight;
        Vector2 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
 
}
