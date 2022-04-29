using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Single_Pouch
{
    public string name;
    public GameObject item;
    public Single_Pouch(string pouchName, GameObject physicalItem)
    {
        name = pouchName;
        item = physicalItem;
    }
}
