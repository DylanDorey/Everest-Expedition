using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/*
 * Author: [Dorey, Dylan]
 * Last Updated: [03/18/2024]
 * [Allows the player to move left and right on the map, jump, and select items in inventory slots]
 */

public enum PlayerState
{
    onStart,
    thirstEmpty,
    onDeath,
    completedChallenge,
    onFall
}

public class PlayerController : MonoBehaviour
{
    //singelton for PlayerController
    private static PlayerController _instance;
    public static PlayerController Instance { get { return _instance; } }

    //reference to scriptable object PlayerInput
    public PlayerInput playerInput;

    //player controller booleans
    public bool hasJumped = false;
    public bool isGrounded = true;
    public bool hasLanded = true;

    //player controller attributes
    [Range(1f, 15f)]
    public float playerSpeed = 8f;

    [Range(1f, 150f)]
    public float rotateSpeed;

    [Range(1f, 10f)]
    public float jumpHeight = 5f;

    [Range(1f, 5f)]
    public float jumpDelay = 2f;

    //spawn position
    public Vector3 spawnPos;

    //raycast elements
    private RaycastHit hit;
    private Vector3 rayDirection = Vector3.down;


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
        //initialize spawn pos
        spawnPos = transform.position;
    }

    private void OnEnable()
    {
        PlayerEventBus.Subscribe(PlayerState.onDeath, OnDeath);

        GameEventBus.Subscribe(GameState.startGame, EnablePlayerController);
        GameEventBus.Subscribe(GameState.gameOver, DisablePlayerController);
    }

    private void OnDisable()
    {
        PlayerEventBus.Unsubscribe(PlayerState.onDeath, OnDeath);

        GameEventBus.Unsubscribe(GameState.startGame, EnablePlayerController);
        GameEventBus.Unsubscribe(GameState.gameOver, DisablePlayerController);
    }

    void FixedUpdate()
    {
        //reads the Vector2 value from the playerActions components and from the move action (AD) in our actions scriptable object
        Vector2 moveVec = playerInput.Player.Move.ReadValue<Vector2>();
        transform.Translate(new Vector3(0f, 0f, moveVec.y) * playerSpeed * Time.deltaTime);

        Vector2 rotateVec = playerInput.Player.Rotate.ReadValue<Vector2>();
        transform.Rotate(new Vector3(0f, rotateVec.x, 0f) * rotateSpeed * Time.deltaTime);

        CheckIfGrounded();
    }

    private void OnTriggerEnter(Collider other)
    {
        //if the other game object is tagged spikes
        if (other.CompareTag("Spikes"))
        {
            //do damage to player
            PlayerData.Instance.TakeDamage(40);
        }

        //if the other game object is tagged death barrier
        if (other.CompareTag("DeathBarrier"))
        {
            //kill the player
            PlayerEventBus.Publish(PlayerState.onDeath);
        }

        //if the other game object is tagged checkpoint
        if (other.gameObject.CompareTag("Checkpoint"))
        {
            //set the new spawnpoint for the player, turn the checpoint off, grant the reward for reaching the checkpoint, they remove the checkpoint game object
            spawnPos = other.gameObject.transform.position;
            other.gameObject.SetActive(false);
            other.GetComponent<Checkpoints>().GrantReward();
            Destroy(other.gameObject);
        }

        //if the other game object is tagged Flag
        if(other.gameObject.CompareTag("Flag"))
        {
            GameEventBus.Publish(GameState.gameOver);
        }
    }
    /// <summary>
    /// Allows the player to move forward and backwards
    /// </summary>
    /// <param name="context"> the context in which the button was pressed </param>
    public void OnMove(InputAction.CallbackContext context)
    {
        //On move is only going to fire when called with W or S
        Vector2 moveVec = context.ReadValue<Vector2>();
        transform.Translate(new Vector3(0f, 0f, moveVec.y) * -playerSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Allows the player to rotate the camera
    /// </summary>
    /// <param name="context"> the context in which the button was pressed </param>
    public void OnRotate(InputAction.CallbackContext context)
    {
        //On rotate fires when called with A or D
        Vector2 rotateVec = context.ReadValue<Vector2>();
        transform.Rotate(new Vector3(0f, rotateVec.x, 0f) * rotateSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Allows the player to jump
    /// </summary>
    /// <param name="context"> the context in which the button was pressed </param>
    public void OnJump(InputAction.CallbackContext context)
    {
        //if the player hasn't jumped yet
        if (!hasJumped)
        {
            //On hyperspace is only going to fire when the event is called. It doesn't continualy get called when held
            if (context.performed)
            {
                //jump and start the jump delay
                StartCoroutine(JumpDelay());
            }
        }
    }

    /// <summary>
    /// allows the player to jump vertically every 2 seconds
    /// </summary>
    private IEnumerator JumpDelay()
    {
        for (int index = 0; index < 1; index++)
        {
            //the player has jumped so disable jumping for 2 seconds and apply upward force to the rigid body
            hasJumped = true;
            GetComponent<Rigidbody>().AddForce(transform.up * (jumpHeight * 1000f) * Time.deltaTime);

            yield return new WaitForSeconds(jumpDelay);
        }

        //set hasJumped back to false
        hasJumped = false;
    }

    /// <summary>
    /// Uses any item that is stored in inventory slots 1-5
    /// </summary>
    public void OnSlot1Select()
    {
        OnSlotSelect(0);
    }

    public void OnSlot2Select()
    {
        OnSlotSelect(1);
    }

    public void OnSlot3Select()
    {
        OnSlotSelect(2);
    }

    public void OnSlot4Select()
    {
        OnSlotSelect(3);
    }

    public void OnSlot5Select()
    {
        OnSlotSelect(4);
    }
    //////////////////////////////////////////////

    /// <summary>
    /// Applies the item's ability/use to the player when selectedc and removes the item from the inventory
    /// </summary>
    /// <param name="slotIndex"> the slot that is selected by the player </param>
    private void OnSlotSelect(int slotIndex)
    {
        //inventory slot at slotIndex reference
        InventorySlot inventorySlot = InventoryManager.Instance.inventorySlots.transform.GetChild(slotIndex).GetComponent<InventorySlot>();

        //if the slot has an item
        if (inventorySlot.hasItem)
        {
            //use it, then remove it
            PlayerData.Instance.ApplyItemAbility(inventorySlot.itemUse);
            InventoryManager.Instance.RemoveItemOnUse(slotIndex);
        }
    }

    /// <summary>
    /// When the player dies this function will be called
    /// </summary>
    private void OnDeath()
    {
        //set the players position to the spawn pos
        transform.position = spawnPos;

        //reset player data to default
        PlayerData.Instance.ResetPlayerData();
    }

    /// <summary>
    /// checks if the player is grounded
    /// </summary>
    private void CheckIfGrounded()
    {
        //DEBUG draw ray line
        //Debug.DrawRay(transform.position, rayDirection, Color.red);

        //if the ray hits something
        if (Physics.Raycast(transform.position, rayDirection, out hit, 1f))
        {
            //is grounded is true
            isGrounded = true;
            
            //set hasLanded to true for 1 frame
            StartCoroutine(OnLanding());
        }
        else
        {
            //otherwise is grounded is false
            isGrounded = false;
        }
    }

    /// <summary>
    /// sets hasLanded to true for 1 frame when landing
    /// </summary>
    /// <returns></returns>
    private IEnumerator OnLanding()
    {
        hasLanded = true;
        yield return null;
        hasLanded = false;
    }

    /// <summary>
    /// enables the player's input
    /// </summary>
    private void EnablePlayerController()
    {
        //reference for the PlayerInput scriptable object
        playerInput = new PlayerInput(); //constructor

        //turn playerActions on
        playerInput.Enable();
    }

    /// <summary>
    /// disables the player's input
    /// </summary>
    private void DisablePlayerController()
    {
        //turn playerActions off
        playerInput.Disable();
    }
}
