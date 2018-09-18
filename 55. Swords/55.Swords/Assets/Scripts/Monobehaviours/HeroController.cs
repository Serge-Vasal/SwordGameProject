using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Collections;
using System;

public class HeroController : MonoBehaviour
{    
    public AOE_SO aoeStompAttack;
    public int AoeAmount = 50;

    Animator animator;
    NavMeshAgent agent;
    CharacterStats stats;

    private GameObject attackTarget;
    private GameObject harvestTarget;
    [SerializeField] private Button stompButton;

    void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        stats = GetComponent<CharacterStats>();
    }

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);
        }
        aoeStompAttack.aoeInstance = Instantiate(aoeStompAttack.AoePrefab);
        aoeStompAttack.aoeScript = aoeStompAttack.aoeInstance.GetComponent<AoeAttackObject>();             
        aoeStompAttack.aoeInstance.SetActive(false);
        if (stompButton != null)
        {
            stompButton.onClick.AddListener(Stomp);
        }
        
    }

    void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        stompButton.gameObject.SetActive(currentState == GameManager.GameState.RUNNING);        
    }

    void Update()
    {
        animator.SetFloat("Speed", agent.velocity.magnitude);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Stomp();
        }
    }

    public void SetDestination(Vector3 destination)
    {
        StopAllCoroutines();
        agent.isStopped = false;
        agent.destination = destination;
    }

    public void AttackTarget(GameObject target)
    {
        Weapon weapon = stats.GetCurrentWeapon();
        if (weapon != null)
        {
            StopAllCoroutines();

            agent.isStopped = false;
            attackTarget = target;
            StartCoroutine(PursueAndAttackTarget());
        }
    }

    public void HarvestTarget(GameObject target)
    {
        Weapon weapon = stats.GetCurrentWeapon();
        if (weapon != null)
        {
            StopAllCoroutines();

            agent.isStopped = false;
            harvestTarget = target;
            StartCoroutine(PursueAndHarvestTarget());
        }
    }

    private IEnumerator PursueAndAttackTarget()
    {
        agent.isStopped = false;
        Weapon weapon = stats.GetCurrentWeapon();

        while (Vector3.Distance(transform.position, attackTarget.transform.position) > weapon.Range)
        {
            agent.destination = attackTarget.transform.position;
            yield return null;
        }

        agent.isStopped = true;

        transform.LookAt(attackTarget.transform);
        animator.SetTrigger("Attack");
    }

    private IEnumerator PursueAndHarvestTarget()
    {
        agent.isStopped = false;
        Weapon weapon = stats.GetCurrentWeapon();

        while (Vector3.Distance(transform.position, harvestTarget.transform.position) > weapon.Range)
        {
            agent.destination = harvestTarget.transform.position;
            yield return null;
        }

        agent.isStopped = true;

        transform.LookAt(harvestTarget.transform);
        animator.SetTrigger("Attack");
    }

    public void Hit()
    {        
        if (attackTarget != null)
            stats.GetCurrentWeapon().ExecuteAttack(gameObject, attackTarget);
    }

    public void Stomp()
    {
        if (AoeAmount > 0)
        {
            AoeAmount -= 1;
            animator.SetTrigger("Stomp");
            aoeStompAttack.CastAOE(gameObject, gameObject.transform.position, LayerMask.NameToLayer("PlayerSpells"));
        }        
    }
    
}
