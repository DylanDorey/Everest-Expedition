using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: []
 * Last Updated: [03/30/2024]
 * [Manages starting, initializing, and ending the game]
 */

public enum GameState
{
    menu,
    startGame,
    gameOver
}

public class GameManager : Singleton<GameManager>
{
    //determines if the player is playing or not
    public bool isPlaying = false;

    private void Start()
    {
        //start the game in the menu state
        GameEventBus.Publish(GameState.menu);
    }

    /// <summary>
    /// This will start the game Everest Expedition
    /// </summary>
    public void StartGame()
    {
        //publish the startGame game event
        GameEventBus.Publish(GameState.startGame);

        //set is playing to true
        isPlaying = true;
    }

    /// <summary>
    /// This will send the user back to the main menu
    /// </summary>
    public void ReturnToMenu()
    {
        //publish the menu game event
        GameEventBus.Publish(GameState.menu);
    }

    /// <summary>
    /// This will allow the user to close/quit Everest Expedition
    /// </summary>
    public void QuitGame()
    {
        //quit the application
        Application.Quit();
    }
}
