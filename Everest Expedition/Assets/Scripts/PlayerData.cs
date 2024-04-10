using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: [Dorey, Dylan]
 * Last Updated: [03/18/2024]
 * [Contains all player variable data such as health, thirst, etc]
 */

public class PlayerData : MonoBehaviour
{
    //singelton for PlayerData
    private static PlayerData _instance;
    public static PlayerData Instance { get { return _instance; } }

    //int for the player score, floats for player health and player thirst values
    public int playerScore;
    public float playerHealth;
    public float playerThirst;

    //thirst drain and unlimited thirst values
    public bool unlimitedThirst = false;
    public bool thirstEmpty = false;
    private float drainTick = 5f;
    public float staminaDrainRate = 10f; 
    public float staminaRefillDelay = 1f; 
    public float staminaRefillRate = 20f;
    private bool isMoving = false;
    private Coroutine refillCoroutine;

    private bool isInvincible = false;
    private float invincibilityTimer = 0f;
    public float invincibilityDuration = 3f;
    public float blinkInterval = 0.1f;
    private Renderer playerRenderer;
    private float blinkTimer = 0f;

    private void Awake()
    {
        //if _instance contains something and it isn't this
        if (_instance != null && _instance != this)
        {
            //Destroy it
            Destroy(this.gameObject);
        }
        else
        {
            //otherwise set this to _instance
            _instance = this;
        }
    }

    private void Start()
    {
        PlayerEventBus.Publish(PlayerState.onStart);
    }

    private void OnEnable()
    {
        PlayerEventBus.Subscribe(PlayerState.onStart, PlayerStart);
    }

    private void OnDisable()
    {
        PlayerEventBus.Unsubscribe(PlayerState.onStart, PlayerStart);
    }

    void Update()
    {
        if (unlimitedThirst)
        {
            playerThirst = 100f;
            thirstEmpty = false;
        }

        if (playerThirst <= 0)
        {
            thirstEmpty = true;
        }
        else
        {
            thirstEmpty = false;
        }

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

    private void FixedUpdate()
    {
        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            isMoving = true;
            StartThirstDrain();
        }
        else
        {
            if (isMoving)
            {
                isMoving = false;
                if (refillCoroutine != null)
                {
                    StopCoroutine(refillCoroutine);
                }
                refillCoroutine = StartCoroutine(RefillStamina());
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

        //start draining thirst value
        //StartThirstDrain();
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
    /// Drains the players thirst a certain amount every specified amount of seconds
    /// </summary>
    private void ThirstDrain()
    {
        playerThirst -= staminaDrainRate * Time.deltaTime;
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
    IEnumerator RefillStamina()
    {
        // Wait for the refill delay before starting to refill stamina
        yield return new WaitForSeconds(staminaRefillDelay);

        while (playerThirst < 100f)
        {
            // Refill stamina gradually
            playerThirst += staminaRefillRate * Time.deltaTime;
            yield return null;
        }

        // Ensure stamina is exactly at maximum
        playerThirst = 100f;
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
        if (!isInvincible)
        {
            playerHealth -= damage;

            if (playerHealth <= 0)
            {
                PlayerEventBus.Publish(PlayerState.onDeath);
            }
            else
            {
                isInvincible = true;
                invincibilityTimer = 0f;
            }
        }
    }

    //private void OnGUI()
    //{
    //    if(GUILayout.Button("Damage Health"))
    //    {
    //        playerHealth -= 20;
    //    }

    //    if (GUILayout.Button("Damage Thirst"))
    //    {
    //        playerThirst -= 20;
    //    }
    //}
}
