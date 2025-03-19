using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Bear : MonoBehaviour, IDamagable
{
    public float sleepRange = 10f; 
    public float chaseSpeed = 4f;
    public float attackRange = 2f; 
    [SerializeField] private float health = 100;
    private Animator animator;
    private NavMeshAgent agent;
    private Transform player;
    private bool isChasing = false;
    private bool isAttacking = false;
    [SerializeField] private ParticleSystem bloodParticles;
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform; 
        agent.speed = chaseSpeed ;
        animator.speed = health;
        agent.stoppingDistance = attackRange; 
        agent.autoBraking = true;
        Sleep();
    }
 
    void Update()
    {
        if (player == null || health <= 0)
            return;

        if (isChasing)
        {
            ChasePlayer();
            if (Vector3.Distance(transform.position, player.position) <= attackRange)
            {
                AttackPlayer();
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, player.position) <= sleepRange)
            {
                WakeUp();
            }
        }
    }
    void Sleep()
    {
        animator.SetBool("isSleeping", true);
        animator.SetBool("isChasing", false);
        agent.isStopped = true;
    }

    void WakeUp()
    {

        isChasing = true;
        animator.SetBool("isSleeping", false);
        animator.SetBool("isChasing", true);
        agent.isStopped = false;
    }

    void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    void AttackPlayer()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            int attackType = Random.Range(1, 4); 
            animator.SetInteger("AttackType", attackType);
            animator.SetTrigger("Attack"); 
            Invoke("ResetAttack", 1.5f);
        }
    }

    void ResetAttack()
    {
        isAttacking = false;
        animator.ResetTrigger("Attack"); 
    }



    public void TakeDamage(float damage)
    {
        if (!isChasing)
        {
            WakeUp();
        }
        bloodParticles.Play();
        health -= damage;
        if (health <= 0)
        {
            agent.isStopped = true;
            animator.Play("Death");  
            Destroy(gameObject, 120);
        }
    }
}