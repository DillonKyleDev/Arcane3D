using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_State_Machine : MonoBehaviour
{
    //State variables
    Player_Base_State currentState;
    Player_State_Factory states;

    //Components
    [HideInInspector]
    public GameManager gameManager;
    public CharacterController controller;
    public Animator animator;
    public Player_Controls playerControls;
    public Camera_Elevation cameraY;
    public Camera_Rotator cameraX;

    //Audio sources
    private AudioSource footStep;


    //Local Variables

    //Walking
    [HideInInspector]
    public Vector2 move;
    public float maxVelocity = 30f;
    public Transform lockedMoveTransform;
    public Transform playerTransform;
    [HideInInspector]
    public Quaternion moveInputRotation;
    public float turnSpeed = .25f;
    [HideInInspector]
    public Vector3 smoothAnimationsVector;
    public Vector3 currentVelocity;
    //Inertia System Variables
    [HideInInspector]
    public float playerVelocity;
    [HideInInspector]
    public float blendForward;
    [HideInInspector]
    public float blendRight;
    public float velIncrement = 1f;
    public float velDecrement = 2f;
    //Camera direction for movement
    public Transform cameraRelativeTransformHorizontal;
    //Inputs
    [HideInInspector]
    public Vector2 moveCamera;



    //Locked On Walking
    [HideInInspector]
    public bool isLockedOn = false;



    //Dashing
    [HideInInspector]
    public bool isDashPressed = false;
    [HideInInspector]
    public ParticleSystem dashParticles;
    [HideInInspector]
    public ParticleSystem skidParticles;
    [HideInInspector]
    public Quaternion sideDashRotation;
    public Transform dashTransform;
    public float dashMultiplier = 4f;
    public float dashTime = 0.5f;
    public float dashCost = 30f;
    public bool timeStamped = false;
    public float startTime = 0f;


    //Pick up item
    public bool pickingUpItem = false;


    //Rolling
    public bool isRollingPressed = false;



    //Crouching
    public bool isCrouched = false;


    
    //Attacking
    public bool isRS1HAttacking = false; //Right Sword One Handed
    public bool isRB1HAttacking = false; //Right Blunt One Handed
    public bool isLS1HAttacking = false; //Left Sword One Handed
    public bool isLB1HAttacking = false; //Left Blunt One Handed



    //Getters and Setters
    public bool IsDashPressed { get { return isDashPressed; } set { IsDashPressed = value; }}



    private void Awake()
    {
        states = new Player_State_Factory(this);
        playerControls = new Player_Controls();  
        playerControls.Land.Dash.performed += context => {
            OnDash(context);
        };
        playerControls.Land.Roll.performed += context => {
            OnRoll(context);
        };
        playerControls.Land.Crouch.performed += context => {
            OnCrouch();
        };
        playerControls.Land.LockOn.performed += context => {
           OnLockOn();
        };
        playerControls.Land.LockOn.canceled += context => {
            OnUnlock();
        };
        playerControls.Land.OpenInventory.performed += context => {
           // OnOpenInventory(context);
        };
        playerControls.Land.PickUpItem.performed += context => {
            OnPickUpItem();
        };
        playerControls.Land.EquipLeftHand.performed += context => {
            // OnEquipLeftHand(context);
        };
        playerControls.Land.EquipRightHand.performed += context => {
            // OnEquipRightHand(context);
        };
        playerControls.Land.LeftHandAttack.performed += context => {
            OnLeftHandAttack();
        };
        playerControls.Land.RightHandAttack.performed += context => {
            OnRightHandAttack();
        };

        //Set initial state
        currentState = states.Walking();
        currentState.EnterState();
    }

    void OnDash(InputAction.CallbackContext context)
    {
        isDashPressed = true;
        StartCoroutine(ResetDash());
    }

    void OnRoll(InputAction.CallbackContext context)
    {
        isRollingPressed = true;
    }

    public void StopRolling()
    {
        isRollingPressed = false;
        animator.SetBool("Rolling", false);
    }

    void OnCrouch()
    {
        isCrouched = !isCrouched;
        if(!isCrouched)
        {
            animator.SetBool("Crouched", false);
        }
    }

    void OnLockOn()
    {
        isLockedOn = true;
        gameManager.cameraIsLocked = true;
    }

    void OnUnlock()
    {
        isLockedOn = false;
        gameManager.cameraIsLocked = false;
    }

    void OnLeftHandAttack()
    {
        //isRB1HAttacking = true;
    }

    void OnRightHandAttack()
    {
        Item item = gameManager.HotbarManager.GetActualItem(1).GetComponentInChildren<Item>();
        Debug.Log(item);
        if(item != null && item.GetItemType() == Item.Item_Type.Weapon)
        {
            isRB1HAttacking = true;
            animator.SetBool("RB1HAttack", true);
        }

    }

    void DeactivateRB1H()
    {
        animator.SetBool("RB1HAttack", false);
    }

    public void ActivateHurtBox()
    {
        Item item = gameManager.HotbarManager.GetActualItem(1).GetComponentInChildren<Item>();
        if(item.GetItemType() == Item.Item_Type.Weapon)
        {
            Hurt_Box hb = item.GetComponentInChildren<Hurt_Box>();
            hb.SetDetect(true);
        }
    }
    public void DeactivateHurtBox()
    {
        Item item = gameManager.HotbarManager.GetActualItem(1).GetComponentInChildren<Item>();
        if(item.GetItemType() == Item.Item_Type.Weapon)
        {
            Hurt_Box hb = item.GetComponentInChildren<Hurt_Box>();
            hb.SetDetect(false);
        }
    }

    void CancelRightHandAttack()
    {
        //Might need to modify later
        animator.SetBool("RB1HAttack", false);
        isRB1HAttacking = false;

    }

    void OnPickUpItem()
    {
        GameObject item = gameManager.GetLoadedItem();
        if(item != null)
        {
            pickingUpItem = true;
        }
    }

    public void PickUpAnimTrigger()
    {
        GameObject item = gameManager.GetLoadedItem();
        if(item != null)
        {
            item.GetComponent<AudioSource>().Play();
            item.GetComponent<Pick_Up>().PickUpItem();
        }
    }

    public void EndPickUp()
    {
        pickingUpItem = false;
        animator.SetBool("PickUpItem", false);
    }

    private IEnumerator ResetDash()
    {
        float startTime = Time.time; // need to remember this to know how long to dash
        while(Time.time < startTime + .1f)
        {
            yield return null; // this will make Unity stop here and continue next frame
        }
        isDashPressed = false;
        animator.SetBool("Dashing", false);
    }


    void Start()
    {
        gameManager = GameManager.Instance;

        //Audio
        footStep = gameObject.GetComponent<AudioSource>();

        //Walking
        playerTransform = gameObject.GetComponent<Transform>();
        animator = gameObject.GetComponent<Animator>();
        controller = gameObject.GetComponent<CharacterController>();
        lockedMoveTransform = gameObject.GetComponentInChildren<Locked_Rotator>().transform;
        smoothAnimationsVector = new Vector3(0,0,0);
        blendForward = 0f;
        blendRight = 0f;
        playerVelocity = 0f;

        //Dashing
        dashTransform = GetComponentInChildren<Dash_Rotator>().transform;
        dashParticles = GetComponentInChildren<Dash_Cloud>().GetComponent<ParticleSystem>();
        skidParticles = GetComponentInChildren<Skid_Cloud>().GetComponent<ParticleSystem>();
    }


    void Update()
    {
        //Input definitions
        moveCamera = playerControls.Land.MoveCamera.ReadValue<Vector2>();

        //Walking
        move = playerControls.Land.Move.ReadValue<Vector2>();

        //Camera update
        if(!isLockedOn)
        {
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
        }

    }

    private void FixedUpdate()
    {
        //Check the current states update parameters to see if the state should be switched
        currentState.UpdateState();
    }

    public Player_Base_State GetCurrentState()
    {
        return currentState;
    }

    public void SetCurrentState(Player_Base_State newState)
    {
        currentState = newState;
    }

    //For input system
    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();   
    }

    public void ResetCameraSmooth()
    {
        StartCoroutine(ResetCamera());
    }

    //For camera smoothing after a dash
    private IEnumerator ResetCamera()
    {
        float startTime = Time.time; // need to remember this to know how long to dash
        while(Time.time < startTime + 3f)
        {
            if(gameManager.currentCameraSmooth < gameManager.maxCameraSmooth)
            {
                gameManager.currentCameraSmooth += 0.01f;
            }
            yield return null; // this will make Unity stop here and continue next frame
        }
    }

    //Play footstep sound
    public void PlayFootstep()
    {
        //footStep.Play();
    }
}
