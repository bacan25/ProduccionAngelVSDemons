using Photon.Pun;
using UnityEngine;

public class FallDamage : MonoBehaviourPun
{
    private void OnTriggerEnter(Collider other)
    {
        PhotonView targetPhotonView = other.GetComponent<PhotonView>();
        if (targetPhotonView != null && targetPhotonView.IsMine)
        {
            Health healthComponent = other.GetComponent<Health>(); // Obtenemos el componente Health
            if (healthComponent != null) // Verificamos si healthComponent está asignado
            {
                healthComponent.TakeFallDamage(); // Llamamos al método para aplicar el daño
            }
            else
            {
                Debug.LogError("El objeto no tiene el componente Health.");
            }
        }
    }
}
