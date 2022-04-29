using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Distance : MonoBehaviour
{
    public float distance;
    public Transform anchor;

    void Update()
    {
        if(transform.localPosition.z <= -1f && transform.localPosition.z >= -10f)
        {
            transform.position += transform.forward * distance * Time.deltaTime;
        }
        if(transform.localPosition.z > -2.5f)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -2.5f);
        }
        if(transform.localPosition.z < -10f)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -10f);
        }
    }
}
