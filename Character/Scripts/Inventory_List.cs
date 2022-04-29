using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_List : MonoBehaviour
{   
    private GameManager gameManager;
    private Weapons_List Weapons;
    private Items_On_Back ItemsOnBack;
    private Items_On_Waist ItemsOnWaist;
    private Hot_Bar_Manager HotbarManager;
 
    private List<GameObject> ItemsInInventory = new List<GameObject>();
    public List<GameObject> equippedWeapons = new List<GameObject>();

    void Start()
    {
        gameManager = GameManager.Instance;
        Weapons = gameManager.Weapons;
        ItemsOnBack = gameManager.ItemsOnBack;
        ItemsOnWaist = gameManager.ItemsOnWaist;
        HotbarManager = gameManager.HotbarManager;
    }

    public List<GameObject> GetWeaponsInInventory()
    {
        return ItemsInInventory;
    }

    public void AddItemToInventory(GameObject addedItem, Item.Item_Size itemSize)
    {
        if(itemSize == Item.Item_Size.Small)
        {
            if(AddToSmallSlots(addedItem) == true)
            {
                ItemsInInventory.Add(addedItem);
                gameManager.RemoveLoadedItem();
            }
        }
        else
        if(itemSize == Item.Item_Size.Medium || itemSize == Item.Item_Size.Large)
        {
            if(AddToMediumSlots(addedItem) == true)
            {
                ItemsInInventory.Add(addedItem);
                gameManager.RemoveLoadedItem();
            } else if(HotbarManager.RightHandFree())
            {
                HotbarManager.AddRightHand(addedItem);
                ItemsInInventory.Add(addedItem);
                gameManager.RemoveLoadedItem();
            } else if(HotbarManager.LeftHandFree())
            {
                HotbarManager.AddLeftHand(addedItem);
                ItemsInInventory.Add(addedItem);
                gameManager.RemoveLoadedItem();
            }
        }
    }

    public bool AddToMediumSlots(GameObject addedItem)
    {
        if(HotbarManager.SlotFree(2))
        {
            HotbarManager.AddLeftSlot(addedItem);
            return true;
        } else if(HotbarManager.SlotFree(3))
        {
            HotbarManager.AddRightSlot(addedItem);
            return true;
        } else if(HotbarManager.RightHandFree())
        {
            HotbarManager.AddRightHand(addedItem);
            return true;
        } else if(HotbarManager.LeftHandFree())
        {
            HotbarManager.AddLeftHand(addedItem);
            return true;
        } else return false;
    }

    public bool AddToSmallSlots(GameObject addedItem)
    {
        if(HotbarManager.SlotFree(5))
        {
            HotbarManager.AddSlot1(addedItem);
            return true;
        } else if(HotbarManager.SlotFree(6))
        {
            HotbarManager.AddSlot2(addedItem);
            return true;
        } else if(HotbarManager.SlotFree(7))
        {
            HotbarManager.AddSlot3(addedItem);
            return true;
        } else if(HotbarManager.SlotFree(8))
        {
            HotbarManager.AddSlot4(addedItem);
            return true;
        } else return false;
    }

    public bool AddToBack(GameObject addedItem)
    {
        if(HotbarManager.SlotFree(4))
        {
            return false;
            // HotbarManager.AddBack(addedItem);
            // return true;
        }
        return false;
    }

    public void RemoveItemFromInventory(Item removeItem)
    {
        //remove item
    }

    public void EquipWeapon(GameObject weapon, int equipIndex)
    {
        weapon.GetComponent<Weapon_Item>().SetIsEquipped(true);
        weapon.GetComponent<Weapon_Item>().SetEquippedLocation(equipIndex);
    }   

    public void UnequipWeapon(GameObject weapon)
    {
        weapon.GetComponent<Weapon_Item>().SetIsEquipped(false);
    }

    public void UpdateEquippedWeapons()
    {
        equippedWeapons.Clear();
        foreach(GameObject item in ItemsInInventory)
        {
            if(item.GetComponent<Weapon_Item>().GetIsEquipped())
            {
                equippedWeapons.Add(item);
            }
        }
        ItemsOnBack.UpdateBack();
    }

    public List<GameObject> GetEquippedWeapons()
    {
        return equippedWeapons;
    }

    private void Update()
    {
  
    }
}
