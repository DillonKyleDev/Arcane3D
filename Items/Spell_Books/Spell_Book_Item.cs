using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_Book_Item : Item
{
    public enum Spell_Type {
        Water,
        Fire,
        Earth,
        Air,
        Life,
        Death,
        Entropy,
    }

    public float damage;
    public float manaCost;
    public float energyCost;
    public Spell_Type spellType;
    public GameObject effect;
    public Spell_Book_Item() : base(Item_Type.Weapon, true, false, false) 
    {
        //Constructor
    }

    public void SetInitialValues(float dmg, float mnaCost, float engCost, float wght, Spell_Type type, GameObject spellObject)
    {
        damage = dmg;
        manaCost = mnaCost;
        energyCost = engCost;
        base.weight = wght;
        spellType = type;
        effect = spellObject;
    }
    
    public void SetDamage(float dmg)
    {
        damage = dmg;
    }

    public void SetManaCost(float cost)
    {
        manaCost = cost;
    }

    public void SetEnergyCost(float cost)
    {
        energyCost = cost;
    }

    public void SetType(Spell_Type type)
    {
        spellType = type;
    }
    
    public float GetDamage()
    {
        return damage;
    }

    public float GetManaCost()
    {
        return manaCost;
    }

    public Spell_Type GetSpellType()
    {
        return spellType;
    }

    public GameObject GetEffect()
    {
        return effect;
    }
}
