using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats_Manager : MonoBehaviour
{
    private float strength = 1;
    private float intelligence = 1;
    private float wisdom = 1;
    private float constitution = 1;
    private float defense = 1;
    private float dexterity = 1;
    

    public float AdjustStaminaCost(float originalCost) 
    {
        return originalCost / strength * 5;
    }

    public void SetAllStats(float str, float intel, float wis, float cons, float def, float dex)
    {
        strength = str;
        intelligence = intel;
        wisdom = wis;
        constitution = cons;
        defense = def;
        dexterity = dex;
    }

    public void SetStrength(float str)
    {
        strength = str;
    }

    public void SetIntelligence(float intel)
    {
        intelligence = intel;
    }

    public void SetWisdom(float wis)
    {
        wisdom = wis;
    }

    public void SetConstitution(float cons)
    {
        constitution = cons;
    }

    public void SetDefense(float def)
    {
        defense = def;
    }

    public void SetDexterity(float dex)
    {
        dexterity = dex;
    }

    public float GetStrength(float str)
    {
        return strength;
    }

    public float GetIntelligence(float intel)
    {
        return intelligence;
    }

    public float GetWisdom(float wis)
    {
        return wisdom;
    }

    public float GetConstitution(float cons)
    {
        return constitution;
    }

    public float GetDefense(float def)
    {
        return defense;
    }

    public float GetDexterity(float dex)
    {
        return dexterity;
    }

}
