using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    // Puntos de ruta (Patrol)
    [SerializeField]private Transform[] pathPoints;

    public Transform player;
    [SerializeField]private float visionRange = 10f;
    [SerializeField]private float visionAngle = 45f;

    [SerializeField]private float restPatrol = 2f;  
 
    [SerializeField]private float shootingRange = 10f; // Rango de disparo

    [SerializeField]private NavMeshAgent agent;
    private int currentPathIndex;

    private bool isWaiting;
    [SerializeField]private bool playerDetected = false;
    [HideInInspector] public bool isAgro;
   

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

        if(playerDetected)
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
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);

        // Gira suavementeeee
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

        if(distanceToPlayer <= shootingRange)
        {
            agent.isStopped = true;
            enemyShooting.Shoot(player.transform);
            
        }
        else
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
        
    }

    void IsPlayerDetected()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= visionRange)
        {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
            
            // Verifica si el player está dentro del angulo de visión
            if (angleToPlayer <= visionAngle / 2)
            {
                playerDetected = true;
            }
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

        if (playerDetected)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, player.position);
        }
    }
}
