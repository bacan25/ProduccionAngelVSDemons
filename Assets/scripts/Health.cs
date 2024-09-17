using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviourPunCallbacks, IPunObservable
{
    public int currentHealth;
    public int maxHealth;
    private Vector3 respawnPosition; // Posición de respawn del jugador
    public Move_Player player; // Referencia al script de movimiento
    private bool isRespawning = false; // Bandera para evitar múltiples respawns simultáneos

    public Slider healthBar; // Si tienes una barra de vida en la UI (opcional)

    private void Start()
    {
        currentHealth = maxHealth;

        // Intentar asignar el componente Move_Player
        if (player == null)
        {
            player = GetComponent<Move_Player>(); // Buscar en el mismo GameObject
            if (player == null)
            {
                // Si no se encuentra en el mismo GameObject, buscar en los hijos
                player = GetComponentInChildren<Move_Player>();

                if (player == null)
                {
                    Debug.LogError("El componente Move_Player no se encontró en el jugador ni en sus hijos.");
                }
            }
        }

        // Buscar el Slider en el Canvas del jugador si no ha sido asignado manualmente
        if (healthBar == null)
        {
            Canvas playerCanvas = GetComponentInChildren<Canvas>(); // Buscar el Canvas dentro del prefab del jugador
            if (playerCanvas != null)
            {
                healthBar = playerCanvas.GetComponentInChildren<Slider>(); // Buscar el Slider dentro del Canvas
            }

            if (healthBar == null)
            {
                Debug.LogError("No se encontró una barra de vida (Slider) en el Canvas del jugador.");
            }
        }

        // Guardar la posición inicial como posición de respawn
        respawnPosition = transform.position;

        // Inicializamos la barra de vida (si existe)
        UpdateUI();
    }

    private void Update()
    {
        if (!photonView.IsMine) return; // Solo el jugador local ejecuta este código

        // Si el jugador está muerto y no está ya respawneando
        if (currentHealth <= 0 && !isRespawning)
        {
            isRespawning = true;
            RespawnPlayer();
            isRespawning = false;
        }
    }

    [PunRPC]
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // Asegurarnos de que la salud no sea negativa
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        // Actualizar la barra de vida o cualquier otra UI
        UpdateUI();

        if (currentHealth <= 0)
        {
            if (CompareTag("Minion")) // O "Enemy" según tu configuración
            {
                DeathEnemy();
            }
            else if (CompareTag("Player"))
            {
                // El jugador será respawneado en Update()
            }
        }
    }

    public void TakeFallDamage()
    {
        // Aplicar suficiente daño para "matar" al jugador por caída
        TakeDamage(currentHealth); // Esto provocará que la salud del jugador llegue a cero
    }

    public void Potion()
    {
        // Incrementar la salud
        currentHealth += 10; // Cambia este valor según lo que quieras que recupere la poción

        // Asegúrate de que la salud no exceda el máximo
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        // Actualizar la barra de vida o cualquier otra UI
        UpdateUI();
    }

    void RespawnPlayer()
    {
        // Desactivar temporalmente el movimiento y colisiones del jugador
        player.muerto = true;
        Collider playerCollider = GetComponent<Collider>();
        if (playerCollider != null)
        {
            playerCollider.enabled = false;
        }

        // Mover al jugador a la posición de respawn
        transform.position = respawnPosition;

        // Restablecer la salud
        currentHealth = maxHealth;

        // Actualizar la barra de vida
        UpdateUI();

        // Reactivar el movimiento y colisiones del jugador
        if (playerCollider != null)
        {
            playerCollider.enabled = true;
        }
        player.muerto = false;
    }

    public void DeathEnemy()
    {
        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    public void UpdateUI()
    {
        // Si tienes una barra de vida (Slider) asignada, actualízala
        if (healthBar != null)
        {
            healthBar.value = (float)currentHealth / maxHealth; // Actualiza el valor de la barra
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Enviamos el valor de currentHealth a los demás
            stream.SendNext(currentHealth);
        }
        else
        {
            // Recibimos el valor de currentHealth de otro jugador
            currentHealth = (int)stream.ReceiveNext();
        }
    }

    // Método para establecer la posición de respawn desde el InGameManager o cualquier otro sistema
    public void SetRespawnPosition(Vector3 position)
    {
        respawnPosition = position;
    }
}
