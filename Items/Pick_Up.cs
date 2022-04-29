using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pick_Up : MonoBehaviour
{
    private GameManager gameManager;
    private Inventory_List Inventory;
    private Item.Item_Size itemSize;
    
    void Start()
    {
        gameManager = GameManager.Instance;
        Inventory = gameManager.Inventory; 

        if(gameObject.GetComponent<Item>() != null)
        {
            itemSize = gameObject.GetComponent<Item>().GetItemSize();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Player")
        {
            gameManager.AddLoadedItem(gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.name == "Player")
        {
            if(gameManager.loadedItem != null && gameManager.loadedItem.gameObject == other.gameObject)
            {
                gameManager.RemoveLoadedItem();
            }
        }
    }

    public void PickUpItem()
    {
        Inventory.AddItemToInventory(gameObject, itemSize);
    }

    public Item.Item_Size GetItemSize()
    {
        return itemSize;
    }
}
