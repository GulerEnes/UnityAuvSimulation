using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotLook : MonoBehaviour
{
    public Transform player;

    public float sensitivity = 10;
    float y = 0;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        y += Input.GetAxis("Mouse X") * sensitivity;
        player.transform.localRotation = Quaternion.Euler(0, y, 0);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
                Cursor.lockState = CursorLockMode.None;

            if (Cursor.lockState == CursorLockMode.None)
                Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
