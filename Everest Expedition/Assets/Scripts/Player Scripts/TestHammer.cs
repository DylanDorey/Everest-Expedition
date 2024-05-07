using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Author: [Dorey, Dylan]
 * Last Updated: [04/03/2024]
 * [A pickaxe that allows the player to traverse by boosting themselves via mouse movement]
 */

public class TestHammer : MonoBehaviour
{
    //the multiplier for how how the player gets boosted in the air
    [Range(1, 5)]
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

    //reference to player's transform and rigidbody
    public Transform playerPos;
    public Rigidbody playerRB;

    //Every layer of the jump power meter
    public GameObject powerLvl1;
    public GameObject powerLvl2;
    public GameObject powerLvl3;
    public GameObject powerLvl4;
    public GameObject powerLvl5;
    public GameObject powerLvl6;
    public GameObject powerLvl7;
    public GameObject powerLvl8;
    public GameObject powerLvl9;
    public GameObject powerLvl10;
    public GameObject powerLvl11;
    public GameObject powerLvl12;


    private void Start()
    {
        //initialize the mouse position
        lastMousePosition = Input.mousePosition;

        //set all power meter images to active = false
        powerLvl1.GetComponent<Image>().enabled = false;
        powerLvl2.GetComponent<Image>().enabled = false;
        powerLvl3.GetComponent<Image>().enabled = false;
        powerLvl4.GetComponent<Image>().enabled = false;
        powerLvl5.GetComponent<Image>().enabled = false;
        powerLvl6.GetComponent<Image>().enabled = false;
        powerLvl7.GetComponent<Image>().enabled = false;
        powerLvl8.GetComponent<Image>().enabled = false;
        powerLvl9.GetComponent<Image>().enabled = false;
        powerLvl10.GetComponent<Image>().enabled = false;
        powerLvl11.GetComponent<Image>().enabled = false;
        powerLvl12.GetComponent<Image>().enabled = false;
    }

    private void Update()
    {
        //the last mouse position detected
        lastMousePosition = Input.mousePosition;

        //convert the mouse delta return value to positive in the event it returns negative
        ConvertMouseDeltaToPositive();

        //make sure the hammer does not go passed a certain angle
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
        angle = Mathf.Atan2(mouse_pos.y, 0f) * Mathf.Rad2Deg;

        //update the player's eulerAngles y value every frame
        float playerY = playerPos.eulerAngles.y;

        //apply degree value to picks rotation
        transform.rotation = Quaternion.Euler((-angle * 0.3f), playerY, 0f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if the pick hits the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            //calculate the mouse delta
            mouseDelta = Input.mousePosition - lastMousePosition;

            //create a force speed with mouse delta and a boost multiplier
            if (GameManager.Instance.isPlaying)
            {
                //create a force speed based upon the mouse delta y multiplies by the boost multiplier
                float forceSpeed = mouseDelta.y * boostMultiplier;

                //add that force to the player in an upward motion
                playerRB.AddForce(Vector3.up * -forceSpeed);

                //gets the jump height and shows it on the power meter
                if(-forceSpeed > 100f)
                {
                    powerLvl1.GetComponent<Image>().enabled = true;
                }

                if (-forceSpeed > 300f)
                {
                    powerLvl2.GetComponent<Image>().enabled = true;
                }

                if (-forceSpeed > 500f)
                {
                    powerLvl3.GetComponent<Image>().enabled = true;
                }

                if (-forceSpeed > 700f)
                {
                    powerLvl4.GetComponent<Image>().enabled = true;
                }

                if (-forceSpeed > 900f)
                {
                    powerLvl5.GetComponent<Image>().enabled = true;
                }

                if (-forceSpeed > 1100f)
                {
                    powerLvl6.GetComponent<Image>().enabled = true;
                }

                if (-forceSpeed > 1300f)
                {
                    powerLvl7.GetComponent<Image>().enabled = true;
                }

                if (-forceSpeed > 1500f)
                {
                    powerLvl8.GetComponent<Image>().enabled = true;
                }

                if (-forceSpeed > 1600f)
                {
                    powerLvl9.GetComponent<Image>().enabled = true;
                }

                if (-forceSpeed > 1700f)
                {
                    powerLvl10.GetComponent<Image>().enabled = true;
                }

                if (-forceSpeed > 1800f)
                {
                    powerLvl11.GetComponent<Image>().enabled = true;
                }

                if (-forceSpeed > 2000f)
                {
                    powerLvl12.GetComponent<Image>().enabled = true;
                }
            }
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
