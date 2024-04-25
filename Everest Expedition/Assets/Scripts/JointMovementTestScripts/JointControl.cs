using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class JointControl : MonoBehaviour
{
    public Rigidbody pick;
    public Vector3 jointPoint;
    public float angle;
    public float speed;

    // Update is called once per frame
    void FixedUpdate()
    {

        Vector3 mousePos2D = Input.mousePosition;

        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        Vector3 mouseDelta = mousePos3D - jointPoint;

        float maxMagnitude = this.GetComponent<SphereCollider>().radius;

        if(mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }

        float Xval = Input.GetAxis("Mouse X");
        float Yval = Input.GetAxis("Mouse Y");

        pick.angularVelocity += new Vector3(Xval * speed, Yval, Xval * speed);
    }
}
