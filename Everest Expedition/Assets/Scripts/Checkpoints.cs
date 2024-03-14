using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    [SerializeField] List<GameObject> Check;
    [SerializeField] Vector3 Playerpoint;

    private void OnTriggerEnter(Collider other)
    {

    }
}
