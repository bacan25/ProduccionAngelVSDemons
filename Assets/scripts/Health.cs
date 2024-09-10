using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;
    

    public Slider healthBar;

    void Start()
    {
        currentHealth = maxHealth;

    }

    void UpdateUI()
    {
        healthBar.value = currentHealth;
    } 

    public void TakeDamage(int damage)
    {
        
        currentHealth -= damage;
        
        if (currentHealth < 1)
        {
            Death();
        }

        UpdateUI();
    }

    public void Potion()
    {
        currentHealth += 10;
        UpdateUI();
    }

    public void Death()
    {
        Destroy(gameObject);
    }     
}
