using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Generator : MonoBehaviour
{
    public enum GeneratedItem {
        Weapon,
        Pouch,
    }
    public GeneratedItem ItemToGenerateItem;
    private Generate_Item generateItem;
    public Transform spawnLocation;
    private GameObject item;
    
    //For weapons
    public Weapons_List.WeaponNames Weapon = Weapons_List.WeaponNames.LongSword;
    public Weapon_Item.Reach Reach = Weapon_Item.Reach.Long;
    public Item.Item_Size Size = Item.Item_Size.Large;
    public float WeaponDamage = 25;
    public float WeaponDurability = 100;
    public float WeaponWeight = 50;
    //For pouches
    public Pouch_Items_List.PouchNames Pouch = Pouch_Items_List.PouchNames.SinglePouch;
    public float PouchWeight = 5;
    void Start()
    {
        gameObject.AddComponent<Generate_Item>();
        generateItem = gameObject.GetComponent<Generate_Item>();
        spawnLocation = gameObject.transform;
    }   

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Player")
        {
            //Spawn sword
            if(ItemToGenerateItem == GeneratedItem.Weapon)
            {
                Vector3 spawnLocation = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 2, gameObject.transform.position.z);
                item = generateItem.GenerateWeapon(Weapon, WeaponDamage, WeaponDurability, WeaponWeight, Reach, Size);
                Instantiate(item, spawnLocation, gameObject.transform.rotation).gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0f, 300f, 1f));
            }

            //Spawn pouch
            if(ItemToGenerateItem == GeneratedItem.Pouch)
            {
                Vector3 spawnLocation = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 2, gameObject.transform.position.z);
                item = generateItem.GeneratePouch(Pouch, PouchWeight);
                Instantiate(item, spawnLocation, gameObject.transform.rotation).gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0f, 300f, 100f));
            }
        }
    }
}
