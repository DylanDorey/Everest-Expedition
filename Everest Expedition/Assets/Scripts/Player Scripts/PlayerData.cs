using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: [Dorey, Dylan]
 * Last Updated: [03/18/2024]
 * [Contains all player variable data such as health, thirst, etc]
 */

public class PlayerData : Singleton<PlayerData>
{
    //int for the player score, floats for player health and player thirst values
    public int playerScore;
    public float playerHealth;
    public float playerThirst;

    //Keeps tracks of the TemmpBar 
    public float maxTime = 60f;
    public float decreaseRate = 1f;
    public float currentTime;

    //thirst drain and unlimited thirst values
    public bool unlimitedThirst = false;
    public bool thirstEmpty = false;
    private readonly float drainTick = 3f;

    //Damage blink values
    private bool isInvincible = false;
    private float invincibilityTimer = 0f;
    public float invincibilityDuration = 3f;
    public float blinkInterval = 0.1f;
    private Renderer playerRenderer;
    private float blinkTimer = 0f;

    private void OnEnable()
    {
        GameEventBus.Subscribe(GameState.startGame, PlayerStart);
        GameEventBus.Subscribe(GameState.gameOver, StopThirstDrain);
    }

    private void OnDisable()
    {
        GameEventBus.Unsubscribe(GameState.startGame, PlayerStart);
        GameEventBus.Unsubscribe(GameState.gameOver, StopThirstDrain);
    }

    void Update()
    {
        //if the player has unlimited thirst
        if (unlimitedThirst)
        {
            //set the player thirst to 100 and thirst empty to false
            playerThirst = 100f;
            thirstEmpty = false;
        }

        //if player thirst is less than or equal to 0
        if (playerThirst <= 0)
        {
            //set thirst empty to true
            thirstEmpty = true;
        }
        else
        {
            //otherwise set thirst empty to false
            thirstEmpty = false;
        }

        //if the player is playing the game
        if (GameManager.Instance.isPlaying)
        {
            //if the player isInvincible
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
                playerRenderer.enabled = true;
            }
        }
    }

    /// <summary>
    /// when the player starts
    /// </summary>
    private void PlayerStart()
    {
        //initialize the player renderer
        playerRenderer = GetComponent<Renderer>();

        //reset the player data back to default
        ResetPlayerData();

        //start draining thirst value
        StartThirstDrain();
    }

    /// <summary>
    /// Resets all of the players data values to their default state
    /// </summary>
    public void ResetPlayerData()
    {
        //set player score to 0, set player health to 100, and set playerThirst to 100
        playerScore = 0;
        playerHealth = 100;
        playerThirst = 100;
        currentTime = 0f;

        //set unlimitedThirst and thirstEmpty to false
        unlimitedThirst = false;
        thirstEmpty = false;
    }

    /// <summary>
    /// Gives the player unlimited thirst for a short duration
    /// </summary>
    /// <param name="unlimitedDuration"> the length of unlimited thirst </param>
    /// <returns></returns>
    public IEnumerator UnlimitedThirst(float unlimitedDuration)
    {
        //save the players original thirst value
        float originalThirst = playerThirst;

        for (int index = 0; index < 1; index++)
        {
            //set unlimited thirst to true
            unlimitedThirst = true;

            yield return new WaitForSeconds(unlimitedDuration);
        }

        //set unlimited thirst to false
        unlimitedThirst = false;

        //set the players thirst back to its original value
        playerThirst = originalThirst;
    }

    /// <summary>
    /// Applies the items ability to the player data class
    /// </summary>
    /// <param name="itemBehavior"> the use of the item/function </param>
    public void ApplyItemAbility(IItemBehavior itemBehavior)
    {
        itemBehavior.UseItem(this);
    }

    /// <summary>
    /// Starts the thirst drain effect
    /// </summary>
    private void StartThirstDrain()
    {
        //Invoke the thirst drain function
        InvokeRepeating("ThirstDrain", drainTick, drainTick);
    }

    /// <summary>
    /// Stops the thirst drain effect
    /// </summary>
    private void StopThirstDrain()
    {
        //cancel thirst drain invoke
        CancelInvoke("ThirstDrain");
    }

    /// <summary>
    /// Drains the players thirst a certain amount every specified amount of seconds
    /// </summary>
    private void ThirstDrain()
    {
        //if the thirst is not empty
        if (!thirstEmpty)
        {
            //remove 5 thirst
            playerThirst -= 5f;
        }
        else
        {
            //otherwise start draining the health exponentially
            InvokeRepeating("HealthDrain", drainTick, drainTick);
        }
    }

    /// <summary>
    /// Increases temperature bar as the player plays
    /// </summary>
    public void TempGain()
    {
        // Decrease time
        //maxTime -= decreaseRate * Time.deltaTime;
        //currentTime = Mathf.Max(currentTime, 0f); // Ensure time doesn't go below 0

        ////Update temp slider value
        //UIManager.Instance.tempSlider.value = currentTime - maxTime;
        UIManager.Instance.tempSlider.value = currentTime;

        //set the current time
        currentTime += decreaseRate * Time.deltaTime;

        //if the current time is greater than or equal to the max time
        if (currentTime >= maxTime)
        {
            //publish the on death player event
            PlayerEventBus.Publish(PlayerState.onDeath);
        }
    }

    /// <summary>
    /// Drains the players health when the thirst is empty
    /// </summary>
    private void HealthDrain()
    {
        //if the thirst is empty
        if (thirstEmpty)
        {
            //remove 3 health from the player
            playerHealth -= 3f;
        }
    }

    /// <summary>
    /// removes health from the player
    /// </summary>
    /// <param name="damage"> the incoming damage </param>
    public void TakeDamage(int damage)
    {
        //if the player is not invincible
        if (!isInvincible)
        {
            //subtract the incoming damage from the player's health
            playerHealth -= damage;

            //if the players health is less than or equal to 0
            if (playerHealth <= 0)
            {
                //publish the on death player event
                PlayerEventBus.Publish(PlayerState.onDeath);
            }
            else
            {
                //otherwise set isInvincible to true and set the invincibility timer to 0
                isInvincible = true;
                invincibilityTimer = 0f;
            }
        }
    }

}
