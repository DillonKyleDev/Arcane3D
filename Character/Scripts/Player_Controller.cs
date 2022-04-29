using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Controller : MonoBehaviour
{
    private GameManager gameManager;
    private Hot_Bar_Manager hotBarManager;
    private Player_Controls playerControls;
    private UnityEngine.CharacterController controller;
    private Animator animator;
    private ParticleSystem dashParticles;
    private ParticleSystem skidParticles;
    private ParticleSystem fireBreath;
    private Quaternion lookRotation;
    private Transform playerTransform;
    private Quaternion prevDirection;
    private SphereCollider oneHandedArc;
    public double _dashTime = .2;
    public double _idleLungeTime = .1;
    public double _lungeTime = .1;
    public float playerSpeed = 6f;
    public float slowSpeed = 2.5f;
    public float regularSpeed = 6f;
    public float turnSpeed = .25f;
    public float regularTurnSpeed = .25f;
    public float slowTurnSpeed = .1f;
    public float maxInertia = .1f;
    private float currentInertia;
    public float inertiaIncrement = .1f;
    public float dashMultiplier = 3f;
    HashSet<string> attackedList;
    private bool hasAttackLunged = false;
    public float lockOnDistance = 10f;
    public float dashCost = 100f;
    private bool isLockedOn = false;
    private GameObject lockedTarget;
    private Quaternion sideDashRotation;
    private Transform dashTransform;
    private bool hasDashed = false;
    private bool canMove = true;
    private bool inventoryButtonPressed;
    private bool openInventory = false;
    public Inventory_Menu inventoryMenu;
    public bool isMoving = false;
    public GameObject leftHandItem;
    public GameObject rightHandItem;
    public float dashInertia;
    public float lungeInertia;
    public bool isLunging = false;
    public bool hasInertia = false;



    private void Awake()
    {
        playerControls = new Player_Controls();  

        playerControls.Land.Sprint.performed += context => {
            
        };
        playerControls.Land.Dash.performed += context => {
            OnDash(context);
        };
        playerControls.Land.LockOn.performed += context => {
            OnLockOn(context);
        };
        playerControls.Land.LockOn.canceled += context => {
            OnUnlock();
        };
        playerControls.Land.OpenInventory.performed += context => {
            OnOpenInventory(context);
        };
        playerControls.Land.PickUpItem.performed += context => {
            OnPickUpItem(context);
        };
        playerControls.Land.EquipLeftHand.performed += context => {
            OnEquipLeftHand(context);
        };
        playerControls.Land.EquipRightHand.performed += context => {
            OnEquipRightHand(context);
        };
        playerControls.Land.LeftHandAttack.started += context => {
            OnLeftHandAttackStarted();
        };
        playerControls.Land.RightHandAttack.started += context => {
            OnRightHandAttackStarted();
        };
        playerControls.Land.LeftHandAttack.performed += context => {
            OnLeftHandAttack();
        };
        playerControls.Land.RightHandAttack.performed += context => {
            OnRightHandAttack();
        };
    }
    void OnMove()
    {   
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Walking Ready Attack Right") || animator.GetCurrentAnimatorStateInfo(0).IsName("Walking Ready Attack Left"))
        {
            playerSpeed = slowSpeed;
        } else 
        {
            playerSpeed = regularSpeed;
        }
        Vector2 move = playerControls.Land.Move.ReadValue<Vector2>();
        if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Slide") && 
        !animator.GetCurrentAnimatorStateInfo(0).IsName("Follow Through Left") && 
        !animator.GetCurrentAnimatorStateInfo(0).IsName("Follow Through Right") &&
        (move.x != 0 || move.y != 0) && isLunging == false) 
        {
            hasInertia = true;
            //change facing direction
            if(!isLockedOn)
            {
                lookRotation = Quaternion.LookRotation(new Vector3(move.x, 0, move.y));
            } else {
                if(isLockedOn && lockedTarget != null) 
                {
                    lookRotation = Quaternion.LookRotation(new Vector3(lockedTarget.transform.position.x - playerTransform.position.x, 0, lockedTarget.transform.position.z - playerTransform.position.z));
                } else {
                    lookRotation = Quaternion.LookRotation(playerTransform.forward);                    
                }
            }
            //For rotating the player to face a new direction while dashing, to make it harder to turn while dashing and sliding
            if(animator.GetBool("dashing") || animator.GetCurrentAnimatorStateInfo(0).IsName("Slide") || animator.GetCurrentAnimatorStateInfo(0).IsName("Dash") && !isLockedOn) 
            {
                playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, lookRotation, turnSpeed / 3);
            } else {
                if(!isLockedOn)
                {
                    playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, lookRotation, turnSpeed);
                }
            }
            if(animator.GetBool("dashing") == false)
            {
                //move player in direction faced
                if(!isLockedOn && canMove)
                {
                    controller.Move(playerTransform.forward * Time.deltaTime * playerSpeed * currentInertia);
                } else if (canMove) {
                    controller.Move(new Vector3(move.x, 0, move.y) * Time.deltaTime * playerSpeed * currentInertia);
                }
                if(currentInertia < 1) 
                {
                    currentInertia += inertiaIncrement;
                }
                animator.SetBool("walking", true);
            } else {
                animator.SetBool("walking", false);
            }
        } else if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Slide") && 
        !animator.GetCurrentAnimatorStateInfo(0).IsName("Follow Through Right") && 
        !animator.GetCurrentAnimatorStateInfo(0).IsName("Follow Through Left") && 
        !animator.GetCurrentAnimatorStateInfo(0).IsName("Idle01") &&
        isLunging == false) {
            if(currentInertia > maxInertia ) 
            {
                if(!isLockedOn && canMove)
                {   
                    controller.Move(playerTransform.forward * Time.deltaTime * playerSpeed * currentInertia);
                } else if (canMove) {
                    controller.Move(new Vector3(move.x, 0, move.y) * Time.deltaTime * playerSpeed * currentInertia);
                }
                currentInertia -= inertiaIncrement;
            }
            animator.SetBool("walking", false);
        }
        if((animator.GetCurrentAnimatorStateInfo(0).IsName("Follow Through Left") || 
        animator.GetCurrentAnimatorStateInfo(0).IsName("Follow Through Right") ||
        animator.GetCurrentAnimatorStateInfo(0).IsName("Walking Ready Attack Right") ||
        animator.GetCurrentAnimatorStateInfo(0).IsName("Walking Ready Attack Left")) && 
        (move.x != 0 || move.y != 0) && !isLockedOn && isLunging == false)
        {
            //change facing direction
            lookRotation = Quaternion.LookRotation(new Vector3(move.x, 0, move.y));
            playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, lookRotation, turnSpeed / 10);
        }
        if(lockedTarget != null)
        {
            lookRotation = Quaternion.LookRotation(new Vector3(lockedTarget.transform.position.x - playerTransform.position.x, 0, lockedTarget.transform.position.z - playerTransform.position.z));
            playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, lookRotation, turnSpeed);
        }
    }


    void OnDash(InputAction.CallbackContext context)
    {
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Walking")) {
            if(gameManager.GetStamina() >= dashCost && !hasDashed)
            {
                hasDashed = true;
                StartCoroutine(DashCoroutine());
            } else {
                //play dud animation
            }
        }
    }


    void OnLockOn(InputAction.CallbackContext context)
    {
        isLockedOn = false;
        if(playerControls.Land.LockOn.ReadValue<float>() == 1) 
        {
            if(lockedTarget == null)
            {
                RaycastHit hit;
                //First cast ray to check in front
                if(Physics.SphereCast(playerTransform.position, 2f, playerTransform.forward, out hit, lockOnDistance))
                {
                    if(hit.collider.name == "Enemy")
                    {
                        lockedTarget = hit.collider.gameObject;
                        isLockedOn = true;
                    }
                } else {
                    //if nothing in front, go with closest enemy to the player
                    Collider[] colliders = Physics.OverlapSphere(playerTransform.position, lockOnDistance);
                    if(colliders.Length > 0)
                    {
                        foreach(Collider col in colliders) {
                            if(col.name == "Enemy") 
                            {
                                isLockedOn = true;
                                lockedTarget = col.gameObject;
                            }
                        }
                    }
                }
            }
            isLockedOn = true;
        } else {
            isLockedOn = false;
            lockedTarget = null;
        }
    }

    void OnUnlock()
    {
        isLockedOn = false;
        lockedTarget = null;
    }

    void OnOpenInventory(InputAction.CallbackContext context) {
        inventoryMenu.gameObject.SetActive(openInventory);
        openInventory = !openInventory;
    }

    void OnPickUpItem(InputAction.CallbackContext context)
    {
        if(gameManager.GetLoadedItem() != null)
        {
            if(CheckForAvailable())
            {
                gameManager.GetLoadedItem().GetComponent<Pick_Up>().PickUpItem();
                gameManager.RemoveLoadedItem();
                CheckForAvailable();
            }
        }
    }

    void OnEquipLeftHand(InputAction.CallbackContext context)
    {
        hotBarManager.EquipLeftSlot();
    }

    void OnEquipRightHand(InputAction.CallbackContext context)
    {
        hotBarManager.EquipRightSlot();
    }
    
    //Slow player speed while they're readying weapon to attack
    private IEnumerator SlowPlayerSpeed()
    {   
        float startTime = Time.time; // need to remember this to know how long to dash
        while(Time.time < startTime + 4f)
        {
            if(playerSpeed > slowSpeed)
            {
                playerSpeed -= .1f;
            }
            yield return null; // this will make Unity stop here and continue next frame
        }
    }

    //Increase player speed after they've released their attack
    private IEnumerator IncreasePlayerSpeed()
    {
        float startTime = Time.time; // need to remember this to know how long to dash
        while(Time.time < startTime + 4f)
        {
            if(playerSpeed < regularSpeed)
            {
                playerSpeed += .1f;
            }
            yield return null; // this will make Unity stop here and continue next frame
        }
    }

    void OnLeftHandAttackStarted()
    {
        leftHandItem = hotBarManager.GetSlotItem(0);
        if(leftHandItem != null)
        {
            Item item = leftHandItem.GetComponent<Item>();
            Item.Item_Type itemType = item.GetItemType();
            if(itemType == Item.Item_Type.Weapon || itemType == Item.Item_Type.Spell && item.GetStaminaCost() <= gameManager.GetStamina())
            {
                // if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Ready Attack"))
                // {
                //     animator.SetBool("attackLeft", false);
                //     animator.SetBool("readyLeft", true);
                //     StartCoroutine(SlowPlayerSpeed());
                //     turnSpeed = slowTurnSpeed;
                // }
            } 
        }    
        else if(gameManager.GetPunchCost() <= gameManager.GetStamina())
        {
            // if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Ready Attack"))
            // {
            //     animator.SetBool("attackLeft", false);
            //     animator.SetBool("readyLeft", true);
            //     StartCoroutine(SlowPlayerSpeed());
            //     turnSpeed = slowTurnSpeed;
            // }
        }
    }
    
    void OnRightHandAttackStarted()
    {
        rightHandItem = hotBarManager.GetSlotItem(1);
        if(rightHandItem != null)
        {
            Item item = rightHandItem.GetComponent<Item>();
            Item.Item_Type itemType = item.GetItemType();
            // if(itemType == Item.Item_Type.Weapon || itemType == Item.Item_Type.Spell && item.GetStaminaCost() <= gameManager.GetStamina())
            // {
            //     if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Ready Attack"))
            //     {
            //         //animator.SetBool("attackRight", false);
            //         //animator.SetBool("readyRight", true);
            //         StartCoroutine(IncreasePlayerSpeed());
            //         turnSpeed = slowTurnSpeed;
            //     }
            // }
        } 
        else if(gameManager.GetPunchCost() <= gameManager.GetStamina())
        {
            // if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Ready Attack"))
            // {
            //     animator.SetBool("attackRight", false);
            //     animator.SetBool("readyRight", true);
            //     StartCoroutine(IncreasePlayerSpeed());
            //     turnSpeed = slowTurnSpeed;
            // }
        }
    }
    void OnLeftHandAttack()
    {
        leftHandItem = hotBarManager.GetSlotItem(0);
        if(leftHandItem != null)
        {
            Item item = leftHandItem.GetComponent<Item>();
            Item.Item_Type itemType = item.GetItemType();
            if(itemType == Item.Item_Type.Weapon || itemType == Item.Item_Type.Spell && item.GetStaminaCost() <= gameManager.GetStamina())
            {
                //animator.SetBool("readyLeft", false);
                //animator.SetBool("attackLeft", true);
                playerSpeed = regularSpeed;
                turnSpeed = regularTurnSpeed;
                animator.Play(item.GetAnimationClip().name);
            } 
        }
        else if(gameManager.GetPunchCost() <= gameManager.GetStamina())
        {
            //animator.SetBool("readyLeft", false);
            //animator.SetBool("attackLeft", true);
            playerSpeed = regularSpeed;
            turnSpeed = regularTurnSpeed;
            //animator.Play("Follow Through Left");
        }
    }
    
    void OnRightHandAttack()
    {
        rightHandItem = hotBarManager.GetSlotItem(1);
        if(rightHandItem != null)
        {
            Item item = rightHandItem.GetComponent<Item>();
            Item.Item_Type itemType = item.GetItemType();
            if(itemType == Item.Item_Type.Weapon || itemType == Item.Item_Type.Spell && item.GetStaminaCost() <= gameManager.GetStamina())
            {
                //animator.SetBool("readyRight", false);
                //animator.SetBool("attackRight", true);
                playerSpeed = regularSpeed;
                turnSpeed = regularTurnSpeed;
                animator.Play(item.GetAnimationClip().name);
                //animator.Play("Follow Through Right");
            } 
        }
        else if(gameManager.GetPunchCost() <= gameManager.GetStamina())
        {
            animator.SetBool("readyRight", false);
            animator.SetBool("attackRight", true);
            playerSpeed = regularSpeed;
            turnSpeed = regularTurnSpeed;
            //animator.Play("Follow Through Right");
        }
    }

