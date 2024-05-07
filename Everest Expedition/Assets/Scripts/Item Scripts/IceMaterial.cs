using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceMaterial : MonoBehaviour
{
    public Rigidbody rb;
    private bool isSliding;
    public Vector3 slideDirection;
    private float slideForce = 20f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        int randomDirectionIndex = Random.Range(0, 2);

        switch(randomDirectionIndex)
        {
            case 0:
                slideDirection = Vector3.left;
                break;
            case 1:
                slideDirection = Vector3.right;
                break;
        }
    }

    void FixedUpdate()
    {
        if (isSliding)
        {
            rb.AddForce(slideDirection * slideForce, ForceMode.Force);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            rb = collision.transform.GetComponent<Rigidbody>();

            // Start sliding immediately upon contact
            isSliding = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isSliding = false;
        }
    }
}
