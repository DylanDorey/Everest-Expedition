using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public Vector3 startPoint;
    public Vector3 endPoint;
    public float moveSpeed = 5f;
    public float waitTime = 2f;

    [Range(1f, 20f)]
    public float moveDistance;

    public bool movingToEnd = true;
    // Start is called before the first frame update
    void Start()
    {
        startPoint = transform.position;
        endPoint = new Vector3(transform.position.x - moveDistance, transform.position.y, transform.position.z);

        StartCoroutine(MoveBetweenPoint());
    }

    // Update is called once per frame
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
