using Photon.Pun;
using UnityEngine;

public class BulletDamage : MonoBehaviourPun
{
    public int damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Minion"))
        {
            PhotonView targetPhotonView = other.GetComponent<PhotonView>();
            if (targetPhotonView != null)
            {
                targetPhotonView.RPC("TakeDamage", RpcTarget.All, damage);
            }
            PhotonNetwork.Destroy(this.gameObject);
        }
        else if (other.CompareTag("Ground"))
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
}
