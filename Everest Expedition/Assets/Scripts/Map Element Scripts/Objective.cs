using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: [Dorey, Dylan]
 * Last Updated: [04/03/2024]
 * [Initializes the index of the objective]
 */

public class Objective : MonoBehaviour
{
    //the order of the objective
    public int objectiveIndex;

    private void Start()
    {
        //set the objective index to minus 1 whatever it is set to
        objectiveIndex -= 1;
    }
}
