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

        // Solo el propietario de la bala debe programar su destrucción
        if (photonView.IsMine)
        {
            Invoke(nameof(DestroyBullet), 4f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine) return; // Solo el propietario debe manejar la colisión y destrucción

        if (other.CompareTag("Player"))
        {
            PhotonView targetView = other.GetComponent<PhotonView>();

            if (targetView != null)
            {
                if (PhotonNetwork.InRoom)
                {
                    targetView.RPC("TakeDamage", RpcTarget.All, 10); // Ejemplo: 10 de daño
                }
                else if (PhotonNetwork.OfflineMode)
                {
                    var healthSystem = other.GetComponent<HealthSystem>();
                    if (healthSystem != null)
                    {
                        healthSystem.TakeDamage(20); // Ejemplo: 20 de daño
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
            if (!photonView.IsMine)
            {
                photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
            }
            PhotonNetwork.Destroy(gameObject); // Destruir el objeto en red
        }
    }
}
