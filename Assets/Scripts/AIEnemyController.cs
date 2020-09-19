using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class AIEnemyController : MonoBehaviour
{
    public float speed = 1f;
    public int health = 100;
    public int attackDamage = 10;
    public float attackCooldown = 3f;
    public AudioClip audioAttack;
    public AudioClip audioDead;

    Animator animator;
    GameObject target;
    PlayerStats targetStats;
    NavMeshAgent agent;
    AudioSource audioSource;
    bool targetInRange = false;
    bool isCanAttack = true;
    bool isDead = false;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        target = GameObject.FindWithTag("Player");
        targetStats = target.GetComponent<PlayerStats>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        agent.updateRotation = true;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (target != null && !isDead) {
            agent.SetDestination(target.transform.position);

            if (agent.velocity != Vector3.zero) {
                animator.SetBool("isWalk", true);
            } else {
                animator.SetBool("isWalk", false);
            }

            if (targetInRange && isCanAttack && !targetStats.isDead) {
                StartCoroutine(CooldownAttack());
            }
        }
    }

    IEnumerator CooldownAttack()
    {
		isCanAttack = false;
        animator.SetTrigger("attack");
        if (audioAttack != null && !audioSource.isPlaying) {
            audioSource.clip = audioAttack;
            audioSource.Play();
        }
		yield return new WaitForSeconds(attackCooldown);
		isCanAttack = true;
	}

    public void AttackEvent()
	{
		if (targetInRange)
			targetStats.TakeDamage(attackDamage);
	}

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0 && !isDead) Death();
    }

    void Death()
    {
        isDead = true;
        targetStats.addKill();
        animator.SetTrigger("die");
        agent.enabled = false;
        if (audioDead != null) audioSource.PlayOneShot(audioDead);
        Destroy(this.gameObject, 5f);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player")) {
            targetInRange = true;
        }
    }
    
    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player")) {
            targetInRange = false;
        }
    }
}
