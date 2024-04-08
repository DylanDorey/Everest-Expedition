using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
 * Author: [Dorey, Dylan]
 * Last Updated: [4/01/2024]
 * [Displays the correct UI for the user depending on what state the game is in]
 */

public class UIManager : MonoBehaviour
{
    //singelton for InventoryManager
    private static UIManager _instance;
    public static UIManager Instance { get { return _instance; } }

    //Various screen UI elements
    public GameObject menuScreen, playingScreen, gameOverScreen;

    //health and thirst sliders
    public Slider healthSlider, thirstSlider;

    //health and thirst text, and center and objective text
    public TextMeshProUGUI healthText, thirstText, centerText, objectiveText;

    /// <summary>
    /// KINESTHETIC PROTOTYPE VARIABLES
    /// </summary>
    public string[] tasks;
    public string[] objectives;

    /////////////////////////////////////////

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

        tasks = new string[] { "Welcome to Everest Expedition Kinesthetic Prototype. Walk by using W and S", "Look left and right by using A and D", "Walk forward to those spikes", "Grab that medkit there to heal yourself", 
            "Select numbers 1-5 to use items in your inventory. Press 1 to use that medkit", "Boost yourself up by dragging the mouse downward at different speeds for different heights", "You have reached a checkpoint. You will respawn here if you fall off", "Thank you for playtesting our Kinesthetic Prototype!" };

        objectives = new string[] { "Walk forward using W", "Look left and right using A and D", "Walk into the spikes", "Grab the medkit", "Use the medkit by pressing 1", "Boost yourself up in the air to the next platform", "Don' fall off","Please fill out the Google Form" };
    }
    
    private void Start()
    {
        GameEventBus.Publish(GameState.startGame);
    }

    private void Update()
    {
        //attach the health and thirst slider values to the players health and thirst variables
        healthSlider.value = PlayerData.Instance.playerHealth;
        thirstSlider.value = PlayerData.Instance.playerThirst;

        //check what color the health and thirst text should be
        HealthThirstTextRed();
    }

    private void OnEnable()
    {
        GameEventBus.Subscribe(GameState.startGame, EnablePlayingUI);
    }

    private void OnDisable()
    {
        GameEventBus.Unsubscribe(GameState.startGame, EnablePlayingUI);
    }

    private void OnTriggerStay(Collider other)
    {
        //if the other game object is tagged objective
        if (other.gameObject.CompareTag("Objective"))
        {
            //display the objective and task text
            DisplayText(other.GetComponent<Objective>().objectiveIndex);
            objectiveText.text = SetObjectiveText(other.GetComponent<Objective>().objectiveIndex);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //clear text when leaving collider
        centerText.text = "";
    }

    /// <summary>
    /// enables the menu UI
    /// </summary>
    private void EnableMenuUI()
    {
        //disable the playing screen and game over screen, but enable the menu screen
        menuScreen.SetActive(true);
        playingScreen.SetActive(false);
        gameOverScreen.SetActive(false);
    }

    /// <summary>
    /// enables the playing game screen
    /// </summary>
    private void EnablePlayingUI()
    {
        //disable the menu and game over screen, but enable the playing screen
        menuScreen.SetActive(false);
        playingScreen.SetActive(true);
        gameOverScreen.SetActive(false);

        //disable the center text and objective text
        centerText.text = "";
        objectiveText.text = "";
    }

    /// <summary>
    /// enable the game over screen
    /// </summary>
    private void EnableGameOverUI()
    {
        //disable the menu and playing screen, but enable the game over screen
        menuScreen.SetActive(false);
        playingScreen.SetActive(false);
        gameOverScreen.SetActive(true);
    }

    /// <summary>
    /// Turns the health and thirst text red when its empty
    /// </summary>
    private void HealthThirstTextRed()
    {
        //if the players health is 0
        if(PlayerData.Instance.playerHealth <= 0)
        {
            //set the text color to red
            healthText.color = Color.red;
        }
        else
        {
            //otherwsie set it to green
            healthText.color = Color.green;
        }

        //if the players thirst is 0
        if (PlayerData.Instance.playerThirst <= 0)
        {
            //set the text to red
            thirstText.color = Color.red;
        }
        else
        {
            //otherwise set it to cyan
            thirstText.color = Color.cyan;
        }
    }

    /// <summary>
    /// Tells the player what they have picked up
    /// </summary>
    public IEnumerator ItemPickup(string itemName)
    {
        for (int index = 0; index < 1; index++)
        {
            //if the inventory is not full
            if (InventoryManager.Instance.inventoryFull == false)
            {
                //display what the player picked up for 3 seconds
                centerText.text = "Picked up " + itemName;
                yield return new WaitForSeconds(3f);
            }
            else
            {
                //otherwise tell the player the invetory is full for three seconds
                centerText.text = "Inventory Full";
                yield return new WaitForSeconds(3f);
            }
        }

        //set the center text back to empty
        centerText.text = "";
    }

    /////////////////////////////////////////////////////////
    //KINESTHETIC PROTOTYPE UI
    ////////////////////////////////////////////////////////

    /// <summary>
    /// Display the task in the center of the screen
    /// </summary>
    /// <param name="task"> the index of tasks to choose from </param>
    /// <returns></returns>
    public void DisplayText(int task)
    {
       //set the center screen text to the correct task
       centerText.text = tasks[task];
    }

    /// <summary>
    /// Sets the objective for the player
    /// </summary>
    /// <param name="objective"> the index of the objectives to choose from </param>
    /// <returns></returns>
    public string SetObjectiveText(int objective)
    {
        //returns the correct objective depending on the objective the player collides with
        return objectives[objective];
    }

    /////////////////////////////////////////////////////
}
