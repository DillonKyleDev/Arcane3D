using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow_Player : MonoBehaviour
{
    private GameManager gameManager;
    public Transform player;
    public Transform clip;
    public Transform lockedCameraView;
    public float lerpTime = 2f;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }
    void FixedUpdate()
    {
        if(!gameManager.cameraIsLocked)
        {
            //Move camera to Camera Clip position with a little height on it
            Vector3 pos = clip.transform.position;
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, pos, lerpTime * 1.5f * Time.deltaTime);
        } else
        {
            Vector3 pos = lockedCameraView.transform.position;
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, pos, lerpTime * 1.5f * Time.deltaTime);
        }

        //Rotate camera to face the player 
        Vector3 higherPlayer = new Vector3(player.position.x, player.position.y + 2f, player.position.z);
        Quaternion lookRotation = Quaternion.LookRotation((higherPlayer - transform.position));
        gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, lookRotation, gameManager.currentCameraSmooth);
    }
}