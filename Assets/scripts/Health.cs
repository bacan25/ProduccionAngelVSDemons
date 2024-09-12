using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviourPunCallbacks
{
  public int maxHealth = 100;
    public int currentHealth; 

    public GameObject healthBarObject;  // Asigna la barra de vida como un GameObject en el Inspector
    private Slider healthBar;  // Referencia al Slider dentro del GameObject
    public Transform healthBarTransform;

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

        if (currentHealth < 0)
        {
            currentHealth = 0;
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

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }
    }
}
