using System;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    public float patrolTime = 15; // time in seconds to wait before seeking a new patrol destination
    public float aggroRange = 10; // distance in scene units below which the NPC will increase speed and seek the player
    public Transform[] waypoints; // collection of waypoints which define a patrol area

    public AttackDefinition_SO attackDefinition;

    public Transform SpellHotSpot;

    int index; // the current waypoint index in the waypoints array
    float speed, agentSpeed; // current agent speed and NavMeshAgent component speed
    Transform player; // reference to the player object transform

    Animator animator; // reference to the animator component
    NavMeshAgent agent; // reference to the NavMeshAgent

    private float timeOfLastAttack;
    private Spell spell;

    private bool playerIsAlive;

    void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        if (agent != null) { agentSpeed = agent.speed; }
        player = GameObject.FindGameObjectWithTag("Player").transform;
        index = UnityEngine.Random.Range(0, waypoints.Length);

        InvokeRepeating("Tick", 0, 0.5f);

        if (waypoints.Length > 0)
        {
            InvokeRepeating("Patrol", UnityEngine.Random.Range(0, patrolTime), patrolTime);
        }

        timeOfLastAttack = float.MinValue;
        playerIsAlive = true;

        player.gameObject.GetComponent<DestructedEvent>().IDied += PlayerDied;     
    }

    private void Start()
    {
        if (attackDefinition is Weapon)
        {
            if (((Weapon)attackDefinition).weaponPreb != null)
            {
                ((Weapon)attackDefinition).weaponInstance = Instantiate(((Weapon)attackDefinition).weaponPreb);
            }
        }
        else if (attackDefinition is Spell)
        {
            spell=Instantiate((Spell)attackDefinition);
            if (spell.ProjectilePrefab != null)
            {
                spell.projectileInstance = Instantiate(spell.ProjectilePrefab);
                spell.projectileScript = spell.projectileInstance.GetComponent<Projectile>();
                spell.projectileInstance.SetActive(false);
                spell.projectileInstance.layer = LayerMask.NameToLayer("EnemySpells");
                spell.projectileScript.ProjectileCollided += spell.OnProjectileCollided;
            }
        }
    }

    private void PlayerDied()
    {
        playerIsAlive = false;
    }

    void Update()
    {        
        animator.SetFloat("Speed", agent.velocity.magnitude);

        float timeSinceLastAttack = Time.time - timeOfLastAttack;
        bool attackOnCooldown = timeSinceLastAttack < attackDefinition.CoolDown;

        agent.isStopped = attackOnCooldown;

        if (playerIsAlive)
        {
            float distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);
            bool attackInRange = distanceFromPlayer < attackDefinition.Range; 

            if (!attackOnCooldown && attackInRange)
            {
                transform.LookAt(player.transform);
                timeOfLastAttack = Time.time;
                animator.SetTrigger("Attack");
            }
        }        
    }

     public void Hit()
     {
        if (!playerIsAlive)
        {
            return;
        }
        if(attackDefinition is Weapon)
        {
            ((Weapon)attackDefinition).ExecuteAttack(gameObject, player.gameObject);
        }
        else if(attackDefinition is Spell)
        {
            spell.Cast(gameObject, SpellHotSpot.position, player.transform.position);
        }
     }

    void Patrol()
    {
        index = index == waypoints.Length - 1 ? 0 : index + 1;
    }

    void Tick()
    {
        if (gameObject.activeSelf && player)
        {
            agent.destination = waypoints[index].position;
            agent.speed = agentSpeed / 2;
        }
        

        if (gameObject.activeSelf&&player != null && Vector3.Distance(transform.position, player.position) < aggroRange)
        {
            agent.destination = player.position;
            agent.speed = agentSpeed;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, aggroRange);
    }
}
