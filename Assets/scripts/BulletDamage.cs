using Photon.Pun;
using UnityEngine;

public class BulletDamage : MonoBehaviourPun
{
    [SerializeField] private float bulletSpeed = 20f; // Velocidad de la bala
    private Rigidbody rb;

    private void Start()
    {
        // Obtener el componente Rigidbody y aplicar velocidad inicial
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = transform.forward * bulletSpeed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PhotonView targetView = other.GetComponent<PhotonView>();

            if (targetView != null)
            {
                // Si estás en una sala, envía el RPC; de lo contrario, ejecuta el daño localmente.
                if (PhotonNetwork.InRoom)
                {
                    // Enviar el RPC para que todos los jugadores lo reciban
                    targetView.RPC("TakeDamage", RpcTarget.All, 10);  // Ejemplo: 10 de daño
                }
                else if (PhotonNetwork.OfflineMode)
                {
                    // Ejecutar localmente si estamos en modo offline
                    var healthSystem = other.GetComponent<HealthSystem>();
                    if (healthSystem != null)
                    {
                        healthSystem.TakeDamage(10);  // Ejemplo: 10 de daño
                    }
                }
                else
                {
                    Debug.LogWarning("No puedes enviar RPCs fuera de una sala o en modo offline.");
                }
            }

            DestroyBullet();
        }
        else if (other.CompareTag("Ground"))
        {
            DestroyBullet();
        }
    }

    private void DestroyBullet()
    {
        if (PhotonNetwork.OfflineMode)
        {
            Destroy(gameObject); // Destruir el objeto localmente
        }
        else
        {
            // Transferir la propiedad antes de destruir si no somos los dueños
            if (!photonView.IsMine)
            {
                photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
            }
            PhotonNetwork.Destroy(gameObject); // Destruir el objeto en red
        }
    }
}
