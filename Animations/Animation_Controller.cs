using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Animation_Controller : MonoBehaviour
{
    private CharacterController controller;
    private Animator animator;
    private Player_Controls playerControls;
    public float maxVelocity = 30f;
    private Transform moveTransform;
    private Transform playerTransform;
    private Quaternion moveInputRotation;
    public float turnSpeed = .25f;
    private Vector3 smoothAnimationsVector;
    private Vector3 currentVelocity;
    private Vector3 smoothVelocity;
    public float upMomentum = .5f;
    public float downMomentum = 1f;

    //Inertia System Variables
    private float playerVelocity;
    private float blendForward;
    private float blendRight;
    public float velIncrement = 1f;
    public float velDecrement = 2f;

    //Camera direction for movement
    public Transform camera;
    //Inputs
    private Vector2 moveCamera;

    public Camera_Elevation cameraY;
    public Camera_Rotator cameraX;

    private void Awake()
    {
        playerControls = new Player_Controls();
    }

    void Start()
    {
        playerTransform = gameObject.GetComponent<Transform>();
        animator = gameObject.GetComponent<Animator>();
        controller = gameObject.GetComponent<CharacterController>();
        moveTransform = gameObject.GetComponentInChildren<Locked_Rotator>().transform;
        smoothAnimationsVector = new Vector3(0,0,0);
        blendForward = 0f;
        blendRight = 0f;
        playerVelocity = 0f;
    }

    void FixedUpdate()
    {   
        //Input definitions
        moveCamera = playerControls.Land.MoveCamera.ReadValue<Vector2>();
        if(moveCamera.x != 0 || moveCamera.y != 0)
        {
            //Set horizontal and vertical speed based on distance from player
            cameraX.horizontal = moveCamera.x * 2.5f;
            cameraY.vertical = moveCamera.y * -1f;
        } else 
        {
            cameraX.horizontal = 0f;
            cameraY.vertical = 0f;
            //cameraDistance.distance = 0f;
        }

        Vector2 move = playerControls.Land.Move.ReadValue<Vector2>();
        if(move.x != 0 || move.y != 0)
        {       
            //Smooth the velocity change for fake inertia
            if(playerVelocity < (Mathf.Abs(Vector2.Distance(move, new Vector2(0,0))) * maxVelocity))
            {
                playerVelocity += velIncrement;
            }
            if(playerVelocity > maxVelocity) playerVelocity = maxVelocity;
            if(playerVelocity < 0) playerVelocity = 0;
            animator.SetFloat("PlayerVel", playerVelocity);
            
            //Move character in forward direction
            if(animator.GetBool("LockedOn"))
            {
                //Slow the change in direction
                moveInputRotation = Quaternion.LookRotation(new Vector3(move.x, 0, move.y));
                moveTransform.rotation = Quaternion.Slerp(moveTransform.rotation, moveInputRotation, turnSpeed / 3);
                //Move player
                controller.Move(moveTransform.forward * Time.deltaTime * animator.GetFloat("PlayerVel") / 8);
            } else 
            {
                //Slow the change in direction
                moveInputRotation = Quaternion.LookRotation(new Vector3(move.x, 0, move.y));
                playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, camera.rotation * moveInputRotation, turnSpeed / 3);
                //Move player
                controller.Move(playerTransform.forward * Time.deltaTime * animator.GetFloat("PlayerVel") / 5);
            }

            //Smooth the input vector for animation blend tree to slowly blend between directions
            smoothAnimationsVector = Vector3.SmoothDamp(smoothAnimationsVector, new Vector3(move.x, 0, move.y), ref currentVelocity, .2f);

            blendForward = Mathf.SmoothDamp(blendForward, move.y, ref currentVelocity.x, 1f);
            blendRight = Mathf.SmoothDamp(blendRight, move.x, ref currentVelocity.z, 1f);

            if(blendForward > 1) blendForward = 1;
            if(blendForward < -1) blendForward = -1;
            if(blendRight > 1) blendRight = 1;
            if(blendRight < -1) blendRight = -1;
            animator.SetFloat("Forward", blendForward);
            animator.SetFloat("Right", blendRight);
        } else
        {
            //Inertia slow down
            if( playerVelocity > 0)
            {
                playerVelocity -= velDecrement;
            }
            if(playerVelocity > maxVelocity) playerVelocity = maxVelocity;
            if(playerVelocity < 0) playerVelocity = 0;
            animator.SetFloat("PlayerVel", playerVelocity);

            //Move player
            if(animator.GetBool("LockedOn"))
            {
                controller.Move(moveTransform.forward * Time.deltaTime * animator.GetFloat("PlayerVel") / 8);    
            }
                controller.Move(playerTransform.forward * Time.deltaTime * animator.GetFloat("PlayerVel") / 5);
        }
    }
    
    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();   
    }
}