// private void OnDrawGizmos()
// {
//     Gizmos.color = Color.yellow;
//     Gizmos.DrawSphere(gameObject.GetComponentInChildren<Left_Hand>().transform.position, .5f);
//     Gizmos.DrawSphere(gameObject.GetComponentInChildren<Right_Hand>().transform.position, .5f);
// }
    public void OnAttack()
    {
        Item item = null;
        //Set values to be used for attack
        if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Follow Through Left") && !animator.GetCurrentAnimatorStateInfo(0).IsName("Follow Through Right"))
        {
            attackedList.Clear();
            hasAttackLunged = false;
            Hurt_Box[] hurtBoxes = playerTransform.GetComponentsInChildren<Hurt_Box>();
            foreach(Hurt_Box hurtBox in hurtBoxes)
            {
                hurtBox.SetDetect(false);
            }
        } else //Perform forward attack lunge
        if(!hasAttackLunged) 
        {
            if(animator.GetCurrentAnimatorStateInfo(0).IsName("Follow Through Left"))
            {
                if(leftHandItem != null)
                {
                    item = leftHandItem.GetComponent<Item>();
                    item.GetComponentInChildren<Hurt_Box>().SetDetect(true);
                }
            } else if(animator.GetCurrentAnimatorStateInfo(0).IsName("Follow Through Right"))
            {
                if(rightHandItem != null)
                {
                    item = rightHandItem.GetComponent<Item>();
                    item.GetComponentInChildren<Hurt_Box>().SetDetect(true);
                }
            }

            float staminaCost;
            if(item != null)
            {
                staminaCost = item.GetStaminaCost();
            } else {
                staminaCost = gameManager.GetPunchCost();
            }

            //gameManager.DepleteStamina(staminaCost);
            Vector2 move = playerControls.Land.Move.ReadValue<Vector2>();
            if(move.x != 0 || move.y != 0)
            {
                StartCoroutine(AttackLungeCoroutine(_lungeTime));
            } else {
                StartCoroutine(AttackLungeCoroutine(_idleLungeTime));
            }
            hasAttackLunged = true;
        }
    }
    
    public bool CheckForAvailable()
    {
        bool somethingThere = false;
        Collider[] cols = Physics.OverlapSphere(gameObject.transform.position, 1.5f);
        foreach(Collider col in cols) 
        {
            if(col.GetComponent<Pick_Up>() != null)
            {
                gameManager.AddLoadedItem(col.gameObject);
                somethingThere = true;
            }
        }
        if(somethingThere == false)
        {
            gameManager.RemoveLoadedItem();
        }
        return somethingThere;
    }

    void Start()
    {
        gameManager = GameManager.Instance;
        hotBarManager = gameManager.HotbarManager;
        currentInertia = maxInertia;
        controller = gameObject.GetComponent<CharacterController>();
        animator = gameObject.GetComponent<Animator>();
        playerTransform = gameObject.GetComponent<Transform>();
        dashParticles = GetComponentInChildren<Dash_Cloud>().GetComponent<ParticleSystem>();
        skidParticles = GetComponentInChildren<Skid_Cloud>().GetComponent<ParticleSystem>();
        fireBreath = GetComponentInChildren<Front_Middle_Spell_Cast>().GetComponent<ParticleSystem>();
        attackedList = new HashSet<string>();
        lockedTarget = null;
        sideDashRotation = new Quaternion();
        dashTransform = GetComponentInChildren<Dash_Rotator>().transform;
        inventoryMenu.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();   
    }

    void FixedUpdate()
    {
        OnMove();
        OnAttack();
    }

    public bool GetLockedStatus() 
    {
        return isLockedOn;
    }
    private IEnumerator DashCoroutine()
    {
        //gameManager.DepleteStamina(dashCost);
        Vector2 lastLooked = new Vector2(0,0);
        animator.SetBool("doneDashing", false);
        Vector2 move = playerControls.Land.Move.ReadValue<Vector2>();
        sideDashRotation = Quaternion.LookRotation(new Vector3(move.x, 0, move.y));
        dashTransform.rotation = Quaternion.Slerp(dashTransform.rotation, sideDashRotation, turnSpeed / 3);

        float startTime = Time.time; // need to remember this to know how long to dash
        while(Time.time < startTime + _dashTime)
        {
            animator.SetBool("dashing", true);
            move = playerControls.Land.Move.ReadValue<Vector2>();
            if(move.x != 0 || move.y != 0)
            {
                sideDashRotation = Quaternion.LookRotation(new Vector3(move.x, 0, move.y));
                lastLooked = move;
            } else {
                sideDashRotation = Quaternion.LookRotation(new Vector3(lastLooked.x, 0, lastLooked.y));
            }
            
            dashTransform.rotation = Quaternion.Slerp(dashTransform.rotation, sideDashRotation, turnSpeed / 3);

            //Wind up
            if(startTime + 0.15f > Time.time) {
                if(isLockedOn) 
                {
                    controller.Move(dashTransform.forward * Time.deltaTime * playerSpeed);    
                } else {
                    controller.Move(playerTransform.forward * Time.deltaTime * playerSpeed);
                }
            }
            //Wait 0.2f seconds and then dash effect
            else
            if(startTime + 0.15f < Time.time && !(startTime + _dashTime - 0.05f < Time.time)) 
            {
                dashParticles.Play();
                if(isLockedOn) 
                {
                    controller.Move(dashTransform.forward * Time.deltaTime * playerSpeed * dashMultiplier);    
                } else {
                    controller.Move(playerTransform.forward * Time.deltaTime * playerSpeed * dashMultiplier);
                }
            }
            else
            //Transition into slide
            dashInertia = maxInertia;
            if(startTime + _dashTime - 0.05f < Time.time) 
            {
                dashParticles.Stop();
                animator.SetBool("doneDashing", true);
                if(isLockedOn && move.x != 0 || move.y != 0) 
                {
                    controller.Move(dashTransform.forward * Time.deltaTime * playerSpeed * dashInertia);    
                } else if(move.x != 0 || move.y != 0) {
                    controller.Move(playerTransform.forward * Time.deltaTime * playerSpeed * dashInertia);
                }
                if(dashInertia > maxInertia) 
                {
                    dashInertia -= inertiaIncrement;
                }
                skidParticles.Play();
            }
            yield return null; // this will make Unity stop here and continue next frame
        }
        animator.SetBool("dashing", false);
        hasDashed = false;
    }

    private IEnumerator AttackLungeCoroutine(double lungeTime) 
    {
        animator.SetBool("walking", false);
        isLunging = true;
        hasInertia = false;
        float startTime = Time.time; // need to remember this to know how long to dash
        lungeInertia = 1;
        while(Time.time < startTime + lungeTime)
        {
            if(startTime + lungeTime/2 > Time.time)
            {
                controller.Move(playerTransform.forward * Time.deltaTime * playerSpeed);
            } else {
                if(lungeInertia > maxInertia) 
                {
                    controller.Move(playerTransform.forward * Time.deltaTime * playerSpeed * lungeInertia);
                    lungeInertia -= inertiaIncrement;
                }
            }
            yield return null; // this will make Unity stop here and continue next frame
        }
        hasAttackLunged = true;
        isLunging = false;
        hasInertia = false;
    }

    private void Update() {
        //Apply gravity to player
        controller.Move(new Vector3(0, -150/playerTransform.position.y, 0) * Time.deltaTime);
        animator.SetBool("isGrounded", controller.isGrounded);
    }
}
