using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items_On_Waist : MonoBehaviour
{
    private GameManager gameManager;
    public static Items_On_Waist Instance;
    private Inventory_List inventory;
    public Transform leftSideSlot;
    public Transform rightSideSlot;
    public Transform slot1;
    public Transform slot2;
    public Transform slot3;
    public Transform slot4;
    

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        gameManager = GameManager.Instance;
        inventory = gameManager.Inventory;
        
        leftSideSlot = GetComponentInChildren<Left_Side_Slot>().transform;
        rightSideSlot = GetComponentInChildren<Right_Side_Slot>().transform;
        slot1 = GetComponentInChildren<Slot1>().transform;
        slot2 = GetComponentInChildren<Slot2>().transform;
        slot3 = GetComponentInChildren<Slot3>().transform;
        slot4 = GetComponentInChildren<Slot4>().transform;
    }

    public void UpdateWaist() 
    {

    }

}
