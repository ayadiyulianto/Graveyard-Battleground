using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChange : MonoBehaviour
{
    public GameObject firstPersonCamera, thirdPersonCamera;
    bool isFPC = true;

    void Start()
    {
        if (isFPC) {
            firstPersonCamera.SetActive(true);
            thirdPersonCamera.SetActive(false);
        } else {
            firstPersonCamera.SetActive(false);
            thirdPersonCamera.SetActive(true);
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Camera")) {
            if (isFPC) {
                firstPersonCamera.SetActive(false);
                thirdPersonCamera.SetActive(true);
                isFPC = false;
            } else {
                firstPersonCamera.SetActive(true);
                thirdPersonCamera.SetActive(false);
                isFPC = true;
            }
        }
    }
}
