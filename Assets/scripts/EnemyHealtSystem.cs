using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthSystem : MonoBehaviourPun
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    [SerializeField] private Slider healthSlider; // Slider de la UI que representa la barra de salud del enemigo

    public int monedas;

    void Start()
    {
        currentHealth = maxHealth;

        // Inicializar el valor del slider de salud
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth; // Establecer el valor máximo del slider
            healthSlider.value = currentHealth; // Establecer el valor inicial del slider
        }

        UpdateHealthUI(); // Inicializar la barra de salud con el valor actual
    }

    [PunRPC]
    public void TakeDamage(int damage, int shooterViewID)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthUI(); // Actualizar la barra de salud en la interfaz de usuario

        if (currentHealth <= 0)
        {
            Die(shooterViewID);
        }

        Debug.Log($"Enemigo: {gameObject.name} ha recibido {damage} de daño. Salud restante: {currentHealth}/{maxHealth}");
    }

    private void Die(int shooterViewID)
    {
        // Dar monedas al jugador que mató al enemigo
        PhotonView shooterView = PhotonView.Find(shooterViewID);
        if (shooterView != null && shooterView.IsMine)
        {
            PlayerGoldManager playerGoldManager = shooterView.GetComponent<PlayerGoldManager>();
            if (playerGoldManager != null)
            {
                playerGoldManager.AddGold(monedas);
            }
            else
            {
                Debug.LogError("PlayerGoldManager no encontrado en el jugador que mató al enemigo.");
            }
        }

        // Destruir el enemigo localmente si estamos en modo offline o si es controlado por este cliente
        if (PhotonNetwork.OfflineMode || photonView.IsMine)
        {
            if (PhotonNetwork.OfflineMode)
            {
                Destroy(gameObject);
            }
            else
            {
                PhotonNetwork.Destroy(gameObject);
            }

            Debug.Log($"Enemigo: {gameObject.name} ha muerto.");
        }
    }

    // Método para infligir daño desde scripts externos
    public void ApplyDamage(int damage, int shooterViewID)
    {
        if (PhotonNetwork.OfflineMode)
        {
            // En modo offline, aplicar el daño directamente
            TakeDamage(damage, shooterViewID);
        }
        else
        {
            // En modo online, enviar un RPC a todos los clientes
            photonView.RPC("TakeDamage", RpcTarget.AllBuffered, damage, shooterViewID);
        }
    }

    private void UpdateHealthUI()
    {
        // Actualizar la barra de salud si existe un slider asignado
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth; // Asignar la salud actual al slider
        }
        else
        {
            Debug.LogError("Barra de vida no asignada en EnemyHealthSystem.");
        }
    }
}
