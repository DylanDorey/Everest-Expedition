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
    initialize,
    startGame,
    gameOver
}

public class GameManager : MonoBehaviour
{
    //singelton for GameManager
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    public bool isPlaying = false;

    void Awake()
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
        //start the game in the menu state
        GameEventBus.Publish(GameState.menu);
    }

    /// <summary>
    /// This will intialize all game elements for Everest Expedition
    /// </summary>
    private void InitializeGame()
    {
        //publish the initialize game event
        GameEventBus.Publish(GameState.initialize);
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
