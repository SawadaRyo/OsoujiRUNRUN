using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursorController : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.S))
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
}
