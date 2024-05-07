using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    //Vector3s for the start and end flight path
    public Vector3 startPoint;
    public Vector3 endPoint;

    //the speed the bird is moving and the time that it waits before flying again
    public float moveSpeed = 5f;
    public float waitTime = 2f;

    //keeps track of the distance that the bird can travel
    [Range(1f, 20f)]
    public float moveDistance;

    public bool movingToEnd = true;
    // Start is called before the first frame update
    void Start()
    {
        //changes the birds start position
        startPoint = transform.position;

        //sets the birds end location
        endPoint = new Vector3(transform.position.x - moveDistance, transform.position.y, transform.position.z);

        //begins the coroutine for moving the bird from start to end
        StartCoroutine(MoveBetweenPoint());
    }

    //moves the bird between a start point and end point
    IEnumerator MoveBetweenPoint()
    {
        while (true)
        { 
            Vector3 targetPosition = movingToEnd ? startPoint : endPoint;
            if (transform.position != targetPosition)
            { 
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }
            else
            {
                yield return (waitTime);
                movingToEnd = !movingToEnd;
            }
        }
    }
}
