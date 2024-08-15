using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviourPun
{
    public float speed = 20f;
    public float lifeTime = 2f;  // Duración antes de que la bala se autodestruya

    void Start()
    {
        // Destruir la bala después de un tiempo para evitar que quede indefinidamente en la escena
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // Mover la bala hacia adelante cada frame
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine) return;  // Solo el propietario de la bala maneja las colisiones

        // Si la bala impacta con un jugador
        if (other.CompareTag("Player"))
        {
            PhotonView target = other.GetComponent<PhotonView>();
            if (target != null && target.IsMine)
            {
                target.RPC("TakeDamage", RpcTarget.All, 10f);  // Aplicar daño al jugador
            }
        }

        // Destruir la bala tras la colisión
        PhotonNetwork.Destroy(gameObject);
    }
}
