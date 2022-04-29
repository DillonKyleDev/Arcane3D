using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Rotator : MonoBehaviour
{
    private GameManager gameManager;
    public Transform player;
    public float horizontal;
    public float slerpSpeed = 1f;
    private bool cameraLocked = false;

    private void Start()
    {
        gameManager = GameManager.Instance;    
    }

    void Update()
    {
        //Keep rotator at the same coordinates as player
        Vector3 newPos = new Vector3(player.transform.position.x, player.transform.position.y + 2, player.transform.position.z);
        gameObject.transform.position = Vector3.Slerp(transform.position, newPos, slerpSpeed);       

        if(!gameManager.cameraIsLocked)
        {
            gameObject.transform.Rotate(0.0f, horizontal, 0.0f, Space.Self);         
        }
    }
}
