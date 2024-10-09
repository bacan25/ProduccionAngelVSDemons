using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyAI : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform[] pathPoints;
    [SerializeField] private float visionRange = 10f;
    [SerializeField] private float visionAngle = 45f;
    [SerializeField] private float restPatrol = 2f;
    [SerializeField] private float shootingRange = 10f;

    private NavMeshAgent agent;
    private int currentPathIndex;
    private bool isWaiting;
    public EnemyManager enemyManager;
    [SerializeField] private EnemyShooting enemyShooting;
    [SerializeField] private Animator anim;
    private bool isShooting;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentPathIndex = 0;
        agent.destination = pathPoints[currentPathIndex].position;
        isWaiting = false;

        if (enemyManager == null)
        {
            enemyManager = GetComponentInParent<EnemyManager>();
            if (enemyManager == null)
            {
                Debug.LogError("EnemyManager no asignado en EnemyAI.");
            }
        }
    }

    void Update()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        
        if (enemyManager.playerDetected == null)
            DetectPlayer();

        if (enemyManager.playerDetected != null)
        {
            if (!isShooting) StartCoroutine(ChasePlayer());
        }
        else if (!isWaiting)
        {
            Patrol();
        }
    }

    void DetectPlayer()
    {
        float minDistance = Mathf.Infinity;
        Transform closestPlayer = null;

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject playerObj in players)
        {
            Transform playerTransform = playerObj.transform;
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            if (distanceToPlayer <= visionRange)
            {
                Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
                float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

                if (angleToPlayer <= visionAngle / 2)
                {
                    if (distanceToPlayer < minDistance)
                    {
                        minDistance = distanceToPlayer;
                        closestPlayer = playerTransform;
                    }
                }
            }
        }

        enemyManager.playerDetected = closestPlayer;

        // Agrega este debug para confirmar que el jugador es detectado
        if (closestPlayer != null)
        {
            Debug.Log("Jugador detectado por el enemigo: " + closestPlayer.name);
        }
    }


    IEnumerator ChasePlayer()
    {
        isShooting = true;
        if (enemyManager.playerDetected != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, enemyManager.playerDetected.position);
            Vector3 directionToPlayer = (enemyManager.playerDetected.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);

            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

            if (distanceToPlayer <= shootingRange)
            {
                agent.isStopped = true;
                anim.SetTrigger("Attack");
                yield return new WaitForSeconds(1);
                agent.isStopped = false;
                

                // Verificar si se está llamando a Shoot
                Debug.Log("Enemigo disparando.");
                enemyShooting.Shoot();

                yield return new WaitForSeconds(3);
            }
            else
            {
                agent.isStopped = false;
                agent.SetDestination(enemyManager.playerDetected.position);
            }
        }

        isShooting = false;
    }

    void Patrol()
    {
        if (agent.remainingDistance < 0.5f && !isWaiting)
        {
            StartCoroutine(WaitAtPatrolPoint());
        }
    }

    IEnumerator WaitAtPatrolPoint()
    {
        isWaiting = true;
        yield return new WaitForSeconds(restPatrol);
        currentPathIndex = (currentPathIndex + 1) % pathPoints.Length;
        agent.destination = pathPoints[currentPathIndex].position;
        isWaiting = false;
    }
}
