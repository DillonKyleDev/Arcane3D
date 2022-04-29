using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wearable_Item : Item
{
    public enum WearLocation {
        Head,
        Torso,
        Hands,
        Legs,
        Feet,
        Finger,
    }

    public float defense;
    public float durability;
    public WearLocation wearLocation;
    public GameObject armorPiece;

    public Wearable_Item() : base(Item_Type.Wearable, true, false, false) 
    {
        //Constructor
    }

    public void SetInitialValues(float def, float dur, float wght, WearLocation loc, GameObject piece)
    {
        defense = def;
        durability = dur;
        base.weight = wght;
        wearLocation = loc;
        armorPiece = piece;
    }

    public void SetDefense(float def)
    {
        defense = def;
    }

    public void SetDurability(float dur)
    {
        durability = dur;
    }

    public void SetWearLocation(WearLocation loc)
    {
        wearLocation = loc;
    }

    public float GetDefense()
    {
        return defense;
    }

    public float GetDurability()
    {
        return durability;
    }

    public WearLocation GetWearLocation()
    {
        return wearLocation;
    }
}
