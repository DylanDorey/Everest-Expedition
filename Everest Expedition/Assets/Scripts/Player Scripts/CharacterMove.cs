using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UIElements;

public class CharacterMove : MonoBehaviour
{
    public Vector3 mouse_pos;
    public Transform target;
    public GameObject character_pos;
    public GameObject pickaxe;
    public Vector3 object_pos;
    public float angle;

    // Update is called once per frame
    void FixedUpdate()
    {
        mouse_pos = Input.mousePosition;
        mouse_pos.z = 5.23f;
        object_pos = Camera.main.WorldToScreenPoint(target.position);
        mouse_pos.x = mouse_pos.x - object_pos.x;
        mouse_pos.y = mouse_pos.y - object_pos.y;
        angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(character_pos.transform.rotation.x, character_pos.transform.rotation.y, angle);
        transform.position = new Vector3(character_pos.transform.localPosition.x, character_pos.transform.localPosition.y, character_pos.transform.localPosition.z - .75f);
    }
}
