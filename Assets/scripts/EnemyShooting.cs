using Photon.Pun;
using UnityEngine;

public class EnemyShooting : MonoBehaviourPun
{
    [SerializeField] private Transform pivot;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private EnemyManager enemyManager;

    void Start()
    {
        enemyManager = GetComponentInParent<EnemyManager>();
        if (enemyManager == null)
        {
            Debug.LogError("EnemyManager no encontrado.");
        }
    }

    public void Shoot()
    {
        if (enemyManager.playerDetected != null)
        {
            GameObject projectile = PhotonNetwork.Instantiate(projectilePrefab.name, pivot.position, pivot.rotation);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = (enemyManager.playerDetected.position - pivot.position).normalized * projectileSpeed;
            }
        }
    }
}
