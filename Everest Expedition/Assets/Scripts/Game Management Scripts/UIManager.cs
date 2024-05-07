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

public class UIManager : Singleton<UIManager>
{
    //Keeps tracks of the TemmpBar 
    public float maxTime = 60f;
    public float decreaseRate = 1f;

    //Various screen UI elements
    public GameObject menuScreen, playingScreen, optionsScreen, controlsScreen, gameOverScreen, sensitivitySlider, climbingSymbol, exploringSymbol;

    //health and thirst sliders
    public Slider healthSlider, thirstSlider, tempSlider;

    //health and thirst text, and center and objective text
    public TextMeshProUGUI healthText, thirstText, centerText, objectiveText;

    private bool optionsOpen = false;
    private bool controlsOpen = false;

    /// <summary>
    /// KINESTHETIC PROTOTYPE VARIABLES
    /// </summary>
    public string[] tasks;
    public string[] objectives;

    /////////////////////////////////////////

    private void Start()
    {
        //initialize tasks
        tasks = new string[] { "Welcome to Everest Expedition!. Walk by using W and S to start the tutorial.", "Look left and right by using A and D", "Change your sensitivity by pressing ENTER", "View controls by pressing TAB", "Walk forward to those spikes", "Grab that medkit there to heal yourself",
            "Select numbers 1-5 to use items in your inventory. Press 1 to use that medkit now", "Boost yourself up by dragging the mouse downward at different speeds for different heights", "You have reached a checkpoint! You will be rewarded with a random item when reaching checkpoints", "Beware, icy surfaces will make you slide off of platforms", 
            "You've switched to the climbing state and cannot turn. Climb by moving your mouse left and right.", "WINNER! Grab the Flag!" };

        //initialize objectives
        objectives = new string[] { "Walk forward using W and backwards using S", "Look left and right using A and D", "Press ENTER to open options menu", "Press TAB to view Control Scheme", "Walk into the spikes", "Grab the medkit", "Use the medkit by pressing 1", "Boost yourself up in the air to the platform", "Don' fall off", "Avoid the icy surface", "Climb up the platforms", "Pick up the flag to claim victory!"};
    }

    private void Update()
    {
        //attach the health and thirst slider values to the players health and thirst variables
        healthSlider.value = PlayerData.Instance.playerHealth;
        thirstSlider.value = PlayerData.Instance.playerThirst;

        //check what color the health and thirst text should be
        HealthThirstTextRed();

        if (GameManager.Instance.isPlaying)
        {
            //start the temperature gain
            PlayerData.Instance.TempGain();
            ClimbingExploringSymbols();
        }
    }

    private void OnEnable()
    {
        GameEventBus.Subscribe(GameState.menu, EnableMenuUI);
        GameEventBus.Subscribe(GameState.startGame, EnablePlayingUI);
        GameEventBus.Subscribe(GameState.gameOver, EnableGameOverUI);
    }

    private void OnDisable()
    {
        GameEventBus.Unsubscribe(GameState.menu, EnableMenuUI);
        GameEventBus.Unsubscribe(GameState.startGame, EnablePlayingUI);
        GameEventBus.Unsubscribe(GameState.gameOver, EnableGameOverUI);
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
    void OnTriggerEnter(Collider other)
    {
        // Check if collision is with checkpoint prefab
        if (other.CompareTag("Checkpoint"))
        {
            // Reset time to max time
            PlayerData.Instance.currentTime = maxTime;
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
        SetDisplayScreen(true, false, false, false, false);
    }

    /// <summary>
    /// enables the playing game screen
    /// </summary>
    private void EnablePlayingUI()
    {
        //disable the menu and game over screen, but enable the playing screen
        SetDisplayScreen(false, true, false, false, false);

        //disable the center text and objective text
        centerText.text = "";
        objectiveText.text = "";

        //disable the cursor
        Cursor.visible = false;
    }

    /// <summary>
    /// opens and closes the option menu when playing
    /// </summary>
    public void OptionsUI()
    {
        //if the options menu is not open
        if (optionsOpen == false)
        {
            //disable the menu, game over screen, and playing screen, but enable the options menu
            SetDisplayScreen(false, false, true, false, false);

            //disable the center text
            centerText.text = "";

            //enable the cursor 
            Cursor.visible = true;

            //set options open to true
            optionsOpen = true;
        }
        else //if the options menu is open
        {
            //reenable the playing screen
            SetDisplayScreen(false, true, false, false, false);

            //disable the cursor
            Cursor.visible = false;

            //update the players sensitivity given the UI slider value
            PlayerController.Instance.rotateSpeed = sensitivitySlider.GetComponent<Slider>().value;

            //set options open to false
            optionsOpen = false;
        }
    }

    /// <summary>
    /// opens and closes the option menu when playing
    /// </summary>
    public void ControlsUI()
    {
        //if the options menu is not open
        if (controlsOpen == false)
        {
            //disable the menu, game over screen, and playing screen, but enable the options menu
            SetDisplayScreen(false, false, false, true, false);

            //disable the center text
            centerText.text = "";

            //set options open to true
            controlsOpen = true;
        }
        else //if the options menu is open
        {
            //reenable the playing screen
            SetDisplayScreen(false, true, false, false, false);

            //set options open to false
            controlsOpen = false;
        }
    }

    /// <summary>
    /// Sets the climbing symbol or exploring symbol active depending on which state the player is in
    /// </summary>
    public void ClimbingExploringSymbols()
    {
        //set the state correct symbol active
        climbingSymbol.SetActive(PlayerController.Instance.isClimbing);
        exploringSymbol.SetActive(PlayerController.Instance.isExploring);
    }

    /// <summary>
    /// enable the game over screen
    /// </summary>
    private void EnableGameOverUI()
    {
        //enable the cursor 
        //disable the menu and playing screen, but enable the game over screen
        Cursor.visible = true;
        SetDisplayScreen(false, false, false, false, true);
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

    /// <summary>
    /// enables and disables the correct screen at runtime
    /// </summary>
    /// <param name="menu"> sets the menu screen on or off </param>
    /// <param name="game"> sets the game screen on or off </param>
    /// <param name="options"> sets the options screen on or off </param>
    /// <param name="controls"> sets the controls screen on or off </param>
    /// <param name="over"> sets the game over screen on or off </param>
    private void SetDisplayScreen(bool menu, bool game, bool options, bool controls, bool over)
    {
        menuScreen.SetActive(menu);
        playingScreen.SetActive(game);
        optionsScreen.SetActive(options);
        controlsScreen.SetActive(controls);
        gameOverScreen.SetActive(over);
    }

    /// <summary>
    /// Display the task in the center of the screen
    /// </summary>
    /// <param name="task"> the index of tasks to choose from </param>
    public void DisplayText(int task)
    {
       //set the center screen text to the correct task
       centerText.text = tasks[task];
    }

    /// <summary>
    /// Sets the objective for the player
    /// </summary>
    /// <param name="objective"> the index of the objectives to choose from </param>
    /// <returns> the objective for the player </returns>
    public string SetObjectiveText(int objective)
    {
        //returns the correct objective depending on the objective the player collides with
        return objectives[objective];
    }
}
