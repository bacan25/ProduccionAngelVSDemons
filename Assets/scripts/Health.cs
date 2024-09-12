using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviourPunCallbacks
{
    public int currentHealth;
    public int maxHealth;

    public Slider healthBar;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }
    }

    [PunRPC]
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

    [PunRPC]
    public void Death()
    {
        PhotonNetwork.Destroy(gameObject); // Destruir el objeto en la red
    }
}
