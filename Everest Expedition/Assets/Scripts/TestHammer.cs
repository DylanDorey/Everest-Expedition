using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHammer : MonoBehaviour
{
    [Range(0, 50)]
    public float boostMultiplier;

    public Transform rotationPoint;
    public Transform pickPos;
    private Vector3 pickaxePos;
    private Vector3 mouse_pos;
    public float angle;

    private Vector3 lastMousePosition;
    private Vector3 mouseDelta;

    public Rigidbody playerRB;

    public GameObject followCube;


    private void Start()
    {
        lastMousePosition = Input.mousePosition;
    }

    private void Update()
    {
        lastMousePosition = Input.mousePosition;

        if(mouseDelta.y < 0)
        {
            mouseDelta.y *= -1;
        }
    }

    private void FixedUpdate()
    {
        transform.position = pickPos.position;

        mouse_pos = Input.mousePosition;
        pickaxePos = Camera.main.WorldToScreenPoint(rotationPoint.position);
        mouse_pos.z = 5.23f;
        mouse_pos.x = mouse_pos.x - pickaxePos.x;
        mouse_pos.y = mouse_pos.y - pickaxePos.y;
        angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;

        followCube.transform.position = Camera.main.WorldToScreenPoint(Input.mousePosition);

        transform.rotation = Quaternion.Euler(-angle * 0.5f, transform.parent.transform.position.y, 0f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            mouseDelta = Input.mousePosition - lastMousePosition;
            float forceSpeed = mouseDelta.y * boostMultiplier;

            playerRB.AddForce(Vector3.up * -forceSpeed);
        }
    }
}
