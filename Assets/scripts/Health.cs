using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth;

    //public Slider healthBar;

    void Start()
    {
        currentHealth = maxHealth;

    }

    /* void UpdateUI()
    {
        healthBar.value = currentHealth / maxHealth;
    } */

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        //UpdateUI();
    }
}
