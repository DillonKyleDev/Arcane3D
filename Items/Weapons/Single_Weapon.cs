using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Single_Weapon
{
    public string name;
    public GameObject item;
    public Single_Weapon(string itemName, GameObject physicalItem)
    {
        name = itemName;
        item = physicalItem;
    }
}
