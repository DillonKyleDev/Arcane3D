using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons_List : MonoBehaviour
{
    public static Weapons_List Instance;
    public enum WeaponNames {
        ShortSword,
        LongSword,
        Mace,
    }

    public List<KeyValuePair<WeaponNames, Single_Weapon>> WeaponObjectList = new List<KeyValuePair<WeaponNames, Single_Weapon>>();
    public Single_Weapon ShortSwordInfo;
    public Single_Weapon LongSwordInfo;
    public Single_Weapon MaceInfo;
    public GameObject ShortSword;
    public GameObject LongSword;
    public GameObject Mace;
    private Weapon_Item newWeapon;
    
    private void Awake()
    {
        Instance = this;

        ShortSwordInfo = new Single_Weapon("Short Sword", ShortSword);
        LongSwordInfo = new Single_Weapon("Long Sword", LongSword);
        MaceInfo = new Single_Weapon("Mace", Mace);

        //Add weapons to key value pairs to be able to find them when creating new weapons using WeaponName variable
        WeaponObjectList.Add(new KeyValuePair<WeaponNames, Single_Weapon>(WeaponNames.ShortSword, ShortSwordInfo));
        WeaponObjectList.Add(new KeyValuePair<WeaponNames, Single_Weapon>(WeaponNames.LongSword, LongSwordInfo));
        WeaponObjectList.Add(new KeyValuePair<WeaponNames, Single_Weapon>(WeaponNames.Mace, MaceInfo));
    }
    
    public GameObject CreateNewWeapon(WeaponNames weapon, float damage, float durability, float weight, Weapon_Item.Reach reach, Item.Item_Size size)
    {
        switch (weapon)
        {
            case WeaponNames.LongSword:
            {
                LongSword.GetComponent<Weapon_Item>().SetInitialValues("Long Sword", damage, durability, weight, reach, size);
                return LongSword;
            }
            case WeaponNames.ShortSword:
            {
                ShortSword.GetComponent<Weapon_Item>().SetInitialValues("Short Sword", damage, durability, weight, reach, size);
                return ShortSword;
            }
            case WeaponNames.Mace:
            {
                Mace.GetComponent<Weapon_Item>().SetInitialValues("Mace", damage, durability, weight, reach, size);
                return Mace;
            }
            default: return new GameObject();
        }
    }
}