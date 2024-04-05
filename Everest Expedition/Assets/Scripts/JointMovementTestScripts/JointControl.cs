using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointControl : MonoBehaviour
{
    public Vector3 pickLoc;
    public Vector3 jointPoint;
    private Vector3 mouse_pos;
    public GameObject joint;
    public GameObject pickaxe;
    public Transform rotPoint;
    public Transform playerPos;
    public float angle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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

        pickaxe.GetComponent<Rigidbody>().transform.position = mouseDelta;

        //pickaxe.transform.rotation = pickaxe.transform.localRotation;

        mouse_pos = Input.mousePosition;
        pickLoc = Camera.main.WorldToScreenPoint(rotPoint.position);
        mouse_pos.z = 5f;
        mouse_pos.x = mouse_pos.x - pickLoc.x;
        mouse_pos.y = mouse_pos.y - pickLoc.y;
        angle = Mathf.Atan2(mouse_pos.x, mouse_pos.y) * Mathf.Rad2Deg;

        float playerY = playerPos.eulerAngles.y;

        pickaxe.transform.rotation = Quaternion.Euler(0f, playerY, (angle + 90f));
    }
}
