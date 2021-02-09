using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Add_force : MonoBehaviour
{
    Rigidbody local;

    public float x_force, y_force, z_force;
    public bool x_axis = false, y_axis = false, z_axis = false;
    public bool rise = false;

    void Start()
    {
        local = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if(x_axis == true)
            local.AddForce(transform.right * x_force, ForceMode.Impulse);

        if(y_axis == true)
            local.AddForce(transform.up * y_force, ForceMode.Impulse);

        if(z_axis == true)
            local.AddForce(transform.forward * z_force, ForceMode.Impulse);

        if(rise == true)
            transform.position += Vector3.up * (Time.deltaTime * 2);
    }

}
