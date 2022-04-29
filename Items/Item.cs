using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private GameManager gameManager;
    public enum Item_Type {
        Weapon,
        Wearable,
        Magic,
        Spell,
        Crafting,
        Health,
        Pouch,
    }

    public enum Item_Size {
        Small,
        Medium,
        Large,
        ExtraLarge
    }

    protected string itemName;
    protected Item_Type itemType;
    public Item_Size itemSize;
    public UnityEngine.UI.Image inventoryImage;
    protected bool canEquip;
    protected int equippedLocation = 0;
    protected bool isEquipped = false;
    protected bool canStack;
    protected bool isPlaceable;
    protected float weight;
    public Vector3 onPlayerRRotation;
    public Vector3 onPlayerLRotation;
    public Vector3 onBeltRotation;
    private Animator playerAnimator;
    public List<AnimationClip> itemAnimationList = new List<AnimationClip>();
    private AnimationClip currentAnimation;
    
    
    public Item(Item_Type type, bool equip, bool stack, bool placeable)
    {
        canEquip = equip;
        canStack = stack;
        isPlaceable = placeable;
    }

    private void Start()
    {
        gameManager = GameManager.Instance;  
        playerAnimator = gameManager.GetPlayerObject().GetComponent<Animator>();  
        if(itemAnimationList.Count > 0)
        {
            currentAnimation = itemAnimationList[0];
        }       
    }

    public AnimationClip GetAnimationClip()
    {
        return currentAnimation;
    }

    public AnimationClip StartAnimationCycle()
    {
        return currentAnimation;
    }

    public float GetStaminaCost()
    {
        return 3f;
    }

    public float GetEnergyCost()
    {
        return 3f;
    }
    
    public Item.Item_Size GetItemSize()
    {
        return itemSize;
    }
    public string GetName()
    {
        return itemName;
    }

    public Item.Item_Type GetItemType()
    {
        return itemType;
    }
    
    public UnityEngine.UI.Image GetImage()
    {
        return inventoryImage;
    }

    public void SetIsEquipped(bool isEquip)
    {
        isEquipped = isEquip;
    }

    public void SetEquippedLocation(int index)
    {
        equippedLocation = index;
    }

    public bool GetIsEquipped()
    {
        return isEquipped;
    }

    public int GetEquippedLocation()
    {
        return equippedLocation;
    }

    public bool CanEquip()
    {
        return canEquip;
    }

    public bool CanStack()
    {
        return canStack;
    }

    public bool CanPlace()
    {
        return isPlaceable;
    }

    public void SetWeight(float wght)
    {
        weight = wght;
    }

    public float GetWeight()
    {
        return weight;
    }

    public void ActivateAnimation()
    {

    }

    public Vector3 GetOnPlayerRRotation()
    {
        return onPlayerRRotation;
    }
    
    public Vector3 GetOnPlayerLRotation()
    {
        return onPlayerLRotation;
    }

    public Vector3 GetOnBeltRotation()
    {
        return onBeltRotation;
    }
}

