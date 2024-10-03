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
                targetPhotonView.RPC("TakeDamage", RpcTarget.All, playerBulletDamage);
            }
            PhotonNetwork.Destroy(gameObject);
        }
        else if (other.CompareTag("Ground"))
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
