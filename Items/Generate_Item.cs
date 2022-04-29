using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate_Item : MonoBehaviour
{
    //Grab item list instances
    private Weapons_List weaponList;
    private Pouch_Items_List pouchList;
    private Wearables_List wearableList;
    private Magic_Items_List magicList;
    private Spell_Books_List spellList;
    private Craftables_List craftableList;
    
    //Grab item classes
    private Item item;
    private Weapon_Item weapon;
    private Pouch_Item pouch;
    private Wearable_Item wearable;
    private Magic_Item magic;
    private Spell_Book_Item spell;
    private Crafting_Item craftable;

    public enum ItemTypes {
        Item,
        WeaponItem,
        WearableItem,
        MagicItem,
        SpellBookItem,
        CraftableItem,
    }

    private GameObject itemGenerated;

    void Start()
    {
        weaponList = Weapons_List.Instance;
        pouchList = Pouch_Items_List.Instance;
        // wearableList = Wearables_List.Instance;
        // magicList = Magic_Items_List.Instance;
        // spellList = Spell_Books_List.Instance;
        // craftableList = Craftables_List.Instance;
    }

    public GameObject GenerateWeapon(Weapons_List.WeaponNames weapon, float damage, float durability, float weight, Weapon_Item.Reach reach, Item.Item_Size size)
    {
        return weaponList.CreateNewWeapon(weapon, damage, durability, weight, reach, size);
    }

    public GameObject GeneratePouch(Pouch_Items_List.PouchNames pouch, float weight)
    {
        return pouchList.CreateNewPouch(pouch, weight);
    }
}
