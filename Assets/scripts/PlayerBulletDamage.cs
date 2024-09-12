using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerBulletDamage : MonoBehaviour
{
    public int playerBulletDamage;

    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto golpeado es un jugador o enemigo
        if (other.CompareTag("Player") || other.CompareTag("Minion"))
        {
            // Si estamos en modo multijugador
            if (PhotonNetwork.IsConnectedAndReady)
            {
                PhotonView targetPhotonView = other.GetComponent<PhotonView>();
                if (targetPhotonView != null && targetPhotonView.IsMine)
                {
                    // Aplicar daño a través de Photon (multijugador)
                    targetPhotonView.RPC("TakeDamage", RpcTarget.All, playerBulletDamage);
                }
            }
            else
            {
                // Modo local: aplicar daño directamente
                Health targetHealth = other.GetComponent<Health>();
                if (targetHealth != null)
                {
                    targetHealth.TakeDamage(playerBulletDamage);
                }
            }

            // Destruye la bala después de impactar
            Destroy(gameObject);
        }

        if (other.CompareTag("Ground"))
        {
            Destroy(this.gameObject);
        }

        Destroy(this.gameObject, 5f);
    }

  
}
