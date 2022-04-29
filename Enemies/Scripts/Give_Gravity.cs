using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Give_Gravity : MonoBehaviour
{
    private CharacterController controller;
    private Transform entityTransform;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        entityTransform = GetComponent<Transform>();
    }

    void Update()
    {
        //Apply gravity
        controller.Move(new Vector3(0, -150/entityTransform.position.y, 0) * Time.deltaTime);
    }
}
