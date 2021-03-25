using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class ZombieController : MonoBehaviour
{
    SpawnManager sm;
    Collider mainCollider;
    Collider[] bodyParts;
    NavMeshAgent zombieAgent;
    GameObject chaseTarget;
    Animator zombieAnim;
    bool isDead = false;
    

    private void Awake()
    {
        mainCollider = GetComponent<Collider>();
        bodyParts = GetComponentsInChildren<Collider>(true);
        RagDollMode(false);
        zombieAgent = GetComponent<NavMeshAgent>();
        zombieAnim = GetComponent<Animator>();
        sm = GameObject.Find("SpawnController").GetComponentInChildren<SpawnManager>();
    }

    void Start()
    {
        chaseTarget = GameObject.FindGameObjectWithTag("player");
        transform.LookAt(chaseTarget.transform.position);
        zombieAnim.SetBool("chasing", true);
    }

    void Update()
    {
        if (!isDead)
        {
            zombieAgent.SetDestination(chaseTarget.transform.position);
            
            if (Vector3.Distance(transform.position, chaseTarget.transform.position) <= 1)
            {
                zombieAnim.SetBool("chasing", false);
                zombieAnim.SetBool("eat", true);
                sm.SendMessage("GameOver");
            }

            if (Vector3.Distance(transform.position, chaseTarget.transform.position) > 1)
            {
                zombieAnim.SetBool("chasing", true);
                zombieAnim.SetBool("eat", false);
            }
        }
    }
    //Подключаем режим куклы
    public void RagDollMode(bool arg)
    {

        foreach (var part in bodyParts)
        {
            
            GetComponentInChildren<Rigidbody>().useGravity = !arg;
            GetComponent<Animator>().enabled = !arg;
            
        }

    }
    //Вариант со снарядом
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("bullet"))
        {
            Debug.Log("Hit!");
            zombieAgent.ResetPath();
            RagDollMode(true);
            isDead = true;
            sm.ScoreCount();
            StartCoroutine("Decay");
        }
    }

    IEnumerator Decay()
    {
        yield return new WaitForSeconds(15);
        Destroy(gameObject);
    }

    //Вариант с raycast
    public void GotHitted()
    {
        Debug.Log("Hit!");
        zombieAgent.ResetPath();
        RagDollMode(true);
        isDead = true;
        sm.ScoreCount();
        StartCoroutine("Decay");
    }
}
