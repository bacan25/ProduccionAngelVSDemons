using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    // Puntos de patrullaje (Patrol)
    [SerializeField] private Transform[] pathPoints;

    // Manager del campamento de enemigos
    private EnemyManager enemyManager;

    [SerializeField] private float visionRange = 10f;
    [SerializeField] private float visionAngle = 45f;

    [SerializeField] private float restPatrol = 2f;
    [SerializeField] private float shootingRange = 10f; // Rango de disparo

    [SerializeField] private NavMeshAgent agent;
    private int currentPathIndex;
    private bool isWaiting;
    private bool isAgro;

    private Transform targetPlayer;
    private EnemyShooting enemyShooting;

    private InGameManager gameManager;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyShooting = GetComponent<EnemyShooting>();
        currentPathIndex = 0;
        agent.destination = pathPoints[currentPathIndex].position;
        isAgro = false;
        isWaiting = false;

        // Obtener el InGameManager
        gameManager = FindObjectOfType<InGameManager>();
    }

    void Update()
    {
        // Si el EnemyManager ya detectó al jugador, perseguirlo
        if (enemyManager.playerDetected != null)
        {
            targetPlayer = enemyManager.playerDetected;
            isAgro = true;
        }

        if (isAgro)
        {
            ChasePlayer();
        }
        else
        {
            IsPlayerDetected();

            if (!isWaiting)
            {
                Patrol();
            }
        }
    }

    public void SetEnemyManager(EnemyManager manager)
    {
        enemyManager = manager;
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

    void ChasePlayer()
    {
        if (targetPlayer == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, targetPlayer.position);
        Vector3 directionToPlayer = (targetPlayer.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);

        // Gira suavemente
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

        if (distanceToPlayer <= shootingRange)
        {
            agent.isStopped = true;
            enemyShooting.Shoot(targetPlayer);
        }
        else
        {
            agent.isStopped = false;
            agent.SetDestination(targetPlayer.position);
        }
    }

    void IsPlayerDetected()
    {
        // Asegúrate de que el InGameManager tiene jugadores
        if (gameManager == null || gameManager.playerTransforms.Count == 0) return;

        foreach (Transform player in gameManager.playerTransforms)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer <= visionRange)
            {
                Vector3 directionToPlayer = (player.position - transform.position).normalized;
                float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

                // Verifica si el jugador está dentro del ángulo de visión
                if (angleToPlayer <= visionAngle / 2)
                {
                    // Notificar al EnemyManager que el jugador fue detectado
                    enemyManager.NotifyPlayerDetected(player);
                    isAgro = true;
                    targetPlayer = player;
                    break;
                }
            }
        }
    }

    public void OnPlayerDetected(Transform player)
    {
        // Cuando otro enemigo detecta al jugador, este también se activa
        targetPlayer = player;
        isAgro = true;
    }
}
