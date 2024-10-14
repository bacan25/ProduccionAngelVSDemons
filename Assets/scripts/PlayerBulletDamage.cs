using Photon.Pun;
using UnityEngine;

public class PlayerBulletDamage : MonoBehaviourPun
{
    public int playerBulletDamage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PhotonView targetPhotonView = other.GetComponent<PhotonView>();
            if (targetPhotonView != null)
            {
                if (PhotonNetwork.OfflineMode || targetPhotonView.IsMine)
                {
                    // Modo offline o el jugador es controlado localmente: infligir daño localmente
                    HealthSystem playerHealth = other.GetComponent<HealthSystem>();
                    if (playerHealth != null)
                    {
                        playerHealth.TakeDamage(playerBulletDamage);
                    }
                }
                else
                {
                    // Modo online: enviar RPC para infligir daño al jugador
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
                if (PhotonNetwork.OfflineMode || enemyPhotonView.IsMine)
                {
                    // Modo offline o el enemigo es controlado localmente: infligir daño localmente
                    EnemyHealthSystem enemyHealth = other.GetComponent<EnemyHealthSystem>();
                    if (enemyHealth != null)
                    {
                        enemyHealth.ApplyDamage(playerBulletDamage);
                    }
                }
                else
                {
                    // Modo online: enviar RPC para infligir daño al enemigo
                    enemyPhotonView.RPC("TakeDamage", RpcTarget.All, playerBulletDamage);
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
            // Modo offline: destruir el objeto localmente
            Destroy(gameObject);
        }
        else if (PhotonNetwork.InRoom)
        {
            // Modo online y conectado a una sala: destruir el objeto a través de Photon
            PhotonNetwork.Destroy(gameObject);
        }
        else
        {
            Debug.LogWarning("No se puede destruir el proyectil porque no está conectado a Photon o en una sala.");
        }
    }
}
