using Photon.Pun;
using UnityEngine;

public class BulletDamage : MonoBehaviourPun
{
    public int damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PhotonView targetPhotonView = other.GetComponent<PhotonView>();
            if (targetPhotonView != null)
            {
                // Aplicar daño al jugador a través del HealthSystem
                targetPhotonView.RPC("TakeDamage", RpcTarget.All, damage);
            }
            PhotonNetwork.Destroy(gameObject); // Destruir la bala tras impactar
        }
        else if (other.CompareTag("Ground"))
        {
            PhotonNetwork.Destroy(gameObject); // Destruir la bala si impacta el suelo
        }
    }
}
