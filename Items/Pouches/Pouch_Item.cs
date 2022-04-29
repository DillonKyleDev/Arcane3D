using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pouch_Item : Item
{
    public enum PouchTypes {
        OneByOne,
        OneByTwo,
        TwoByTwo,
        ThreeByOne,
        ThreeByTwo,
        ThreeByThree,
    }

    public int capacity = 1;
    public GameObject[,] storage;
    public PouchTypes pouchType;
    public class PouchSizeInfo {
        public PouchTypes pouchType;
        public Item.Item_Size pouchSize;
        public int capacity;
        public PouchSizeInfo(PouchTypes pType, Item.Item_Size size, int cap)
        {
            pouchType = pType;
            pouchSize = size;
            capacity = cap;
        }
    }

    public List<PouchSizeInfo> pouchSizeInfos = new List<PouchSizeInfo>();

    public Pouch_Item() : base(Item_Type.Pouch, true, false, true) 
    {
        //Constructor
    }

    public void SetInitialValues(string pouchName, PouchTypes pType, float wght)
    {
        pouchSizeInfos.Add(new PouchSizeInfo(PouchTypes.OneByOne, Item.Item_Size.Small, 1));
        pouchSizeInfos.Add(new PouchSizeInfo(PouchTypes.OneByTwo, Item.Item_Size.Small, 2));
        pouchSizeInfos.Add(new PouchSizeInfo(PouchTypes.TwoByTwo, Item.Item_Size.Medium, 4));
        pouchSizeInfos.Add(new PouchSizeInfo(PouchTypes.ThreeByOne, Item.Item_Size.Small, 3));
        pouchSizeInfos.Add(new PouchSizeInfo(PouchTypes.ThreeByTwo, Item.Item_Size.Medium, 6));
        pouchSizeInfos.Add(new PouchSizeInfo(PouchTypes.ThreeByThree, Item.Item_Size.Large, 9));

        PouchSizeInfo info = pouchSizeInfos.Find(pouch => pouch.pouchType == pType);
        base.itemName = pouchName;
        base.itemSize = info.pouchSize;
        base.weight = wght;
        capacity = info.capacity;
        pouchType = info.pouchType;

    }
}
