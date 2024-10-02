using Photon.Pun;
using UnityEngine;

public class BulletDamage : MonoBehaviourPun
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PhotonView targetView = other.GetComponent<PhotonView>();

            if (targetView != null && targetView.IsMine)
            {
                // Asegurarse de que el juego esté conectado a Photon o esté en modo offline
                if (PhotonNetwork.IsConnected || PhotonNetwork.OfflineMode)
                {
                    // Enviar el RPC solo si está conectado a Photon o en modo offline
                    targetView.RPC("TakeDamage", RpcTarget.All, 10);  // Ejemplo: 10 de daño
                }
                else
                {
                    Debug.LogError("No estás conectado a Photon ni en modo offline.");
                }
            }
        }
    }
}
