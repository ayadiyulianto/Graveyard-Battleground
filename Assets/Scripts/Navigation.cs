using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Navigation : MonoBehaviour
{
    public GameObject exitPanel;
    public bool cursorLockedState = false;

    void Start()
    {
        exitPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && exitPanel != null) {
            exitPanel.SetActive(true);
            if (cursorLockedState) Cursor.lockState = CursorLockMode.None;
        }
    }

    public void Load(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void HideExitPanel()
    {
        exitPanel.SetActive(false);
        if (cursorLockedState) Cursor.lockState = CursorLockMode.Locked;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
