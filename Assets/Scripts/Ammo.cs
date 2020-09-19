using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    public WeaponType type;
    public int capacity;

    public void Hide()
    {
        GetComponent<Collider>().enabled = false;
        foreach (Transform child in transform) {
            child.gameObject.SetActive(false);
        }
        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(10f);
        GetComponent<Collider>().enabled = true;
        foreach (Transform child in transform) {
            child.gameObject.SetActive(true);
        }
    }
}
