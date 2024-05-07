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

    //level to spawn in 
    public GameObject level;

    //determines if the game is a fresh start or replay
    private bool hasLevel = false;

    private void Start()
    {
        //disable players gravity
        PlayerController.Instance.gameObject.GetComponent<Rigidbody>().useGravity = false;
        PlayerController.Instance.transform.GetChild(5).GetComponent<Rigidbody>().useGravity = false;

        //set has level to false on start
        hasLevel = false;

        //start the game in the menu state
        GameEventBus.Publish(GameState.menu);
    }

    /// <summary>
    /// This will start the game Everest Expedition
    /// </summary>
    public void StartGame()
    {
        //enable players gravity
        PlayerController.Instance.gameObject.GetComponent<Rigidbody>().useGravity = true;
        PlayerController.Instance.transform.GetChild(5).GetComponent<Rigidbody>().useGravity = true;

        //if there is a level
        if (hasLevel)
        {
            //destroy it
            Destroy(GameObject.FindGameObjectWithTag("Level"));
        }

        //spawn in the level again
        Instantiate(level, new Vector3(0f, 0f, -8f), Quaternion.identity);

        //set has level to true
        hasLevel = true;

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
