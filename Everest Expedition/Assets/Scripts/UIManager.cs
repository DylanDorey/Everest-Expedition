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
    //Various screen UI elements
    public GameObject menuScreen, playingScreen, gameOverScreen;

    //health and thirst sliders
    public Slider healthSlider, thirstSlider;

    //health and thirst text
    public TextMeshProUGUI healthText, thirstText;

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
}
