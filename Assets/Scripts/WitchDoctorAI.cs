using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WitchDoctorAI : MonoBehaviour
{
    public enum FSMStates
    {
        Idle,
        Chase,
        Attack,
        Dead
    }


    Vector3 origin;
    NavMeshAgent agent;
    public float enemySpeed = 5f;
    public FSMStates currentState;
    Animator anim;
    public float chaseDist = 40f;
    public float attackDist = 20f;
    public GameObject player;
    float distToPlayer;
    public GameObject spellproject;
    public GameObject spellOrigin;
    public float shootRate = 2.7f;
    float elapsedTime = 0f;
    public int health = 20;
    bool isDead = false;
    public Transform enemyEyes;
    public float FoV = 30f;
    public GameObject soul;

    void Start(){
        currentState = FSMStates.Idle;
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        origin = transform.position;
    }


    void Update(){
        distToPlayer = Vector3.Distance(transform.position, player.transform.position);
        switch(currentState){
            case FSMStates.Idle:
                UpdateIdleState();
                break;
            case FSMStates.Chase:
                UpdateChaseState();
                break;
            case FSMStates.Attack:
                UpdateAttackState();
                break;
            case FSMStates.Dead:
                UpdateDeadState();
                break;
        }
        elapsedTime += Time.deltaTime;
        if ( health <= 0){
            currentState = FSMStates.Dead;
        }
        
    }

    void UpdateIdleState(){
        anim.SetInteger("State", 0);
        agent.stoppingDistance = 0;
        agent.speed = 1.5f;
        if (distToPlayer <= chaseDist && IsPlayerInClearFOV()){
            currentState = FSMStates.Chase;
        }
        FaceTraget(transform.position);
        agent.SetDestination(origin);
    }

    void UpdateChaseState(){
        anim.SetInteger("State", 1);
        agent.stoppingDistance = attackDist;
        agent.speed = 3.5f;
        if (distToPlayer <= attackDist){
            currentState = FSMStates.Attack;
        } else if ( distToPlayer > chaseDist){
            currentState = FSMStates.Idle;
        }
        FaceTraget(player.transform.position);
        agent.SetDestination(player.transform.position);
    }

    void UpdateAttackState(){
        anim.SetInteger("State", 2);
        if ( distToPlayer <= attackDist){
            currentState = FSMStates.Attack;
        } else if ( distToPlayer > attackDist && distToPlayer <= chaseDist){
            currentState = FSMStates.Chase;
        } else if ( distToPlayer > chaseDist){
            currentState = FSMStates.Idle;
        }
        FaceTraget(player.transform.position);
        EnemySpellCast();
    }

    void UpdateDeadState(){
        anim.SetInteger("State", 0);
        isDead = true;
        FindObjectOfType<LevelManager>().EnemyKilled();
        Destroy(gameObject);
        Instantiate(soul, transform.position + new Vector3(0f, 1f, 0f), transform.rotation);
    }

    void FaceTraget(Vector3 target){
        Vector3 dir = (target- transform.position).normalized;
        dir.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10 * Time.deltaTime);
    }

    private bool IsPlayerInClearFOV(){
        RaycastHit hit;
        Vector3 dirToPlayer = player.transform.position - enemyEyes.position;
        if (Vector3.Angle(dirToPlayer, enemyEyes.forward) <= FoV){
            if (Physics.Raycast(enemyEyes.position, dirToPlayer, out hit, chaseDist)){
                if (hit.collider.CompareTag("Player")){
                    return true;
                }
            }
        }
        return false;
    }



    void SpellCast(){
        Instantiate(spellproject, spellOrigin.transform.position + transform.forward, transform.rotation * Quaternion.Euler(5, 0, 0));
    }

    void EnemySpellCast(){
        if (elapsedTime >= shootRate && !isDead){
            Invoke("SpellCast", 1f); //animation duration when complex animations added
            elapsedTime = 0f;
        }
        
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Lava")
        {
            currentState = FSMStates.Dead;
        }
        if (collision.gameObject.tag == "Fireball")
        {
            health -= 10;
        }
    }
}
