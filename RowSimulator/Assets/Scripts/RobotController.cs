using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour
{
    public float speed = 7.0f;
    public Rigidbody rb;

    void Start()
    {
       rb = this.GetComponent<Rigidbody>();
       Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal") * speed;
        float y = Input.GetAxisRaw("Vertical") * speed;
        float z = Input.GetAxisRaw("Hover") * speed;

        Vector3 move = transform.right * x + transform.forward * y + transform.up * z;
        rb.AddForce(move * speed, ForceMode.Acceleration);
    }
}

