using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public Transform startPosition;
    public Transform endPosition;
    public float moveSpeed = 5f;

    public Vector3 targetPosition;
    // Start is called before the first frame update
    void Start()
    {
        targetPosition = startPosition.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }
}
