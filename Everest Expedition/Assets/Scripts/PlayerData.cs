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

    public bool unlimitedThirst = false;

    //int for the player score, floats for player health and player thirst values
    public int playerScore;
    public float playerHealth;
    public float playerThirst;

    public bool thirstEmpty = false;
    private float drainTick = 5f;

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
        StartThirstDrain();
    }

    private void OnEnable()
    {
        PlayerEventBus.Subscribe(PlayerState.onStart, StartThirstDrain);
    }

    private void OnDisable()
    {
        
    }

    void Update()
    {
        if (unlimitedThirst)
        {
            playerThirst = 100f;
            thirstEmpty = false;
        }

        if(playerThirst <= 0)
        {
            thirstEmpty = true;
        }
        else
        {
            thirstEmpty = false;
        }

        if (playerHealth <= 0)
        {
            PlayerEventBus.Publish(PlayerState.onDeath);
        }
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

    public IEnumerator UnlimitedThirst(float unlimitedDuration)
    {
        float originalThirst = playerThirst;

        for (int index = 0; index < 1; index++)
        {
            unlimitedThirst = true;

            yield return new WaitForSeconds(unlimitedDuration);
        }

        unlimitedThirst = false;

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
    /// Drains the players health when the thirst is empty
    /// </summary>
    private void HealthDrain()
    {
        //if the thirst is empty
        if (thirstEmpty)
        {
            //remove 3 health from the player
            playerHealth -= 3f;
            //playerHealth -= 3f * healthDrainMultiplier;
            //healthDrainMultiplier = healthDrainMultiplier * 1.01f;
            //healthTickMultiplier = healthTickMultiplier * 0.998f;
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
