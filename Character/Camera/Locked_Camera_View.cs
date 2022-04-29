using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locked_Camera_View : MonoBehaviour
{
    public Transform player;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = Vector3.Lerp(transform.position, player.position, .1f);
    }
}
