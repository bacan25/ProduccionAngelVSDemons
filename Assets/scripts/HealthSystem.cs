using Photon.Pun;
using UnityEngine;

public class HealthSystem : MonoBehaviourPun
{
    public int maxHealth = 100;
    private int currentHealth;
    private Vector3 respawnPosition;

    public PlayerCanvas playerCanvas; // Referencia al canvas del jugador

    private void Start()
    {
        currentHealth = maxHealth;
        respawnPosition = transform.position;

        if (playerCanvas == null)
        {
            playerCanvas = GetComponent<PlayerCanvas>();
        }

        UpdateHealthUI();
    }

    [PunRPC]
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Respawn();
        }
    }

    public void TakeFallDamage(int damage)
    {
        TakeDamage(damage);
    }

    private void Respawn()
    {
        transform.position = respawnPosition;
        currentHealth = maxHealth;

        UpdateHealthUI();
    }

    // Función para establecer la posición de respawn
    public void SetRespawnPosition(Vector3 position)
    {
        respawnPosition = position;
    }

    private void UpdateHealthUI()
    {
        if (playerCanvas != null)
        {
            playerCanvas.UpdateHealthBar((float)currentHealth / maxHealth);
        }
    }
}
