using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hot_Bar_Manager : MonoBehaviour
{
    private GameManager gameManager;
    private Inventory_List Inventory;
    private Items_On_Back ItemsOnBack;
    public GameObject hotbarMenu;
    private KeyValuePair<GameObject, GameObject> leftHandEquipped = new KeyValuePair<GameObject, GameObject>();
    private KeyValuePair<GameObject, GameObject> rightHandEquipped = new KeyValuePair<GameObject, GameObject>();
    private KeyValuePair<GameObject, GameObject> backSlot = new KeyValuePair<GameObject, GameObject>();
    private KeyValuePair<GameObject, GameObject> leftHandSlot = new KeyValuePair<GameObject, GameObject>();
    private KeyValuePair<GameObject, GameObject> rightHandSlot = new KeyValuePair<GameObject, GameObject>();
    private KeyValuePair<GameObject, GameObject> slot1 = new KeyValuePair<GameObject, GameObject>();
    private KeyValuePair<GameObject, GameObject> slot2 = new KeyValuePair<GameObject, GameObject>();
    private KeyValuePair<GameObject, GameObject> slot3 = new KeyValuePair<GameObject, GameObject>();
    private KeyValuePair<GameObject, GameObject> slot4 = new KeyValuePair<GameObject, GameObject>();
    private GameObject selectedBeltSlot;
    public List<KeyValuePair<GameObject, GameObject>> slotList = new List<KeyValuePair<GameObject, GameObject>>();
    public List<GameObject> playerCharacterSlots = new List<GameObject>();
    public GameObject CharacterBeltInstance;
    public GameObject playerLeftHand;
    public GameObject playerRightHand;
    private GameObject playerBackSlot;
    private GameObject playerLeftSlot;
    private GameObject playerRightSlot;
    //private GameObject playerBackSlot; 
    private GameObject playerSlot1;
    private GameObject playerSlot2;
    private GameObject playerSlot3;
    private GameObject playerSlot4;

    void Start()
    {
        gameManager = GameManager.Instance;
        Inventory = gameManager.Inventory;

        leftHandEquipped = new KeyValuePair<GameObject, GameObject>(hotbarMenu.GetComponentInChildren<Left_Hand_Equipped>().gameObject, null);
        rightHandEquipped = new KeyValuePair<GameObject, GameObject>(hotbarMenu.GetComponentInChildren<Right_Hand_Equipped>().gameObject, null);
        backSlot = new KeyValuePair<GameObject, GameObject>(hotbarMenu.GetComponentInChildren<Back_Slot>().gameObject, null);
        leftHandSlot = new KeyValuePair<GameObject, GameObject>(hotbarMenu.GetComponentInChildren<Left_Hand_Hotkey>().gameObject, null);
        rightHandSlot = new KeyValuePair<GameObject, GameObject>(hotbarMenu.GetComponentInChildren<Right_Hand_Hotkey>().gameObject, null);
        slot1 = new KeyValuePair<GameObject, GameObject>(hotbarMenu.GetComponentInChildren<Belt_1>().gameObject, null);
        slot2 = new KeyValuePair<GameObject, GameObject>(hotbarMenu.GetComponentInChildren<Belt_2>().gameObject, null);
        slot3 = new KeyValuePair<GameObject, GameObject>(hotbarMenu.GetComponentInChildren<Belt_3>().gameObject, null);
        slot4 = new KeyValuePair<GameObject, GameObject>(hotbarMenu.GetComponentInChildren<Belt_4>().gameObject, null);

        slotList.Add(leftHandEquipped);
        slotList.Add(rightHandEquipped);
        slotList.Add(leftHandSlot);
        slotList.Add(rightHandSlot);
        slotList.Add(backSlot);
        slotList.Add(slot1);
        slotList.Add(slot2);
        slotList.Add(slot3);
        slotList.Add(slot4);


        playerLeftSlot = CharacterBeltInstance.GetComponentInChildren<Left_Side_Slot>().gameObject;
        playerRightSlot = CharacterBeltInstance.GetComponentInChildren<Right_Side_Slot>().gameObject;
        //Temporary single back slot assignment to make sure it's all working
        playerBackSlot = new GameObject();
        playerSlot1 = CharacterBeltInstance.GetComponentInChildren<Slot1>().gameObject;
        playerSlot2 = CharacterBeltInstance.GetComponentInChildren<Slot2>().gameObject;
        playerSlot3 = CharacterBeltInstance.GetComponentInChildren<Slot3>().gameObject;
        playerSlot4 = CharacterBeltInstance.GetComponentInChildren<Slot4>().gameObject;

        playerCharacterSlots.Add(playerLeftHand);
        playerCharacterSlots.Add(playerRightHand);
        playerCharacterSlots.Add(playerLeftSlot);
        playerCharacterSlots.Add(playerRightSlot);
        playerCharacterSlots.Add(playerBackSlot);
        playerCharacterSlots.Add(playerSlot1);
        playerCharacterSlots.Add(playerSlot2);
        playerCharacterSlots.Add(playerSlot3);
        playerCharacterSlots.Add(playerSlot4);
    }

    public bool LeftHandFree()
    {
        return slotList[0].Value == null;
    }

    public bool RightHandFree()
    {
        return slotList[1].Value == null;
    }

    public void AddLeftHand(GameObject item)
    {
        slotList[0] = new KeyValuePair<GameObject, GameObject>(slotList[0].Key, item);
        //Add to hotbar
        ParentIcon(slotList[0].Key, item);
        //Add to player model
        ParentItem(playerLeftHand, item);
    }

    public void RemoveLeftHand()
    {
        slotList[0] = new KeyValuePair<GameObject, GameObject>(slotList[0].Key, null);
    }

    public void AddRightHand(GameObject item)
    {
        slotList[1] = new KeyValuePair<GameObject, GameObject>(slotList[1].Key, item);
        //Add to hotbar
        ParentIcon(slotList[1].Key, item);
        //Add to player model
        ParentItem(playerRightHand, item);
    }

    public void RemoveRightHand()
    {
        slotList[1] = new KeyValuePair<GameObject, GameObject>(slotList[1].Key, null);
    }

    public bool SlotFree(int index)
    {
        return slotList[index].Value == null;
    }

    public void AddToSlot(int index, GameObject item)
    {
        slotList[index] = new KeyValuePair<GameObject, GameObject>(slotList[index].Key, item);
        ParentIcon(slotList[index].Key, item);
        ParentItem(playerCharacterSlots[index], item);
    }

    public void RemoveFromSlot(int index)
    {
        if(slotList[index].Key.GetComponentsInChildren<Hot_Bar_Icon>() != null)
        {
            foreach (Hot_Bar_Icon image in slotList[index].Key.GetComponentsInChildren<Hot_Bar_Icon>())
            {
                Destroy(image.gameObject);
            }
        }
        slotList[index] = new KeyValuePair<GameObject, GameObject>(slotList[index].Key, null);
    }

    public void AddLeftSlot(GameObject item)
    {
        slotList[2] = new KeyValuePair<GameObject, GameObject>(slotList[2].Key, item);
        ParentIcon(slotList[2].Key, item);
        ParentItem(playerLeftSlot, item);
    }

    public void AddRightSlot(GameObject item)
    {
        slotList[3] = new KeyValuePair<GameObject, GameObject>(slotList[3].Key, item);
        ParentIcon(slotList[3].Key, item);
        ParentItem(playerRightSlot, item);
    }

    public void AddBack(GameObject item)
    {
        slotList[4] = new KeyValuePair<GameObject, GameObject>(slotList[4].Key, item);
        ParentIcon(slotList[4].Key, item);
        //ParentItem(playerBackSlot, item);
    }

    public void AddSlot1(GameObject item)
    {
        slotList[5] = new KeyValuePair<GameObject, GameObject>(slotList[5].Key, item);
        ParentIcon(slotList[5].Key, item);
        ParentItem(playerSlot1, item);
    }

    public void AddSlot2(GameObject item)
    {
        slotList[6] = new KeyValuePair<GameObject, GameObject>(slotList[6].Key, item);
        ParentIcon(slotList[6].Key, item);
        ParentItem(playerSlot2, item);
    }

    public void AddSlot3(GameObject item)
    {
        slotList[7] = new KeyValuePair<GameObject, GameObject>(slotList[7].Key, item);
        ParentIcon(slotList[7].Key, item);
        ParentItem(playerSlot3, item);
    }

    public void AddSlot4(GameObject item)
    {
        slotList[8] = new KeyValuePair<GameObject, GameObject>(slotList[8].Key, item);
        ParentIcon(slotList[8].Key, item);
        ParentItem(playerSlot4, item);
    }

    public GameObject GetSlotItem(int index)
    {
        return slotList[index].Value;
    }

    public GameObject GetActualItem(int index)
    {
        Debug.Log(playerCharacterSlots[index]);
        return playerCharacterSlots[index];
    }
    
    public void EquipLeftSlot()
    {
        SwapSlots(0, 2);
    }

    public void EquipRightSlot()
    {
        SwapSlots(1,3);
    }

    public void SwapSlots(int index1, int index2)
    {
        GameObject temp1 = GetSlotItem(index1);
        GameObject temp2 = GetSlotItem(index2);

        if(temp1 != null || temp2 != null)
        {
            RemoveFromSlot(index1);
            RemoveFromSlot(index2);
            AddToSlot(index1, temp2);
            AddToSlot(index2, temp1);
        }
    }

    public void ParentItem(GameObject parent, GameObject item)
    {
        if(item != null)
        {
            item.transform.parent = parent.gameObject.transform;
            item.transform.position = parent.transform.position;
            item.transform.rotation = parent.transform.rotation;
            item.GetComponent<SphereCollider>().enabled = false;
            item.GetComponent<SphereCollider>().isTrigger = false;
            item.GetComponent<MeshCollider>().enabled = false;
            item.GetComponent<MeshCollider>().isTrigger = false;
            item.GetComponent<Rigidbody>().useGravity = false;
            item.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            item.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;

            //Set rotation to sit properly in the hand/location
            Vector3 rot = item.GetComponent<Item>().GetOnPlayerRRotation();
            item.transform.localRotation = Quaternion.Euler(rot.x, rot.y, rot.z);
        }
    }

    public void ParentIcon(GameObject parent, GameObject item)
    {
        if(item != null)
        {
            if(item.GetComponent<Weapon_Item>() != null)
            {
                GameObject itemImage;
                itemImage = Instantiate(item.GetComponent<Weapon_Item>().inventoryImage).gameObject;
                itemImage.transform.SetParent(parent.gameObject.transform, false);
                itemImage.transform.position = parent.transform.position;
                itemImage.transform.rotation = parent.transform.rotation;
            }       
            if(item.GetComponent<Pouch_Item>() != null)
            {
                GameObject itemImage;
                itemImage = Instantiate(item.GetComponent<Pouch_Item>().inventoryImage).gameObject;
                itemImage.transform.SetParent(parent.gameObject.transform, false);
                itemImage.transform.position = parent.transform.position;
                itemImage.transform.rotation = parent.transform.rotation;
            }      
        }
    }
}
