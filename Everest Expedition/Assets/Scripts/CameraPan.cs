using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPan : MonoBehaviour
{
    public Transform target;

    public float smoothSpeed = 5f;
    public Vector3 offset;

    private float defaultRotX = 6f;
    private float defaultPosY = 2f;
    private float targetRotX;
    private float targetPosY;
    private bool hasSetBackToDefault = false;

    private void FixedUpdate()
    {
        //Vector3 wantedPosition = target .position + offset;
        //Vector3 smoothPositon = Vector3.Lerp(transform.position, wantedPosition, smoothSpeed * Time.deltaTime);
        //transform.position = smoothPositon;

        //CheckToAngleCamera();
    }

    private void CheckToAngleCamera()
    {
        if (PlayerController.Instance.isGrounded)
        {
            ReturnToDefault();
        }
        else
        {
            StartCoroutine(OnBoostCameraAngle());
        }
    }

    /// <summary>
    /// Rotates the camera downward when the player boosts themself in the air
    /// </summary>
    private IEnumerator OnBoostCameraAngle()
    {
        while (PlayerController.Instance.isGrounded == false)
        {
            hasSetBackToDefault = false;

            transform.eulerAngles += new Vector3(0.003f, 0f, 0f);
            transform.position += new Vector3(0f, 0.0005f, 0f);

            yield return null;
        }
    }

    private void ReturnToDefault()
    {
        float timer = 0f;

        if (!hasSetBackToDefault)
        {
            timer += Time.deltaTime;

            //lerp to the default rotation and position
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(defaultRotX, transform.rotation.y, transform.rotation.z), timer / 0.5f);
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, defaultPosY, transform.position.z), timer / 0.5f);
        }
    }
}
