using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerBulletDamage : MonoBehaviour
{
    public int playerBulletDamage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Minion"))
        {
            PhotonView targetPhotonView = other.GetComponent<PhotonView>();
            if (targetPhotonView != null && targetPhotonView.IsMine)
            {
                // Llama a TakeDamage en la red para que todos vean el daño
                targetPhotonView.RPC("TakeDamage", RpcTarget.All, playerBulletDamage);
            }
            Destroy(gameObject);
        }

        if (other.CompareTag("Ground"))
        {
            Destroy(this.gameObject);
        }

        Destroy(this.gameObject, 5f);
    }

  
}