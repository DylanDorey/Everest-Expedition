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
    climbingState,
    exploreState,
    onDeath
}

public class PlayerController : Singleton<PlayerController>
{
    //reference to scriptable object PlayerInput
    public PlayerInput playerInput;

    //player controller booleans
    public bool hasJumped = false;
    public bool isGrounded = true;
    public bool hasLanded = true;
    public bool isClimbing = false;
    public bool isExploring = true;
    public bool canRotate = true;

    //game object for climbing pickaxe
    public GameObject pickaxe;

    //mesh renderers for the different picks
    public MeshRenderer climbingPick;
    public MeshRenderer exploringPick;

    //colliders for the picks
    public Collider jumpingPick;
    public Collider rotatingPickEnd;
    public Collider rotatingPickBase;

    //all power meter bars to reset when grounded
    public GameObject pwrLvl1;
    public GameObject pwrLvl2;
    public GameObject pwrLvl3;
    public GameObject pwrLvl4;
    public GameObject pwrLvl5;
    public GameObject pwrLvl6;
    public GameObject pwrLvl7;
    public GameObject pwrLvl8;
    public GameObject pwrLvl9;
    public GameObject pwrLvl10;
    public GameObject pwrLvl11;
    public GameObject pwrLvl12;

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
    private Vector3 rayDirection = Vector3.down;

    //particle effect prefab
    public GameObject particleEffect;

    private void Start()
    {
        //initialize spawn pos
        spawnPos = new Vector3(0f, 0f, 0f);
    }

    private void OnEnable()
    {
        PlayerEventBus.Subscribe(PlayerState.onDeath, OnDeath);
        PlayerEventBus.Subscribe(PlayerState.climbingState, SetClimb);
        PlayerEventBus.Subscribe(PlayerState.exploreState, SetExplore);

        GameEventBus.Subscribe(GameState.startGame, EnablePlayerController);
        GameEventBus.Subscribe(GameState.gameOver, DisablePlayerController);
    }

    private void OnDisable()
    {
        PlayerEventBus.Unsubscribe(PlayerState.onDeath, OnDeath);
        PlayerEventBus.Unsubscribe(PlayerState.climbingState, SetClimb);
        PlayerEventBus.Unsubscribe(PlayerState.exploreState, SetExplore);

        GameEventBus.Unsubscribe(GameState.startGame, EnablePlayerController);
        GameEventBus.Unsubscribe(GameState.gameOver, DisablePlayerController);
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.isPlaying)
        {
            //reads the Vector2 value from the playerActions components and from the move action (WS) in the actions scriptable object
            Vector2 moveVec = playerInput.Player.Move.ReadValue<Vector2>();
            transform.Translate(new Vector3(0f, 0f, moveVec.y) * playerSpeed * Time.deltaTime);

            //if the player can rotate/is in the exploring mode
            if (canRotate)
            {
                //On rotate fires when called with A or D
                Vector2 rotateVec = playerInput.Player.Rotate.ReadValue<Vector2>();
                transform.Rotate(new Vector3(0f, rotateVec.x, 0f) * rotateSpeed * Time.deltaTime);
            }

            //check if the player is grounded
            CheckIfGrounded();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //if the player enters the climbing trigger
        if(other.CompareTag("climbingChange"))
        {
            PlayerEventBus.Publish(PlayerState.climbingState);
        }

        //if the player enters the exploring trigger
        if (other.CompareTag("exploreChange"))
        {
            PlayerEventBus.Publish(PlayerState.exploreState);
        }

        //if the other game object is tagged spikes
        if (other.CompareTag("Spikes"))
        {
            //do damage to player
            PlayerData.Instance.TakeDamage(40);
        }

        //if the other game object is tagged spikes
        if (other.CompareTag("Bird"))
        {
            //do damage to player
            PlayerData.Instance.TakeDamage(75);
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
            //set the new spawnpoint for the player, turn the checpoint off, grant the reward for reaching the checkpoint, then remove the checkpoint game object
            spawnPos = other.gameObject.transform.position;
            other.gameObject.SetActive(false);
            other.GetComponent<Checkpoints>().GrantReward();
            Destroy(other.gameObject);
        }

        //if the other game object is tagged Flag
        if(other.gameObject.CompareTag("Flag"))
        {
            //set isPlaying to false and publish game over
            GameManager.Instance.isPlaying = false;
            GameEventBus.Publish(GameState.gameOver);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            pwrLvl1.GetComponent<Image>().enabled = false;
            pwrLvl2.GetComponent<Image>().enabled = false;
            pwrLvl3.GetComponent<Image>().enabled = false;
            pwrLvl4.GetComponent<Image>().enabled = false;
            pwrLvl5.GetComponent<Image>().enabled = false;
            pwrLvl6.GetComponent<Image>().enabled = false;
            pwrLvl7.GetComponent<Image>().enabled = false;
            pwrLvl8.GetComponent<Image>().enabled = false;
            pwrLvl9.GetComponent<Image>().enabled = false;
            pwrLvl10.GetComponent<Image>().enabled = false;
            pwrLvl11.GetComponent<Image>().enabled = false;
            pwrLvl12.GetComponent<Image>().enabled = false;
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
            //On Jump is only going to fire when the event is called. It doesn't continualy get called when held
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

        //highlight the slot selected
        StartCoroutine(InventoryManager.Instance.HighlightSlot(slotIndex));

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
        if (Physics.Raycast(transform.position, rayDirection, 2f))
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
    /// <returns> wait 1 frame </returns>
    private IEnumerator OnLanding()
    {
        hasLanded = true;
        yield return null;
        hasLanded = false;
    }

    /// <summary>
    /// Resets the player controller
    /// </summary>
    private void ResetPlayerController()
    {

        //set spawnPos back to 0 0 0, and rotation back to default
        spawnPos = new Vector3(0f, 0f, 0f);
        transform.eulerAngles = new Vector3(0f, 0f, 0f);

        //set position to spawnPos
        transform.position = spawnPos;

        //clear inventory
        InventoryManager.Instance.ClearInventory();
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

        //reset the player controller to its default position
        ResetPlayerController();
    }

    /// <summary>
    /// disables the player's input
    /// </summary>
    private void DisablePlayerController()
    {
        //turn playerActions off
        playerInput.Disable();

        //reset the player controller
        ResetPlayerController();
    }

    /// <summary>
    /// sets the player into the climb state
    /// </summary>
    private void SetClimb()
    {
        transform.eulerAngles = new Vector3(0f, 0f, 0f);

        isExploring = false;
        isClimbing = true;
        canRotate = false;

        if (isClimbing == true)
        {
            exploringPick.enabled = false;
            climbingPick.enabled =true;
            
            jumpingPick.GetComponent<Collider>().enabled = false;
            rotatingPickEnd.enabled = true;
            rotatingPickBase.enabled = true;
            pickaxe.transform.rotation = Quaternion.Euler(0, 0, -90);
            Instantiate(particleEffect, transform.position, transform.rotation);
        }
    }

    /// <summary>
    /// sets the player into the exploration state
    /// </summary>
    private void SetExplore()
    {
        isExploring = true;
        isClimbing = false;
        canRotate = true;

        if (isExploring == true)
        {
            exploringPick.enabled = true;
            climbingPick.enabled = false;
            rotatingPickEnd.enabled = false;
            rotatingPickBase.enabled = false;
            jumpingPick.GetComponent<Collider>().enabled = true;

            Instantiate(particleEffect, transform.position, transform.rotation);
        }
    }
}
