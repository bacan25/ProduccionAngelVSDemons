using Photon.Pun;
using UnityEngine;

public class PlayerBulletDamage : MonoBehaviourPun
{
    public int playerBulletDamage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Minion"))
        {
            PhotonView targetPhotonView = other.GetComponent<PhotonView>();
            if (targetPhotonView != null)
            {
                targetPhotonView.RPC("TakeDamage", RpcTarget.All, playerBulletDamage);
            }
            if (PhotonNetwork.OfflineMode)
            {
                Destroy(gameObject);
            }
            else
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
        else if (other.CompareTag("Ground"))
        {
            if (PhotonNetwork.OfflineMode)
            {
                Destroy(gameObject);
            }
            else
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
}
