using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public float invincibilityDuration = 3f;
    public float blinkInterval = 0.1f;

    private bool isInvincible = false;
    private Renderer playerRenderer;
    private float invincibilityTimer = 0f;
    private float blinkTimer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        playerRenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isInvincible)
        { 
            invincibilityTimer += Time.deltaTime;
            blinkTimer += Time.deltaTime;

            if (invincibilityTimer >= invincibilityDuration)
            { 
                isInvincible = false;
            }

            if (blinkTimer >= blinkInterval)
            {
                playerRenderer.enabled = !playerRenderer.enabled;
                blinkTimer = 0f;
            }
        }
        else
        {
            isInvincible = true;
        }
    }
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Spikes"))
        {
            takeDamage(10);
        }
    }
}
