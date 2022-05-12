using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;

    public float speed = 6f;
    public float rotationSpeed = 6f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        //if the spacebutton is down, add a force moving it forward
        if (Input.GetButton("Jump"))
            rb.AddForce(transform.forward, ForceMode.Force);

        //left rotation
        if (Input.GetAxis("Horizontal") < 0)
            rb.AddTorque(-transform.up, ForceMode.Force);
            

        //right rotation
        if (Input.GetAxis("Horizontal") > 0)
            rb.AddTorque(transform.up, ForceMode.Force);

        //downward rotation
        if (Input.GetAxis("Vertical") < 0)
            rb.AddTorque(-transform.right, ForceMode.Force);

        //upwatd rotation
        if (Input.GetAxis("Vertical") > 0)
            rb.AddTorque(transform.right, ForceMode.Force);

     
            

    }

    
}
