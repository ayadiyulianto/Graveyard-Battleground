using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponController : MonoBehaviour
{
    public Animator animator;
    public WeaponInstance mainWeapon, secondWeapon, pistol, meleeWeapon;
    public AudioClip audioPickUp;
    public AudioClip audioChangeWeapon;
    public AudioSource audioSource;

    BackPack backpack;
    WeaponInstance currentWeapon;
    bool canAttack = true;
    bool haveMainWpn, haveSecondWpn, havePistol, haveMeleeWpn;

    // UI
    public Image panelMainWpn, panelSecondWpn, panelPistol, panelMeleeWpn;
    public Image imageAR, imageSG, imagePistol, imageMelee;
    public Text textAmmoMainWpn, textAmmoSecondWpn, textAmmoPistol;
    Color32 selectedColorPanel = new Color32(255, 255, 0, 130);
    Color32 defaultColorPanel = new Color32(0, 0, 0, 130);

    void Start()
    {
        // disable all weapon
        haveMainWpn = haveSecondWpn = havePistol = haveMeleeWpn = false;
        if (mainWeapon != null)
            mainWeapon.NonActivate();
        if (secondWeapon != null)
            secondWeapon.NonActivate();
        if (pistol != null)
            pistol.NonActivate();
        if (meleeWeapon != null)
            meleeWeapon.NonActivate();

        backpack = GetComponent<BackPack>();

        UIStart();
    }

    void UIStart()
    {
        panelMainWpn.color = defaultColorPanel;
        panelSecondWpn.color = defaultColorPanel;
        panelPistol.color = defaultColorPanel;
        panelMeleeWpn.color = defaultColorPanel;
        imageAR.gameObject.SetActive(false);
        imageSG.gameObject.SetActive(false);
        imagePistol.gameObject.SetActive(false);
        imageMelee.gameObject.SetActive(false);
        textAmmoMainWpn.text = "";
        textAmmoSecondWpn.text = "";
        textAmmoPistol.text = "";
    }

    void Update()
    {
        checkAmmo();
        InputHandler();
    }

    void checkAmmo()
    {
        if (currentWeapon != null && currentWeapon.bulletInMagazine == 0 
            && currentWeapon.type != WeaponType.Melee)
        {
            canAttack = false;
        }
    }

    void InputHandler()
    {
        if (Input.GetButton("Fire1") && canAttack && currentWeapon != null)
            StartCoroutine(Attack());
            
        if (Input.GetButtonDown("Reload") && currentWeapon != null)
            StartCoroutine(Reload());

        if (Input.GetKeyDown(KeyCode.Alpha1) && mainWeapon != null && haveMainWpn) {
            StartCoroutine(ChangeWeapon(mainWeapon));
        } else if (Input.GetKeyDown(KeyCode.Alpha2) && secondWeapon != null && haveSecondWpn) {
            StartCoroutine(ChangeWeapon(secondWeapon));
        } else if (Input.GetKeyDown(KeyCode.Alpha3) && pistol != null && havePistol) {
            StartCoroutine(ChangeWeapon(pistol));
        } else if (Input.GetKeyDown(KeyCode.Alpha4) && meleeWeapon != null && haveMeleeWpn) {
            StartCoroutine(ChangeWeapon(meleeWeapon));
        }
    }

    IEnumerator Attack()
    {
        canAttack = false;
        currentWeapon.Attack();

        Camera cam = GetComponentInChildren<Camera>();
        Vector3 rayOrigin = cam.ViewportToWorldPoint(new Vector3(.5f, .5f, .0f));
        RaycastHit hit;
        if (Physics.Raycast(rayOrigin, cam.transform.forward, out hit, currentWeapon.range)) {
            GameObject go = hit.transform.gameObject;
            if (go.CompareTag("Enemy")) {
                go.GetComponent<AIEnemyController>().TakeDamage(currentWeapon.damage);
            }
        }

        UIAmmoWeapon(currentWeapon);
        yield return new WaitForSeconds(currentWeapon.fireRate);
        canAttack = true;
    }

    IEnumerator Reload()
    {
        canAttack = false;
        int availableAmmo = CheckReload();
        if (currentWeapon.bulletInMagazine < currentWeapon.magazineCapacity && 
            availableAmmo > currentWeapon.bulletInMagazine)
        {
            currentWeapon.Reload(availableAmmo);
            yield return new WaitForSeconds(2f);
        }
        canAttack = true;
        UIAmmoWeapon(currentWeapon);
    }

    int CheckReload()
    {
        int availableAmmo = currentWeapon.bulletInMagazine;
        int requiredReload = currentWeapon.magazineCapacity - currentWeapon.bulletInMagazine;
        if (currentWeapon.type == WeaponType.AR) {
            if (backpack.ammoAR > requiredReload) {
                availableAmmo += requiredReload;
                backpack.ammoAR -= requiredReload;
            } else {
                availableAmmo += backpack.ammoAR;
                backpack.ammoAR = 0;
            }
        } else if (currentWeapon.type == WeaponType.SG) {
            if (backpack.ammoSG > requiredReload) {
                availableAmmo += requiredReload;
                backpack.ammoSG -= requiredReload;
            } else {
                availableAmmo += backpack.ammoSG;
                backpack.ammoSG = 0;
            }
        } else if (currentWeapon.type == WeaponType.Pistol) {
            if (backpack.ammoPistol > requiredReload) {
                availableAmmo += requiredReload;
                backpack.ammoPistol -= requiredReload;
            } else {
                availableAmmo += backpack.ammoPistol;
                backpack.ammoPistol = 0;
            }
        }
        return availableAmmo;
    }

    void UIAmmoWeapon(WeaponInstance wpn)
    {
        if (wpn == mainWeapon) {
            int ammoBackPack = 0;
            if (wpn.type == WeaponType.AR) {
                ammoBackPack = backpack.ammoAR;
            } else if (wpn.type == WeaponType.SG) {
                ammoBackPack = backpack.ammoSG;
            }
            textAmmoMainWpn.text = wpn.bulletInMagazine + " / " + ammoBackPack;
        } else if (wpn == secondWeapon) {
            int ammoBackPack = 0;
            if (wpn.type == WeaponType.AR) {
                ammoBackPack = backpack.ammoAR;
            } else if (wpn.type == WeaponType.SG) {
                ammoBackPack = backpack.ammoSG;
            }
            textAmmoSecondWpn.text = wpn.bulletInMagazine + " / " + ammoBackPack;
        } else if (wpn == pistol) {
            textAmmoPistol.text = wpn.bulletInMagazine + " / " + backpack.ammoPistol;
        }
    }

    IEnumerator ChangeWeapon(WeaponInstance weaponSelected)
    {
        canAttack = false;
        if (weaponSelected == currentWeapon) { // unhold weapon
            animator.SetTrigger("ChangeWeapon");
            yield return new WaitForSeconds(.4f);
            animator.SetLayerWeight(1, 0f);
            weaponSelected.NonActivate();
            currentWeapon = null;
            if (audioChangeWeapon != null) audioSource.PlayOneShot(audioChangeWeapon);
        } else {
            animator.SetLayerWeight(1, 1f);
            animator.SetTrigger("ChangeWeapon");
            if (currentWeapon != null)
                currentWeapon.NonActivate();
            yield return new WaitForSeconds(.2f);
            currentWeapon = weaponSelected;
            currentWeapon.Use();
            UIAmmoWeapon(currentWeapon);
            if (audioChangeWeapon != null) audioSource.PlayOneShot(audioChangeWeapon);
        }
        canAttack = true;
        UIChangeWeapon();
    }

    void UIChangeWeapon()
    {
        panelMainWpn.color = defaultColorPanel;
        panelSecondWpn.color = defaultColorPanel;
        panelPistol.color = defaultColorPanel;
        panelMeleeWpn.color = defaultColorPanel;
        if (currentWeapon == mainWeapon) {
            panelMainWpn.color = selectedColorPanel;
        } else if (currentWeapon == secondWeapon) {
            panelSecondWpn.color = selectedColorPanel;
        } else if (currentWeapon == pistol) {
            panelPistol.color = selectedColorPanel;
        } else if (currentWeapon == meleeWeapon) {
            panelMeleeWpn.color = selectedColorPanel;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        GameObject go = collider.gameObject;
        if (go.CompareTag("Weapon")) {
            WeaponType type = go.GetComponent<WeaponInstance>().type;
            if (type == WeaponType.AR && mainWeapon != null && !haveMainWpn) {
                haveMainWpn = true;
                imageAR.gameObject.SetActive(true);
                StartCoroutine(ChangeWeapon(mainWeapon));
            } else if (type == WeaponType.SG && secondWeapon != null && !haveSecondWpn) {
                haveSecondWpn = true;
                imageSG.gameObject.SetActive(true);
                StartCoroutine(ChangeWeapon(secondWeapon));
            } else if (type == WeaponType.Pistol && pistol != null && !havePistol) {
                havePistol = true;
                imagePistol.gameObject.SetActive(true);
                StartCoroutine(ChangeWeapon(pistol));
            } else if (type == WeaponType.Melee && meleeWeapon != null && !haveMeleeWpn) {
                haveMeleeWpn = true;
                imageMelee.gameObject.SetActive(true);
                StartCoroutine(ChangeWeapon(meleeWeapon));
            }
            Destroy(go);
            audioSource.PlayOneShot(audioPickUp);
        } else if (go.CompareTag("Ammo")) {
            Ammo ammo = go.GetComponent<Ammo>();
            if (ammo.type == WeaponType.AR) {
                backpack.ammoAR += ammo.capacity;
                if (haveMainWpn) UIAmmoWeapon(mainWeapon);
            } else if (ammo.type == WeaponType.SG) {
                backpack.ammoSG += ammo.capacity;
                if (haveSecondWpn) UIAmmoWeapon(secondWeapon);
            } else if (ammo.type == WeaponType.Pistol) {
                backpack.ammoPistol += ammo.capacity;
                if (havePistol) UIAmmoWeapon(pistol);
            }
            ammo.Hide();
            audioSource.PlayOneShot(audioPickUp);
        }
    }
}
