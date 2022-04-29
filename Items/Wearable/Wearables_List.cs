using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wearables_List : MonoBehaviour
{
    public Wearable_Item IronHelmet;
    public GameObject ironHelmet;
    public Wearable_Item LeatherGloves;
    public GameObject leatherGloves;
    public Wearable_Item GoldChestplate;
    public GameObject goldChestplate;

    private void Awake()
    {
        // IronHelmet = new Wearable_Item("Iron Helmet", 10, 100, 12, Wearable_Item.WearLocation.Head, ironHelmet);
        // LeatherGloves = new Wearable_Item("Leather Gloves", 3, 100, 4, Wearable_Item.WearLocation.Hands, leatherGloves);
        // GoldChestplate = new Wearable_Item("Gold Chestplate", 25, 100, 50, Wearable_Item.WearLocation.Torso, goldChestplate);
    }
    void Update()
    {
        
    }
}
