using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private Stats_Manager stats;
    public float maxPlayerHealth = 100f;
    public float playerHealth = 100f;
    public float maxPlayerEnergy = 5000f;
    public float playerEnergy = 5000f;
    Coroutine rechargeEnergyCoroutine;
    public float energyRechargeFactor = 1f;
    public float energyRechargeRate = 0.05f;
    public float energyRechargeDelay = 5f;
    private bool hasStartedEnergyRecharge = false;
    Coroutine rechargeStaminaCoroutine;
    public float staminaRechargeRate = 0.2f;
    public UnityEngine.UI.Image energyBar;
    public UnityEngine.UI.Image emptyEnergyBar;
    private float maxEnergyWidth;
    public float maxPlayerStamina = 400f;
    public float playerStamina = 400f;
    public float staminaRechargeFactor = 1f;
    public float staminaRechargeDelay = 1f;
    private bool hasStartedStaminaRecharge = false;
    public UnityEngine.UI.Image staminaBar;
    public UnityEngine.UI.Image emptyStaminaBar;
    private float maxStaminaWidth;
    public GameObject loadedItem = null;
    public GameObject playerObject;

    //Camera
    public float maxCameraSmooth = 2f;
    public float currentCameraSmooth;
    public bool cameraIsLocked = false;

    //Other Managers
    public Inventory_List Inventory;
    public Hot_Bar_Manager HotbarManager;
    public Items_On_Back ItemsOnBack;
    public Items_On_Waist ItemsOnWaist;
    public Weapons_List Weapons;



    private void Awake()
    {
        Instance = this;
        Inventory = GetComponent<Inventory_List>();
        HotbarManager = GetComponent<Hot_Bar_Manager>();
        ItemsOnBack = GetComponent<Items_On_Back>();
        ItemsOnWaist = GetComponent<Items_On_Waist>();
        Weapons = GetComponent<Weapons_List>();
    }

    void Start()
    {
        stats = gameObject.AddComponent<Stats_Manager>();
        maxStaminaWidth = emptyStaminaBar.GetComponent<RectTransform>().rect.width;
        maxEnergyWidth = emptyEnergyBar.GetComponent<RectTransform>().rect.width;
        staminaBar.rectTransform.sizeDelta = new Vector2(CalculateFillAmount(playerStamina, maxPlayerStamina, maxStaminaWidth), 20);
        energyBar.rectTransform.sizeDelta = new Vector2(CalculateFillAmount(playerEnergy, maxPlayerEnergy, maxEnergyWidth), 20);
        currentCameraSmooth = maxCameraSmooth;
    }

    public GameObject GetPlayerObject()
    {
        return playerObject;
    }

    public float GetEnergy()
    {
        return playerEnergy;
    }

    public float GetStamina()
    {
        return playerStamina;
    }
    
    public void DamagePlayer(float damage)
    {
        if(playerHealth - damage > 0)
        {
            playerHealth -= damage;
        } else {
            playerHealth = 0;
            PlayerDied();
        }
    }   

    private void PlayerDied() 
    {
        //On Death
    }

    public void DepleteEnergy(float energyUsed) 
    {
        if(playerEnergy - energyUsed > 0)
        {
            playerEnergy -= energyUsed;
            energyBar.rectTransform.sizeDelta = new Vector2(CalculateFillAmount(playerEnergy, maxPlayerEnergy, maxEnergyWidth), 20);
        } else {
            playerEnergy = 0;
            energyBar.rectTransform.sizeDelta = new Vector2(0, 20);
        }
    }

    public void IncreaseEnergy(float addEnergy)
    {

    }

    public void DepleteStamina(float staminaUsed) 
    {
        if(rechargeStaminaCoroutine != null)
        {
            StopCoroutine(rechargeStaminaCoroutine);
            hasStartedStaminaRecharge = false;
        }
        if(rechargeEnergyCoroutine != null)
        {
            StopCoroutine(rechargeEnergyCoroutine);
            hasStartedEnergyRecharge = false;
        }
        if(playerStamina - staminaUsed > 0)
        {
            playerStamina -= staminaUsed;
            staminaBar.rectTransform.sizeDelta = new Vector2(playerStamina, 20);
        } else {
            playerStamina = 0;
            staminaBar.rectTransform.sizeDelta = new Vector2(0, 20);
        }
    }
    public void IncreaseStamina(float addStamina)
    {
        if(playerStamina + addStamina < maxPlayerStamina && playerEnergy >= addStamina)
        {
            playerStamina += addStamina;
            DepleteEnergy(addStamina);
            staminaBar.rectTransform.sizeDelta = new Vector2(CalculateFillAmount(playerStamina, maxPlayerStamina, maxStaminaWidth), 20);
        } else if(playerEnergy >= addStamina) {
            playerStamina = maxPlayerStamina;
            DepleteEnergy(maxPlayerStamina - playerStamina);
            staminaBar.rectTransform.sizeDelta = new Vector2(maxStaminaWidth, 20);
        }
    }

    public float GetPunchCost()
    {
        return stats.AdjustStaminaCost(20);
    }

    private void FixedUpdate()
    {
        if(playerStamina < maxPlayerStamina && !hasStartedStaminaRecharge)
        {
            rechargeStaminaCoroutine = StartCoroutine(RechargeStaminaDelay());
            hasStartedStaminaRecharge = true;
        }
        if(playerEnergy < maxPlayerEnergy && !hasStartedEnergyRecharge)
        {
            rechargeEnergyCoroutine = StartCoroutine(RechargeEnergyDelay());
            hasStartedEnergyRecharge = true;
        }
    }

    private IEnumerator RechargeStaminaDelay() 
    {
        yield return new WaitForSeconds(staminaRechargeDelay);
        while(playerStamina < maxPlayerStamina)
        {
            yield return new WaitForSeconds(staminaRechargeRate);
            IncreaseStamina(staminaRechargeFactor);
        }
        hasStartedStaminaRecharge = false;
        yield return null; // this will make Unity stop here and continue next frame
    }

    private IEnumerator RechargeEnergyDelay() 
    {
        yield return new WaitForSeconds(energyRechargeDelay);
        while(playerEnergy < maxPlayerEnergy)
        {
            yield return new WaitForSeconds(energyRechargeRate);
            IncreaseStamina(energyRechargeFactor);
        }
        hasStartedEnergyRecharge = false;
        yield return null; // this will make Unity stop here and continue next frame
    }

    private float CalculateFillAmount(float current, float maxAmount, float maxWidth) 
    {
        return (current * maxWidth / maxAmount);
    }

    public void AddLoadedItem(GameObject item)
    {
        loadedItem = item;
    }

    public void RemoveLoadedItem()
    {
        loadedItem = null;
    }

    public GameObject GetLoadedItem()
    {
        return loadedItem;
    }
}
