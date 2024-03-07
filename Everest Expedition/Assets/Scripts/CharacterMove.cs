using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UIElements;

public class CharacterMove : MonoBehaviour
{
    public Vector3 mouse_pos;
    public Transform target; //Assign to the object you want to rotate
    public Vector3 object_pos;
    public float angle;

    // public GameObject playerBody;
    //public GameObject Pick;

    // public Vector3 bodyPos;

    //public Transform m_transform;
    //public Transform customPivot;

    // Start is called before the first frame update
    void Start()
    {
       // m_transform = this.transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //  bodyPos.y = playerBody.transform.position.y;
        //Vector3 mousePos2D = Input.mousePosition;

        //  mousePos2D.z = -Camera.main.transform.position.z;
        // Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        // Vector3 mouseDelta = mousePos3D - bodyPos;

        //  maxHeight = bodyPos.y + 2;
        // if (mouseDelta.y > maxHeight)
        //  {
        //     mouseDelta.Normalize();
        //    mouseDelta *= maxHeight;
        // }

        //  Pick. = mouseDelta;

        // Vector3 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        //float rotationZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Quaternion rotation = Quaternion.AngleAxis(rotationZ - 90, Vector3.forward);

        // m_transform.rotation = rotation;

        //transform.Rotate(customPivot.position);

        //Aim player at mouse
        //which direction is up
        //Vector3 upAxis = new Vector3(0, 0, 1);
        //Vector3 mouseScreenPosition = Input.mousePosition;
        //set mouses z to your targets
        //mouseScreenPosition.z = transform.position.z;
        //Vector3 mouseWorldSpace = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        //transform.LookAt(mouseWorldSpace);
        //zero out all rotations except the axis I want
        //transform.eulerAngles = new Vector3(0, 0, -transform.eulerAngles.z);

        mouse_pos = Input.mousePosition;
        mouse_pos.z = 5.23f; //The distance between the camera and object
        object_pos = Camera.main.WorldToScreenPoint(target.position);
        mouse_pos.x = mouse_pos.x - object_pos.x;
        mouse_pos.y = mouse_pos.y - object_pos.y;
        angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
