using Photon.Pun;
using UnityEngine;

public class PlayerBulletDamage : MonoBehaviourPun
{
    public int playerBulletDamage;
    private PhotonView shooterView; // Referencia al jugador que disparó la bala

    private void Start()
    {
        // Destruir la bala después de 4 segundos de ser instanciada
        Invoke(nameof(DestroyBullet), 4f);
    }

    public void SetShooter(PhotonView shooter)
    {
        shooterView = shooter;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Minion"))
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
                        enemyHealth.ApplyDamage(playerBulletDamage, shooterView != null ? shooterView.ViewID : -1);
                    }
                }
                else
                {
                    // Modo online: enviar RPC para infligir daño al enemigo
                    enemyPhotonView.RPC("TakeDamage", RpcTarget.All, playerBulletDamage, shooterView != null ? shooterView.ViewID : -1);
                }
            }

            DestroyBullet();
        }
        if (other.CompareTag("Player"))
        {
            // Manejar el daño a los enemigos
            PhotonView playerPhotonView = other.GetComponent<PhotonView>();
            if (playerPhotonView != null)
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
                    // Modo online: enviar RPC para infligir daño al enemigo
                    playerPhotonView.RPC("TakeDamage", RpcTarget.All, playerBulletDamage, shooterView != null ? shooterView.ViewID : -1);
                }
            }

            DestroyBullet();
        }
        else
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
            // Transferir la propiedad al cliente actual antes de destruir
            if (!photonView.IsMine)
            {
                photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
            }
            PhotonNetwork.Destroy(gameObject); // Destruir el objeto en red
        }
    }
}
