using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class grabable : MonoBehaviour
{
    Rigidbody rb;
    public Collider collider;
    public PlayerController graber;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IsGrabed(PlayerController graber)
    {
        IsGrabed(graber.GetComponent<Collider>());
    }

    void IsGrabed(Collider other)
    {
        Physics.IgnoreCollision(collider, other, true);
    }
    public void IsDropped(PlayerController dropper)
    {
        IsDropped(dropper.GetComponent<Collider>());
    }

    void IsDropped(Collider other)
    {
        Physics.IgnoreCollision(collider, other, false);
    }

    public void Yeet(Vector3 force)
    {
        rb.AddForce(force);
    }
}
