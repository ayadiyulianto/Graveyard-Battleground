using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    public GameObject player;
    public float mouseSensitivity = 100f;
    float xRotation = 0f;
    Transform playerTransform;
    PlayerStats playerStats;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        playerTransform = player.transform;
        playerStats = player.GetComponent<PlayerStats>();
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        if (!playerStats.isDead) {
            playerTransform.Rotate(Vector3.up * mouseX);
        }
    }
}
