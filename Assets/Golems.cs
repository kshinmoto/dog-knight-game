using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Golems : MonoBehaviour
{
    Animator animator;
    int isPatrollingHash;
    int isAttackingHash;
    int isHitHash;
    int isDeadHash;
    bool currentlyHit;

    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

    public float health;
    public GameObject leftFist;

    // Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    // Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    // states
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    // Start is called before the first frame update
    private void Awake()
    {
        player = GameObject.Find("DogPolyart").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        isPatrollingHash = Animator.StringToHash("isPatrolling");
        isAttackingHash = Animator.StringToHash("isAttacking");
        isHitHash = Animator.StringToHash("isHit");
        isDeadHash = Animator.StringToHash("isDead");
    }

    // Update is called once per frame
    private void Update()
    {
        bool isPatrolling = animator.GetBool(isPatrollingHash);
        bool isAttacking = animator.GetBool(isAttackingHash);
        bool isHit = animator.GetBool(isHitHash);
        bool isDead = animator.GetBool(isDeadHash);

        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patrolling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) AttackPlayer();
    }

    private void Patrolling()
    {
        leftFist.GetComponent<Collider>().enabled = false;
        animator.SetBool(isPatrollingHash, true);
        animator.SetBool(isAttackingHash, false);

        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;

    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }


    private void ChasePlayer()
    {
        leftFist.GetComponent<Collider>().enabled = false;
        animator.SetBool(isPatrollingHash, true);
        animator.SetBool(isAttackingHash, false);
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        animator.SetBool(isPatrollingHash, false);
        animator.SetBool(isAttackingHash, true);

        // enemy doesn't move when attacking
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        // attack code here
        //

        if (!alreadyAttacked)
        {
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void activateFist()
    {
        leftFist.GetComponent<Collider>().enabled = true;
    }

    public void deactivateFist()
    {
        leftFist.GetComponent<Collider>().enabled = false;
    }

    private void OnTriggerEnter(Collider trigger)
    {
        if (trigger.gameObject.tag == "DogSword")
        {
            Debug.Log("Sword Hit");
            health -= 50;
            /*
            animator.SetBool(isHitHash, true);
            if (health <= 0)
            {
                animator.SetBool(isHitHash, false);
                animator.SetBool(isDeadHash, true);
            }
            */
            if (health <= 0) Invoke(nameof(DestroyEnemy), .2f);
        }
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

}
