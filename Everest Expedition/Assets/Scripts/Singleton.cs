using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: [Dorey, Dylan]
 * Last Updated: [04/20/2024]
 * [Singleton as a generic]
 */

public class Singleton<T> : MonoBehaviour where T : Component
{
    //private static instnace of a class
    private static T _instance;

    //public instance of a class
    public static T Instance
    {
        get
        {
            //if there is no instance
            if (_instance == null)
            {
                //set instance to any object with the same T component
                _instance = FindObjectOfType<T>();

                //if instance is still null
                if (_instance == null)
                {
                    //create a new gameobject with said T component and initialize it
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).Name;
                    _instance = obj.AddComponent<T>();
                }
            }

            //return the instance of the class
            return _instance;
        }
    }

    public virtual void Awake()
    {
        //if instance is null
        if (_instance == null)
        {
            //set instance to this as the component T
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            //otherwise destroy this duplicate instance
            Destroy(gameObject);
        }
    }
}
