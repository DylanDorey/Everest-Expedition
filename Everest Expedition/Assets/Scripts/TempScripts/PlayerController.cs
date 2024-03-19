using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/*
 * Author: [Dorey, Dylan]
 * Last Updated: [03/18/2024]
 * [Allows the player to move left and right on the map, jump, and select items in inventory slots]
 */

public enum PlayerState
{
    alive,
    dead,
    climbing
}

public class PlayerController : MonoBehaviour
{
    //reference to scriptable object PlayerInput
    public PlayerInput playerInput;

    public float playerSpeed = 5f;

    private void Awake()
    {
        //reference for the PlayerInput scriptable object
        playerInput = new PlayerInput(); //constructor

        //turn playerActions on
        playerInput.Enable();
    }

    void FixedUpdate()
    {
        //reads the Vector2 value from the playerActions components and from the move action (AD) in our actions scriptable object
        Vector2 moveVec = playerInput.Player.Move.ReadValue<Vector2>();
        transform.Translate(new Vector3(moveVec.x, 0f, 0f) * playerSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {

    }

    /// <summary>
    /// allows the player to rotate when an input action is detected from the action map
    /// </summary>
    public void OnMove(InputAction.CallbackContext context)
    {
        //On move is only going to fire when the event is called. It doesn't continualy get called when held
        Vector2 moveVec = context.ReadValue<Vector2>();
        transform.Translate(new Vector3(moveVec.x, 0f, 0f) * playerSpeed * Time.deltaTime);
    }

    public void OnInventorySelect()
    {
        //UseItemInSlot();
    }

    /// <summary>
    /// Picks up item
    /// </summary>
    public void PickupItem(GameObject item)
    {
        
    }
}
