using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pouch_Items_List : MonoBehaviour
{
    public static Pouch_Items_List Instance;
    public enum PouchNames {
        SinglePouch,
        DoublePouch,
        NovicesPouch,
        SpecialistsPouch,
        ExplorersPouch,
        AdventurersPouch,
    }

    public List<KeyValuePair<PouchNames, Single_Pouch>> PouchObjectList = new List<KeyValuePair<PouchNames, Single_Pouch>>();
    public Single_Pouch SinglePouchInfo;
    public Single_Pouch DoublePouchInfo;
    public Single_Pouch NovicesPouchInfo;
    public Single_Pouch SpecialistsPouchInfo;
    public Single_Pouch ExplorersPouchInfo;
    public Single_Pouch AdventurersPouchInfo;
    public GameObject SinglePouch;
    public GameObject DoublePouch;
    public GameObject NovicesPouch;
    public GameObject SpecialistsPouch;
    public GameObject ExplorersPouch;
    public GameObject AdventurersPouch;
    private Single_Pouch newPouch;
    
    private void Awake()
    {
        Instance = this;

        SinglePouchInfo = new Single_Pouch("Single Pouch", SinglePouch);
        DoublePouchInfo = new Single_Pouch("Double Pouch", DoublePouch);
        NovicesPouchInfo = new Single_Pouch("Novices Pouch", NovicesPouch);
        SpecialistsPouchInfo = new Single_Pouch("Specialists Pouch", SpecialistsPouch);
        ExplorersPouchInfo = new Single_Pouch("Explorers Pouch", ExplorersPouch);
        AdventurersPouchInfo = new Single_Pouch("Adventurers Pouch", AdventurersPouch);

        //Add pouches to key value pairs to be able to find them when creating new weapons using WeaponName variable
        PouchObjectList.Add(new KeyValuePair<PouchNames, Single_Pouch>(PouchNames.SinglePouch, SinglePouchInfo));
        PouchObjectList.Add(new KeyValuePair<PouchNames, Single_Pouch>(PouchNames.DoublePouch, DoublePouchInfo));
        PouchObjectList.Add(new KeyValuePair<PouchNames, Single_Pouch>(PouchNames.NovicesPouch, NovicesPouchInfo));
        PouchObjectList.Add(new KeyValuePair<PouchNames, Single_Pouch>(PouchNames.SpecialistsPouch, SpecialistsPouchInfo));
        PouchObjectList.Add(new KeyValuePair<PouchNames, Single_Pouch>(PouchNames.ExplorersPouch, ExplorersPouchInfo));
        PouchObjectList.Add(new KeyValuePair<PouchNames, Single_Pouch>(PouchNames.AdventurersPouch, AdventurersPouchInfo));
    }
    
    public GameObject CreateNewPouch(PouchNames pouch, float weight)
    {
        switch (pouch)
        {
            case PouchNames.SinglePouch:
            {
                SinglePouch.GetComponent<Pouch_Item>().SetInitialValues(SinglePouchInfo.name, Pouch_Item.PouchTypes.OneByOne, weight);
                return SinglePouch;
            }
            case PouchNames.DoublePouch:
            {
                DoublePouch.GetComponent<Pouch_Item>().SetInitialValues(DoublePouchInfo.name, Pouch_Item.PouchTypes.OneByTwo, weight);
                return DoublePouch;
            }
            case PouchNames.NovicesPouch:
            {
                NovicesPouch.GetComponent<Pouch_Item>().SetInitialValues(NovicesPouchInfo.name, Pouch_Item.PouchTypes.TwoByTwo, weight);
                return NovicesPouch;
            }
            case PouchNames.SpecialistsPouch:
            {
                SpecialistsPouch.GetComponent<Pouch_Item>().SetInitialValues(SpecialistsPouchInfo.name, Pouch_Item.PouchTypes.ThreeByOne, weight);
                return SpecialistsPouch;
            }
            case PouchNames.ExplorersPouch:
            {
                ExplorersPouch.GetComponent<Pouch_Item>().SetInitialValues(ExplorersPouchInfo.name, Pouch_Item.PouchTypes.ThreeByTwo, weight);
                return ExplorersPouch;
            }
            case PouchNames.AdventurersPouch:
            {
                AdventurersPouch.GetComponent<Pouch_Item>().SetInitialValues(AdventurersPouchInfo.name, Pouch_Item.PouchTypes.ThreeByThree, weight);
                return AdventurersPouch;
            }
            default: return new GameObject();
        }
    }
}