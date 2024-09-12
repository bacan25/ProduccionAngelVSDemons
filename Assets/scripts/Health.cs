using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviourPunCallbacks
{
    public int currentHealth;
    public int maxHealth;
    [SerializeField] Transform respawn;
    public Slider healthBar;

    void Start()
    {
        currentHealth = maxHealth;

        // Busca el componente Slider dentro del GameObject de la barra de vida
        if (healthBarObject != null)
        {
            healthBar = healthBarObject.GetComponent<Slider>();

            if (healthBar != null)
            {
                healthBar.maxValue = maxHealth;
                healthBar.value = maxHealth;
            }
            else
            {
                Debug.LogError("No se encontró el componente Slider en el GameObject asignado.");
            }
        }
        else
        {
            Debug.LogError("No se ha asignado ningún GameObject para la barra de vida.");
        }

        // Asigna también el healthBarTransform si no se ha asignado manualmente
        if (healthBarTransform == null && healthBarObject != null)
        {
            healthBarTransform = healthBarObject.transform;
        }
    }

    void Update()
    {
        // Asegúrate de que la barra de vida siga a la cámara del jugador
        if (healthBarTransform != null && Camera.main != null)
        {
            healthBarTransform.LookAt(Camera.main.transform);
        }
    }

    [PunRPC]
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

        // Actualiza la barra de vida después del daño
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Destruye el objeto cuando muere
        if (PhotonNetwork.IsConnectedAndReady)
        {
            if (photonView.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
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
