using Photon.Pun;
using UnityEngine;

public class PlayerBulletDamage : MonoBehaviourPun
{
    public int playerBulletDamage;
    private PhotonView shooterView; // Referencia al jugador que disparó la bala

    private void Start()
    {
        // Solo el propietario de la bala debe programar su destrucción
        if (photonView.IsMine)
        {
            Invoke(nameof(DestroyBullet), 4f);
        }
    }

    public void SetShooter(PhotonView shooter)
    {
        shooterView = shooter;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine) return; // Solo el propietario debe manejar la colisión y destrucción

        if (other.CompareTag("Minion"))
        {
            PhotonView enemyPhotonView = other.GetComponent<PhotonView>();
            if (enemyPhotonView != null)
            {
                if (PhotonNetwork.OfflineMode)
                {
                    EnemyHealthSystem enemyHealth = other.GetComponent<EnemyHealthSystem>();
                    if (enemyHealth != null)
                    {
                        enemyHealth.ApplyDamage(playerBulletDamage, shooterView != null ? shooterView.ViewID : -1);
                    }
                }
                else
                {
                    enemyPhotonView.RPC("TakeDamage", RpcTarget.All, playerBulletDamage, shooterView != null ? shooterView.ViewID : -1);
                }
            }

            DestroyBullet();
        }
        else if (other.CompareTag("Player"))
        {
            PhotonView playerPhotonView = other.GetComponent<PhotonView>();
            if (playerPhotonView != null)
            {
                if (PhotonNetwork.OfflineMode)
                {
                    HealthSystem playerHealth = other.GetComponent<HealthSystem>();
                    if (playerHealth != null)
                    {
                        playerHealth.TakeDamage(playerBulletDamage);
                    }
                }
                else
                {
                    playerPhotonView.RPC("TakeDamage", RpcTarget.All, playerBulletDamage);
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
