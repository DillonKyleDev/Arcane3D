using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Elevation : MonoBehaviour
{
    private GameManager gameManager;
    public Transform player;
    public Transform heightCheck;
    public float vertical;
    private bool cameraLocked = false;

    private void Start()
    {
        gameManager = GameManager.Instance;    
    }

    void Update()
    {
        if(!gameManager.cameraIsLocked)
        {
            if(heightCheck.transform.position.y >= player.position.y - .5f && heightCheck.transform.position.y <= player.position.y + 4f)
            {
                gameObject.transform.Rotate(vertical, 0.0f, 0.0f, Space.Self);  
            }
            if(heightCheck.transform.position.y < player.position.y - .5f)
            {
                gameObject.transform.Rotate(-vertical, 0.0f, 0.0f, Space.Self);  
            }
            if(heightCheck.transform.position.y > player.position.y + 4f)
            {
                gameObject.transform.Rotate(-vertical, 0.0f, 0.0f, Space.Self);  
            }
        }      
    }
}
