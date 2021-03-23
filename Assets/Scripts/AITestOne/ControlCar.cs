using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ControlCar : MonoBehaviour
{
    private IControls controls;
    private Rigidbody rb;

    private void Awake()
    {
        controls = GetComponent(typeof(IControls)) as IControls;
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float maxSpeed = controls.InputY >= 0 ? 15 : 8;
        float speed = controls.InputY >= 0 ? 12 : 8;

        if (rb.velocity.magnitude < maxSpeed)
        {
            rb.AddRelativeForce(new Vector3(0, 0, controls.InputY) * speed);
        } 
        rb.rotation = Quaternion.Euler(rb.rotation.eulerAngles + new Vector3(0, controls.InputX * Time.deltaTime * 150, 0));
    }
}
