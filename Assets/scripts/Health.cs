using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;
    [SerializeField] Transform respawn;
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
        
        if (currentHealth < 1 && this.gameObject.CompareTag("Minion")) //Cambiar tag a Enemy
        {
            DeathEnemy();
        }else if (currentHealth < 1 && this.gameObject.CompareTag("Player"))
        {
            DeathPlayer();
        }

        UpdateUI();
    }

    public void Potion()
    {
        currentHealth += 10;
        UpdateUI();
    }

    public void DeathEnemy()
    {
        Destroy(gameObject);
    }

    public void DeathPlayer()
    {
        this.gameObject.transform.position = respawn.position;
        currentHealth = maxHealth;

    }
}
