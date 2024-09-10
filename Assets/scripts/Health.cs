using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviourPunCallbacks
{
    public int currentHealth;
    public int maxHealth;

    public Slider healthBar;
    public Transform healthBarTransform;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateUI(); // Asegúrate de que la UI de vida esté correcta al inicio
    }

    void Update()
    {
        if (healthBarTransform != null && Camera.main != null)
        {
            // Asegúrate de que la barra de vida siempre esté orientada hacia la cámara
            healthBarTransform.LookAt(Camera.main.transform);
        }
    }

    void UpdateUI()
    {
        if (healthBar != null)
        {
            healthBar.value = (float)currentHealth / maxHealth; // Actualiza la barra de vida proporcionalmente
        }
    }

    [PunRPC]
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth < 1)
        {
            currentHealth = 0;
            Death();
        }

        UpdateUI(); // Asegúrate de que la UI se actualiza cada vez que se recibe daño
    }

    public void Potion()
    {
        currentHealth += 10;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth; // No puede superar la salud máxima
        }
        UpdateUI();
    }

    [PunRPC]
    public void Death()
    {
        if (photonView != null && photonView.IsMine)
        {
            PhotonNetwork.Destroy(gameObject); // Destruir el objeto en la red
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
