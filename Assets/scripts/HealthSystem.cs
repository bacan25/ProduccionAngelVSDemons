using Photon.Pun;
using UnityEngine;

public class HealthSystem : MonoBehaviourPun
{
    public int maxHealth = 100;
    private int currentHealth;
    private Vector3 respawnPosition;

    private PlayerCanvas playerCanvas; // Referencia al PlayerCanvas singleton

    private void Start()
    {
        // Inicializar la salud al máximo
        currentHealth = maxHealth;
        respawnPosition = transform.position;

        // Obtener el PlayerCanvas como singleton
        playerCanvas = PlayerCanvas.Instance;

        if (playerCanvas == null)
        {
            Debug.LogError("PlayerCanvas no encontrado. Asegúrate de que el PlayerCanvas esté en la escena.");
        }

        // Forzar la inicialización correcta de la barra de salud
        UpdateHealthUI();
    }

    [PunRPC]
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Aquí puedes agregar lógica para manejar la muerte del jugador
        if (currentHealth <= 0)
        {
            Debug.Log("El jugador ha muerto.");
            // Implementar respawn o lógica de muerte
        }

        UpdateHealthUI();
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

    public void SetRespawnPosition(Vector3 position)
    {
        respawnPosition = position;
    }

    private void UpdateHealthUI()
    {
        // Actualizar la barra de salud solo si este es el jugador local
        if (playerCanvas != null && (photonView.IsMine || PhotonNetwork.OfflineMode))
        {
            Debug.Log($"Actualizando barra de salud: {currentHealth} / {maxHealth}");
            playerCanvas.UpdateHealthBar((float)currentHealth / maxHealth);
        }
        else
        {
            Debug.LogError("No se pudo actualizar la barra de vida porque PlayerCanvas es nulo o no es el jugador local.");
        }
    }
}
