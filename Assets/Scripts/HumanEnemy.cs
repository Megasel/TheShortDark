using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HumanEnemy : MonoBehaviour, IDamagable
{
    public float idleRange = 10f;      
    public float chaseSpeed = 4f;   
    public float attackRange = 2f;   
    [SerializeField] private float health = 100;
    [SerializeField] private float damage = 10;
    private Animator animator;
    private NavMeshAgent agent;
    private Transform player;
    [SerializeField]private bool isAttacking = false;
    [SerializeField] private ParticleSystem bloodParticles;
    [SerializeField] private LootContainer lootContainer;
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;       

        agent.speed = chaseSpeed ;
        agent.stoppingDistance = attackRange;     
        agent.autoBraking = true;
    }

    void Update()
    {
        if (player == null || health <= 0)
        {
            //Debug.LogWarning("»грок не найден! ”бедитесь, что у игрока есть тег 'Player'.");
            return;
        }

        
            ChasePlayer();

            if (Vector3.Distance(transform.position, player.position) <= attackRange)
            {
                PlayAttackAnimation();
            }
        
        
    }




    void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    void PlayAttackAnimation()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            animator.SetTrigger("isAttack"); 

            Debug.Log("Ѕрод€га атакует игрока!");
            Invoke("ResetAttack", 1.5f); 
        }
    }
    void AttackPlayer()
    {
        if(Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            player.gameObject.GetComponent<IDamagable>().TakeDamage(0.2f);
        }
    }
    void ResetAttack()
    {
        isAttacking = false;
        animator.ResetTrigger("isAttack");    
    }



    public void TakeDamage(float damage)
    {

        bloodParticles.Play();
        health -= damage;
        if (health <= 0)
        {
            lootContainer.enabled = true;
            agent.isStopped = true;
            animator.SetTrigger("isDeath");
            Destroy(gameObject, 120);
        }

    }
}
