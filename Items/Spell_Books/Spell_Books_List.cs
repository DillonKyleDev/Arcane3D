using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_Books_List : MonoBehaviour
{
    public Spell_Book_Item FirePlume;
    public GameObject firePlume;
    public Spell_Book_Item EtherialStorm;
    public GameObject etherialStorm;
    private void Awake()
    {
        // FirePlume = new Spell_Book_Item("Fire Plume", 30, 20, 10, 5, Spell_Book_Item.Spell_Type.Fire, firePlume);
        // EtherialStorm = new Spell_Book_Item("Etherial Storm", 100, 20, 50, 20, Spell_Book_Item.Spell_Type.Air, etherialStorm);
    }

    private void Update()
    {
        
    }
}
