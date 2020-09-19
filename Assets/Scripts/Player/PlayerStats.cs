using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    // UI
    public Slider playerHealth;
    public Text killsTxt;

    // Stats
    public int health = 100;
    public int kills = 0;
    public Animator animator;
    public bool isDead = false;
    public AudioClip audioHurt;
    public AudioClip audioDead;
    public AudioSource audioSource;

    void Start()
    {
        playerHealth.value = health;
        killsTxt.text = "Kill  " + kills;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0 && !isDead) {
            health = 0;
            Die();
        }
        playerHealth.value = health;
        if (audioHurt != null) audioSource.PlayOneShot(audioHurt);
    }

    void Die()
    {
        isDead = true;
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<WeaponController>().enabled = false;
        animator.SetLayerWeight(1, 0f);
        animator.SetTrigger("Die");
        if (audioDead != null) audioSource.PlayOneShot(audioDead);
    }

    public void addKill()
    {
        kills++;
        killsTxt.text = "Kill  " + kills;
    }
}
