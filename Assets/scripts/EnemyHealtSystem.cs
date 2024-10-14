using Photon.Pun;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class EnemyHealthSystem : MonoBehaviourPun
{
    
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    [SerializeField] private Slider healthSlider; // Slider de la UI que representa la barra de salud del enemigo

    private PlayerCanvas playerCanvas;

    public int monedas;

    void Start()
    {
        playerCanvas = PlayerCanvas.Instance;

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
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthUI(); // Actualizar la barra de salud en la interfaz de usuario

        if (currentHealth <= 0)
        {
            UpdateMonedasUI();
            Die();
           
        }

        Debug.Log($"Enemigo: {gameObject.name} ha recibido {damage} de daño. Salud restante: {currentHealth}/{maxHealth}");
    }

    private void Die()
    {
        if (PhotonNetwork.OfflineMode || photonView.IsMine)
        {
            // Destruir el enemigo localmente si estamos en modo offline o si es controlado por este cliente
            if (PhotonNetwork.OfflineMode)
            {
                Destroy(gameObject);
            }
            else
            {
                // Destruir el objeto en red usando Photon si estamos online
                PhotonNetwork.Destroy(gameObject);
            }

            Debug.Log($"Enemigo: {gameObject.name} ha muerto.");
        }
    }

    // Método para infligir daño desde scripts externos
    public void ApplyDamage(int damage)
    {
        if (PhotonNetwork.OfflineMode)
        {
            // En modo offline, aplicar el daño directamente
            TakeDamage(damage);
        }
        else
        {
            // En modo online, enviar un RPC a todos los clientes
            photonView.RPC("TakeDamage", RpcTarget.AllBuffered, damage);
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

    private void UpdateMonedasUI()
    {
        // Actualizar la barra de salud solo si este es el jugador local
        if (playerCanvas != null && (photonView.IsMine || PhotonNetwork.OfflineMode))
        {
            playerCanvas.SumarMonedas(monedas);
        }
        else
        {
            Debug.LogError("No se pudo actualizar las monedas porque el slider no está asignado.");
        }

    }
}
