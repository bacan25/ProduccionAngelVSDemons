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
        respawnPosition = transform.position; // Establece el punto de respawn inicial

        // Obtener el PlayerCanvas como singleton
        playerCanvas = PlayerCanvas.Instance;

        if (playerCanvas == null)
        {
            Debug.LogError("PlayerCanvas no encontrado. Asegúrate de que el PlayerCanvas esté en la escena.");
        }

        // Inicializar la barra de salud
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
            Debug.Log("El jugador ha muerto.");
            Respawn();
        }
    }

    public void TakeFallDamage()
    {
        // Aplica todo el daño (quita toda la vida)
        TakeDamage(currentHealth);
    }

    private void Respawn()
    {
        // Resetear la salud y mover al jugador a la posición de respawn
        currentHealth = maxHealth;
        transform.position = respawnPosition;

        Debug.Log("Respawn del jugador en: " + respawnPosition);
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
