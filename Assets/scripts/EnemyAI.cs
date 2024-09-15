using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    // Puntos de ruta (Patrol)
    [SerializeField]private Transform[] pathPoints;

    //public Transform player;
    [SerializeField]private float visionRange = 10f;
    [SerializeField]private float visionAngle = 45f;

    [SerializeField]private float restPatrol = 2f;  
 
    [SerializeField]private float shootingRange = 10f; // Rango de disparo

    [SerializeField]private NavMeshAgent agent;
    private int currentPathIndex;

    private bool isWaiting;
    
    [HideInInspector] public bool isAgro;

    public InGameManager _inGameManager;
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

    }

    void Update()
    {
        IsPlayerDetected();

        if(enemyManager.playerDetected != null)
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
                //CheckForPlayer();
            }
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

    void ChasePlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, enemyManager.playerDetected.position);
        Vector3 directionToPlayer = (enemyManager.playerDetected.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);

        // Gira suavementeeee
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

        if(distanceToPlayer <= shootingRange)
        {
            agent.isStopped = true;
            enemyShooting.Shoot(enemyManager.playerDetected.transform); //Solo parapruebas de la AI
            
        }
        else
        {
            agent.isStopped = false;
            agent.SetDestination(enemyManager.playerDetected.position);
        }
        
    }

    void IsPlayerDetected()
    {
        float minDistance = 100000;
        Transform closestPlayer = null;
        foreach(Transform player in _inGameManager.playerTransforms)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer <= visionRange)
            {
                Vector3 directionToPlayer = (player.position - transform.position).normalized;
                float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
                
                // Verifica si el player está dentro del angulo de visión
                if (angleToPlayer <= visionAngle / 2)
                {
                    minDistance = distanceToPlayer;
                    closestPlayer = player;
                }
            }
        }

         if (closestPlayer != null)
        {
            enemyManager.playerDetected = closestPlayer;
        }
    }

    // Dibuja los círculitos en la escena
    void OnDrawGizmos()
    {
        // Amarillo = Rango de la visión 
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRange);

        Vector3 leftBoundary = Quaternion.Euler(0, -visionAngle / 2, 0) * transform.forward * visionRange;
        Vector3 rightBoundary = Quaternion.Euler(0, visionAngle / 2, 0) * transform.forward * visionRange;

        //Azulito = Angulo de visión 
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary);

        if (enemyManager.playerDetected)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, enemyManager.playerDetected.position);
        }
    }
}
