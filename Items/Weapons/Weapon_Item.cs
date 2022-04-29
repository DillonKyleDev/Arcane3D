using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Item : Item
{
    public enum Reach {
        Short,
        Medium,
        Long,
        Ranged
    }

    public float damage = 1;
    public float durability = 100;
    public Reach reach = Reach.Short;
    public Weapon_Item() : base(Item_Type.Weapon, true, false, false) 
    {
        //Constructor
    }

    public void SetInitialValues(string weaponName, float dmg, float dur, float wght, Reach rch, Item.Item_Size size)
    {
        base.itemName = weaponName;
        base.itemSize = size;
        base.weight = wght;
        damage = dmg;
        durability = dur;
        reach = rch;
    }

    public void SetDamage(float dmg)
    {
        damage = dmg;
    }

    public void SetReach(Reach rch)
    {
        reach = rch;
    }

    public float GetDamage()
    {
        return damage;
    }

    public Reach GetReach()
    {
        return reach;
    }
}
