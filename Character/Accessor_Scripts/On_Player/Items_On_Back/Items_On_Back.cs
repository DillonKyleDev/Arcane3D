using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items_On_Back : MonoBehaviour
{
    public static Items_On_Back Instance;
    private GameManager gameManager;
    private Inventory_List inventory;
    private List<GameObject> weaponsOnPlayer = new List<GameObject>();
    private List<GameObject> itemsOnPlayer = new List<GameObject>();
    private List<Transform> equipLocations = new List<Transform>();
    private Transform topLeft;
    public Transform topRight;
    private Transform bottomLeft;
    private Transform bottomRight;
    private Transform topMiddle;
    private Transform middleMiddle;
    private Transform bottomMiddle;
    private Transform leftMiddle;
    private Transform rightMiddle;

    private GameObject tempObject;
    private GameObject topLeftItem;
    private GameObject topRightItem;
    private GameObject bottomLeftItem;
    private GameObject bottomRightItem;
    private GameObject topMiddleItem;
    private GameObject middleMiddleItem;
    private GameObject bottomMiddleItem;
    private GameObject leftMiddleItem;
    private GameObject rightMiddleItem;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        gameManager = GameManager.Instance;
        inventory = gameManager.Inventory;

        topLeft = GetComponentInChildren<Top_Left>().transform;
        topRight = GetComponentInChildren<Top_Right>().transform;
        bottomLeft = GetComponentInChildren<Bottom_Left>().transform;
        bottomRight = GetComponentInChildren<Bottom_Right>().transform;
        topMiddle = GetComponentInChildren<Top_Middle>().transform;
        middleMiddle = GetComponentInChildren<Middle_Middle>().transform;
        bottomMiddle = GetComponentInChildren<Bottom_Middle>().transform;
        leftMiddle = GetComponentInChildren<Left_Middle>().transform;
        rightMiddle = GetComponentInChildren<Right_Middle>().transform;

        //Assign equip locations to transforms
        equipLocations.Add(topLeft);
        equipLocations.Add(topRight);
        equipLocations.Add(bottomLeft);
        equipLocations.Add(bottomRight);
        equipLocations.Add(topMiddle);
        equipLocations.Add(middleMiddle);
        equipLocations.Add(bottomMiddle);
        equipLocations.Add(leftMiddle);
        equipLocations.Add(rightMiddle);
    }

    public void UpdateBack()
    {   
        //Weapons
        weaponsOnPlayer = inventory.GetEquippedWeapons();
        foreach(GameObject weapon in weaponsOnPlayer)
        {
            int locationIndex = weapon.GetComponent<Weapon_Item>().GetEquippedLocation();
            Transform transf = equipLocations[locationIndex];

            tempObject = Instantiate(weapon, transf, transf);
            tempObject.transform.position = transf.position;
        }
    }
    private void Update()
    {

    }
}
