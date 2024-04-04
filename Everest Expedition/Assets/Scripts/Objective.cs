using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective : MonoBehaviour
{
    public int objectiveIndex;

    private void Start()
    {
        objectiveIndex = objectiveIndex - 1;
    }
}
