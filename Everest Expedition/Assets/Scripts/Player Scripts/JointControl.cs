using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class JointControl : MonoBehaviour
{
    //the pickaxe's rigidbody, used to apply force
    public Rigidbody pick;

    //the location at which the joint is placed on the player's body, used for rotation
    public Vector3 jointPoint;

    //the angle the pickaxe is at and the speed that it moves depending on mouse movement
    public float angle;
    public float speed;

    // Update is called once per frame
    void FixedUpdate()
    {
        //gathers the mouse position on screen
        Vector3 mousePos2D = Input.mousePosition;

        //sets z value to keep in a single spot in line with the camera
        mousePos2D.z = -Camera.main.transform.position.z;

        //uses the mouse position in worldspace
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        //sets the mouseDelta to the mouse's location minus the joint
        Vector3 mouseDelta = mousePos3D - jointPoint;
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if(mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }

        //gets the X and Y value of mouse motion
        float Xval = Input.GetAxis("Mouse X");
        float Yval = Input.GetAxis("Mouse Y");

        //applies force to the pickaxe with angularVelocity
        pick.angularVelocity += new Vector3(Xval * speed, Yval, Xval * speed);
    }
}
