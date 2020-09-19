using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType {
    AR, SG, Pistol, Melee
}

public class WeaponInstance : MonoBehaviour
{
    public WeaponType type;
    public string weaponName;
    public int damage;
    public int range;
    public float fireRate;
    public int magazineCapacity;
    public ParticleSystem muzzleFlash;
    public AudioClip audioAttack;
    public AudioClip audioReload;

    GameObject playerGameObject;
    Animator playerAnim;
    AudioSource audioSource;
    [SerializeField]
    Collider _collider;
    
    [HideInInspector]
    public int bulletInMagazine;

    void Awake()
    {
        bulletInMagazine = magazineCapacity;
        _collider = GetComponent<Collider>();
    }
    
    void Start()
    {
        playerGameObject = GameObject.FindWithTag("Player");
        playerAnim = playerGameObject.GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Use()
    {
        if (_collider.enabled) {
            _collider.enabled = false;
        }
        gameObject.SetActive(true);
    }

    public void NonActivate()
    {
        gameObject.SetActive(false);
    }

    public void Attack()
    {
        if (muzzleFlash != null) {
            muzzleFlash.Emit(1);
        }

        if (type == WeaponType.Melee) {
            playerAnim.SetTrigger("MeleeAttack");
        } else if (bulletInMagazine > 0) {
            bulletInMagazine -= 1;
        }

        if (audioAttack != null) {
            audioSource.clip = audioAttack;
            audioSource.Play();
        }
    }

    public void Reload(int ammo)
    {
        bulletInMagazine = ammo;
        playerAnim.SetTrigger("Reload");
        if (audioReload != null) {
            audioSource.clip = audioReload;
            audioSource.Play();
        }
    }
}
