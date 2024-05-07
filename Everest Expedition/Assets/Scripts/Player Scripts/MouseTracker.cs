using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class MouseTracker : MonoBehaviour
{
    private Vector3 mousePos2D;
    private Vector3 objPos;

    // Update is called once per frame
    void Update()
    {
        mousePos2D = Input.mousePosition;
        mousePos2D.z = 1;
        Debug.Log(Camera.main.ScreenToWorldPoint(mousePos2D));
        objPos = Camera.main.ScreenToWorldPoint(mousePos2D);
        transform.position = objPos;
    }
}
