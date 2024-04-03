using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHammer : MonoBehaviour
{
    //the multiplier for how how the player gets boosted in the air
    [Range(0, 50)]
    public float boostMultiplier;
    
    //rotation and angle properties of hammer
    public Transform rotationPoint;
    public Transform pickPos;
    private Vector3 pickaxePos;
    private Vector3 mouse_pos;
    public float angle;

    //moust position and mouse delta values
    private Vector3 lastMousePosition;
    private Vector3 mouseDelta;

    //reference to player's rigidbody
    public Rigidbody playerRB;


    private void Start()
    {
        //initialize the mouse position
        lastMousePosition = Input.mousePosition;
    }

    private void Update()
    {
        //the last mouse position detected
        lastMousePosition = Input.mousePosition;

        ConvertMouseDeltaToPositive();

        MaxAngle();
    }

    private void FixedUpdate()
    {
        //set the position of the hammer to the pick position
        transform.position = pickPos.position;

        //convert mouse position to an angle/degree value
        mouse_pos = Input.mousePosition;
        pickaxePos = Camera.main.WorldToScreenPoint(rotationPoint.position);
        mouse_pos.z = 5f;
        mouse_pos.x = mouse_pos.x - pickaxePos.x;
        mouse_pos.y = mouse_pos.y - pickaxePos.y;
        angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;

        //apply degree value to picks rotation
        transform.rotation = Quaternion.Euler(-angle * 0.5f, pickPos.rotation.y, 0f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if the pick hits the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            //calculate the mouse delta
            mouseDelta = Input.mousePosition - lastMousePosition;

            //create a force speed with mouse delta and a boost multiplier
            float forceSpeed = mouseDelta.y * boostMultiplier;

            //add that force to the player in an upward motion
            playerRB.AddForce(Vector3.up * -forceSpeed);
        }
    }

    /// <summary>
    /// Converts the mouse delta to a positive number
    /// </summary>
    private void ConvertMouseDeltaToPositive()
    {
        //if the mouse delta is negative
        if (mouseDelta.y < 0)
        {
            //make it positive
            mouseDelta.y *= -1;
        }
    }

    /// <summary>
    /// Sets the max angle the pick can move to
    /// </summary>
    private void MaxAngle()
    {
        //if the angle surpassed -100
        if (angle < -100f)
        {
            //set the picks max rotation to -100 on the x axis
            transform.rotation = Quaternion.Euler(-100f, transform.parent.transform.position.y, 0f);
        }
        else if (angle > 100f) //if the angle surpassed 100
        {
            //set the picks max rotation to 100 on the x axis
            transform.rotation = Quaternion.Euler(100f, transform.parent.transform.position.y, 0f);
        }
    }
}
