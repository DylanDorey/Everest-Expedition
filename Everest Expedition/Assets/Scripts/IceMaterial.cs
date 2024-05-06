using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceMaterial : MonoBehaviour
{
    Rigidbody rb;
    bool isSliding;
    Vector3 slideDirection;
    public float slideForce = 5f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (isSliding)
        {
            rb.AddForce(slideDirection * slideForce, ForceMode.Force);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Start sliding immediately upon contact
            isSliding = true;
            // Calculate the slide direction based on the player's movement
            slideDirection = other.GetComponent<Rigidbody>().velocity.normalized;
            // Apply the sliding force immediately
            rb.AddForce(slideDirection * slideForce, ForceMode.Force);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isSliding = false;
        }
    }
}
