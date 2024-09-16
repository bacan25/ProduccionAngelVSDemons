using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyAI : MonoBehaviourPunCallbacks
{
    // Puntos de ruta (Patrol)
    [SerializeField] private Transform[] pathPoints;
    [SerializeField] private float visionRange = 10f;
    [SerializeField] private float visionAngle = 45f;
    [SerializeField] private float restPatrol = 2f;
    [SerializeField] private float shootingRange = 10f; // Rango de disparo

    private NavMeshAgent agent;
    private int currentPathIndex;
    private bool isWaiting;
    [HideInInspector] public bool isAgro;

    public EnemyManager enemyManager;

    // Componente de disparo
    private EnemyShooting enemyShooting;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyShooting = GetComponent<EnemyShooting>();
        currentPathIndex = 0;
        agent.destination = pathPoints[currentPathIndex].position;
        isAgro = false;
        isWaiting = false;

        // Asegurarse de que enemyManager está asignado
        if (enemyManager == null)
        {
            enemyManager = GetComponentInParent<EnemyManager>();
            if (enemyManager == null)
            {
                Debug.LogError("EnemyManager no asignado en EnemyAI y no se encontró en los padres.");
            }
        }
    }

    void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        IsPlayerDetected();

        if (enemyManager.playerDetected != null)
        {
            isAgro = true;
        }
        if (isAgro)
        {
            ChasePlayer();
        }
        else
        {
            if (!isWaiting)
            {
                Patrol();
            }
        }
    }

    void IsPlayerDetected()
    {
        float minDistance = Mathf.Infinity;
        Transform closestPlayer = null;

        // Encontrar todos los jugadores en la escena
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject playerObj in players)
        {
            Transform playerTransform = playerObj.transform;
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            if (distanceToPlayer <= visionRange)
            {
                Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
                float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

                // Verifica si el jugador está dentro del ángulo de visión
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

        if (closestPlayer != null)
        {
            enemyManager.playerDetected = closestPlayer;
        }
        else
        {
            enemyManager.playerDetected = null;
            isAgro = false;
        }
    }

    void ChasePlayer()
    {
        if (enemyManager.playerDetected == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, enemyManager.playerDetected.position);
        Vector3 directionToPlayer = (enemyManager.playerDetected.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);

        // Gira suavemente
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

        if (distanceToPlayer <= shootingRange)
        {
            agent.isStopped = true;
            enemyShooting.Shoot(enemyManager.playerDetected.transform);
        }
        else
        {
            agent.isStopped = false;
            agent.SetDestination(enemyManager.playerDetected.position);
        }
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
