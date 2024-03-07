using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class CharacterMove : MonoBehaviour
{
    public GameObject playerBody;
    public GameObject mousePoint;
    public GameObject Pick;

    public Vector3 bodyPos;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       Vector3 mousePos2D = Input.mousePosition;

       mousePos2D.z = -Camera.main.transform.position.z;
       Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

       Vector3 mouseDelta = mousePos3D - bodyPos;


       mousePoint.transform.position = mouseDelta;

       float maxMagnitude = this.GetComponent<SphereCollider>().radius;
       if (mouseDelta.magnitude > maxMagnitude)
       {
           mouseDelta.Normalize();
           mouseDelta *= maxMagnitude;
       }

        Pick.transform.position = mouseDelta;
    }
}
