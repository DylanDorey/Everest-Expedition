using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPan : MonoBehaviour
{
    //Used to give camera smoother movement
    public float maxDistance = 5f;
    public float smoothSpeed = 10f;
    public Vector3 offset;
    private Vector3 _currentVelocity = Vector3.zero;
    public Transform target;

    //default camera position and rotation
    public Transform defaultPos;

    private void FixedUpdate()
    {
        //checks when to start angling the camera down
        CheckToAngleCamera();
    }

    private void Awake()
    {
        offset = transform.position - target.position;  
    }

    private void LateUpdate()
    {
        Vector3 targetPosition = target.position;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _currentVelocity, smoothSpeed * Time.deltaTime);

        /* Early test of Smooth camera code
        //Vector3 wantedPosition = target.position + offset;
        //Vector3 smoothPositon = Vector3.Lerp(transform.position, wantedPosition, smoothSpeed * Time.deltaTime);
        //transform.position = smoothPositon;
        */
        float distance = Vector3.Distance(transform.position, target.position);

        if (distance > maxDistance)
        {
            transform.position = target.position + (transform.position - target.position) * maxDistance;
        }
    }
    /// <summary>
    /// Determines when to move the camera
    /// </summary>
    private void CheckToAngleCamera()
    {
        //if the player has landed
        if (PlayerController.Instance.hasLanded)
        {
            //return the camera back to its default transform
            ReturnToDefault();
        }
        
        //if the player is not grounded
        if (PlayerController.Instance.isGrounded == false)
        {
            //only activates in explore mode
            if(PlayerController.Instance.isExploring == true)
            {
                //start moving the camera in an upwards orientation
                StartCoroutine(OnBoostCameraAngle());
            }
        }
    }

    /// <summary>
    /// Rotates the camera downward when the player boosts themself in the air
    /// </summary>
    private IEnumerator OnBoostCameraAngle()
    {
        //while the player is not grounded
        while (PlayerController.Instance.isGrounded == false)
        {
            //add values to the x rotation and y position every frame
            transform.eulerAngles += new Vector3(0.003f, 0f, 0f);
            transform.position += new Vector3(0f, 0.0004f, 0f);

            //wait one frame
            yield return null;
        }
    }

    /// <summary>
    /// Lerps the camera back to its default postion
    /// </summary>
    private void ReturnToDefault()
    {
        //the timer to lerp by
        float timer = 0f;
        timer += Time.deltaTime;

        //lerp the camera back to the default rotation and position
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(6f, transform.eulerAngles.y, transform.eulerAngles.z), timer / 0.5f);
        transform.position = Vector3.Lerp(transform.position, defaultPos.position, timer / 0.1f);
    }
}
