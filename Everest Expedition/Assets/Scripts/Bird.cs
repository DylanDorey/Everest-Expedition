using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public float moveSpeed = 5f;
    public float waitTime = 2f;

    public bool movingToEnd = true;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MoveBetweenPoint());
    }

    // Update is called once per frame
    IEnumerator MoveBetweenPoint()
    {
        while (true)
        { 
            Vector3 targetPosition = movingToEnd ? startPoint.position : endPoint.position;
            while (transform.position != targetPosition)
            { 
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }
            yield return (waitTime);
            movingToEnd = !movingToEnd;
            
        }
    }
}
