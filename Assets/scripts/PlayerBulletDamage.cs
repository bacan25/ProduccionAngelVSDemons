using Photon.Pun;
using UnityEngine;

public class PlayerBulletDamage : MonoBehaviourPun
{
    public int playerBulletDamage;

    private void Start()
    {
        // Destruir la bala después de 4 segundos de ser instanciada
        Invoke(nameof(DestroyBullet), 4f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PhotonView targetPhotonView = other.GetComponent<PhotonView>();
            if (targetPhotonView != null)
            {
                if (PhotonNetwork.OfflineMode)
                {
                    // Modo offline: infligir daño localmente
                    HealthSystem playerHealth = other.GetComponent<HealthSystem>();
                    if (playerHealth != null)
                    {
                        playerHealth.TakeDamage(playerBulletDamage);
                    }
                }
                else
                {
                    // Modo online: enviar RPC para infligir daño
                    targetPhotonView.RPC("TakeDamage", RpcTarget.All, playerBulletDamage);
                }
            }

            DestroyBullet();
        }
      else if (other.CompareTag("Minion"))
        {
            // Manejar el daño a los enemigos
            PhotonView enemyPhotonView = other.GetComponent<PhotonView>();
            if (enemyPhotonView != null)
            {
                if (PhotonNetwork.OfflineMode)
                {
                    // Modo offline: infligir daño localmente
                    EnemyHealthSystem enemyHealth = other.GetComponent<EnemyHealthSystem>();
                    if (enemyHealth != null)
                    {
                        enemyHealth.ApplyDamage(playerBulletDamage, shooterView.ViewID);
                    }   
                }
                else
                {
                    // Modo online: enviar RPC para infligir daño al enemigo
                    enemyPhotonView.RPC("TakeDamage", RpcTarget.All, playerBulletDamage, shooterView.ViewID);
                }
            }

            DestroyBullet();
        else if (other.CompareTag("Ground"))
        {
            DestroyBullet();
        }
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
            // Transferir la propiedad al cliente actual antes de destruir
            if (!photonView.IsMine)
            {
                photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
            }
            PhotonNetwork.Destroy(gameObject); // Destruir el objeto en red
        }
    }
}
