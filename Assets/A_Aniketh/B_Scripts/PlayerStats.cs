using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] int playerMaxHealth;                         //The maximum health the player can have
    [SerializeField] float playerMaxStamina;                      //The maximum stamina the player can have
    [SerializeField] float staminaDrainRate;                      //The amount of stamina drained
    [SerializeField] float staminaRecoveryRate;                   //The amount of stamina recovered
    int currentHealth;                                            //The current value of the health
    float currentStamina;                                         //The current value of the stamina

    [Header("UI Stuff For Stats")]
    [SerializeField] Slider healthBar;                            //The helath slider UI
    [SerializeField] Slider staminaBar;                           //The stamina slider UI

    void Start()
    {
        //If using Health Bar slider then setting the required values for the slider
        if(healthBar != null)
        {
            healthBar.maxValue = playerMaxHealth;
            healthBar.value = playerMaxHealth;
        }
        currentHealth = playerMaxHealth;                       //Setting helath to be full on start

        //If using Stamina Bar slider then setting the required values for the slider
        if (staminaBar != null)
        {
            staminaBar.maxValue = playerMaxStamina;
            staminaBar.value = playerMaxStamina;
        }
        currentStamina = playerMaxStamina;                     //Setting stamina to be full on start
    }

    //Function that reduces the player's health when taking damage from something
    public void ReduceHealth(int damageTaken)
    {
        currentHealth -= damageTaken;
        if (healthBar != null)
            healthBar.value = currentHealth;
    }

    //Function that manages the stamina recovary
    public void StaminaRecovary(bool sprint)
    {
        //If the player is not sprinting then his stimina will recover
        if (!sprint && currentStamina < playerMaxStamina)
        {
            currentStamina += staminaRecoveryRate;
            if (staminaBar != null)
                staminaBar.value = currentStamina;
        }
    }

    //Function that manages the reducion of stamina
    public void StaminaReducion(ref bool sprint)
    {
        //if the player is sprinting and has staimna then reduce it esle don't let him sprint
        if (sprint && currentStamina > 0)
        {
            currentStamina -= staminaDrainRate;
            if (staminaBar != null)
                staminaBar.value = currentStamina;
        }
        else if (sprint && currentStamina <= 0)
        {
            sprint = false;
        }
    }

}
