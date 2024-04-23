using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class JointControl : MonoBehaviour
{
    public Rigidbody pick;
    public Vector3 pickLoc;
    public Vector3 jointPoint;
    private Vector3 mouse_pos;
    public GameObject joint;
    public GameObject pickaxe;
    public Transform rotPoint;
    public Transform playerPos;
    public Transform hammer;
    public float angle;
    public float speed;

    // Update is called once per frame
    void FixedUpdate()
    {
        jointPoint = joint.transform.position;

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

        //hammer.rotation = Quaternion.LookRotation(mousePos2D);
        //pickaxe.GetComponent<Rigidbody>().transform.position = mouseDelta;

        //pickaxe.transform.rotation = pickaxe.transform.localRotation;

        //mouse_pos = Input.mousePosition;
        //pickLoc = Camera.main.WorldToScreenPoint(rotPoint.position);
        //mouse_pos.z = 5f;
        //mouse_pos.x = mouse_pos.x - pickLoc.x;
        //mouse_pos.y = mouse_pos.y - pickLoc.y;
        //angle = Mathf.Atan2(mouse_pos.x, mouse_pos.y) * Mathf.Rad2Deg;
        //
        //float playerY = playerPos.eulerAngles.y;
        float Xval = Input.GetAxis("Mouse X");
        float Yval = Input.GetAxis("Mouse Y");

        pick.angularVelocity += new Vector3(Xval * speed, Yval, Xval * speed);
    }
}
