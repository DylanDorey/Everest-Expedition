using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
/*
 * Author: [Diaz,Jr Ismael ]
 * Last Updated: [05/06/2024]
 * [Health manager and damage taken script]
 */
public class HealthManager : MonoBehaviour
{
    //Health Properties
    public int maxHealth = 100;
    public int currentHealth;
    public float invincibilityDuration = 3f;
    public float blinkInterval = 0.1f;

    //When player has taken damage, player is invincible
    private bool isInvincible = false;
    private Renderer playerRenderer;
    private float invincibilityTimer = 0f;
    private float blinkTimer = 0f;

    //Player starts at max health 
    void Start()
    {
        currentHealth = maxHealth;
        playerRenderer = GetComponent<Renderer>();
    }

    /// <summary>
    /// In charge of connecting blinking and invincible proterties for a certain time.
    /// After 3 seconds invincibiliity is false
    /// </summary>
    void FixedUpdate()
    {
        if (isInvincible)
        { 
            invincibilityTimer += Time.deltaTime;
            blinkTimer += Time.deltaTime;

            if (invincibilityTimer >= invincibilityDuration)
            { 
                isInvincible = false;
                playerRenderer.enabled = true;
            }

            if (blinkTimer >= blinkInterval)
            {
                playerRenderer.enabled = !playerRenderer.enabled;
                blinkTimer = 0f;
            }
        }
        else
        {
            isInvincible = false;
        }
    }

    /// <summary>
    /// When players take damage, health decreases and invincibilityTimer is active
    /// </summary>
    /// <param name="damage"></param>
 
    public void takeDamage(int damage)
    {
        if (!isInvincible)
        { 
            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                Die();
            }
            else 
            {
                isInvincible = true;
                invincibilityTimer = 0f;
            }
        }
    }


    public void Die()
    {
        Debug.Log("Player Died.");
    }

    //Early code for taking damage
    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Spikes"))
        {
            takeDamage(10);
        }
    }
    */
}
