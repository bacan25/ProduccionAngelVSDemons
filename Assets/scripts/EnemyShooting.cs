using Photon.Pun;
using UnityEngine;

public class EnemyShooting : MonoBehaviourPun
{
    [SerializeField] private Transform pivot;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private EnemyAI minion;

    void Start()
    {
        minion = GetComponentInParent<EnemyAI>();
        if (minion == null)
        {
            Debug.LogError("minion no encontrado.");
        }
    }

    public void Shoot()
    {
        if (minion.enemyManager.playerDetected != null)
        {
            GameObject projectile;

            // Usa Instantiate normal en modo offline
            if (PhotonNetwork.OfflineMode)
            {
                projectile = Instantiate(projectilePrefab, pivot.position, pivot.rotation);
            }
            else
            {
                // Usar PhotonNetwork.Instantiate en modo online
                projectile = PhotonNetwork.Instantiate(projectilePrefab.name, pivot.position, pivot.rotation);
            }

            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = (minion.enemyManager.playerDetected.position - pivot.position).normalized * projectileSpeed;
            }
        }
    }
}
