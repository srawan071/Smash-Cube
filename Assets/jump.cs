using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jump : MonoBehaviour
{
    Rigidbody rb;
  public  float force;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * -49.05f * Time.fixedDeltaTime;
        }
        else if (rb.velocity.y > 0)
        {
            rb.velocity += Vector3.up * -49.05f * Time.fixedDeltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.velocity = Vector3.up*force;
    }
}
