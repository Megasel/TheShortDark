
using UnityEngine;
using UnityEngine.AI;

public class Bunny : MonoBehaviour, IDamagable
{
    public float idleMoveRange = 5f;
    public float runSpeed = 5f;
    public float detectionRange = 10f;
    public LayerMask groundLayer; 
    [SerializeField] private Collider physicsCollider;
    
    private Animator animator;
    private NavMeshAgent agent;
    private Transform player;
    private Vector3 targetPosition;
    private bool isRunningAway = false;
    private bool playerDetected = false;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform; 

        agent.speed = runSpeed;
        agent.autoBraking = false;

        SetNewIdleTarget();
    }

    void Update()
    {
        if (player == null)
            return;

        if (playerDetected)
        {
            RunAwayFromPlayer();
        }
        else
        {
            IdleMovement();
        }
        UpdateAnimation();
    }

    void IdleMovement()
    {
        if (isRunningAway)
        {
            isRunningAway = false;
        }
        if (!agent.pathPending && agent.remainingDistance < 0.1f)
        {
            SetNewIdleTarget();
        }
    }

    void RunAwayFromPlayer()
    {
        if (!isRunningAway)
        {
            isRunningAway = true;
        }

        Vector3 runDirection = (transform.position - player.position).normalized;
        targetPosition = transform.position + runDirection * detectionRange;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPosition, out hit, detectionRange, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
            Debug.DrawLine(transform.position, hit.position, Color.red); 
        }
        
    }

    void SetNewIdleTarget()
    {
        Vector3 randomDirection = Random.insideUnitSphere * idleMoveRange;
        randomDirection += transform.position;
        randomDirection.y = 0;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, idleMoveRange, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
            Debug.Log("Новая цель для состояния покоя установлена: " + hit.position);
        }
        else
        {
            Debug.LogWarning("Не удалось найти валидную позицию на NavMesh для состояния покоя.");
        }
    }

    void UpdateAnimation()
    {
        if (agent.velocity.magnitude > 0.1f)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerDetected = true;
            Debug.Log("Игрок вошел в зону обнаружения.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerDetected = false;
            Debug.Log("Игрок вышел из зоны обнаружения.");
        }
    }

    public void TakeDamage(float damage)
    {
        physicsCollider.enabled = true;
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        animator.Play("CrowDead");
        rb.angularDrag =100;
        rb.freezeRotation = true;
        print("КРОЛИК");
        Destroy(gameObject, 120);
    }
}