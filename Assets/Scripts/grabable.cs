using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class grabable : MonoBehaviour
{
    Rigidbody rb;
    public Collider collider;
    public PlayerController graber;
    bool beingThrown = false;
    [SerializeField] private ParticleSystem particle;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        while (beingThrown)
        {
            if(rb.velocity.magnitude > 2)
            {
                particle.gameObject.SetActive(true);
            }
            else
            {
                particle.gameObject.SetActive(false);
                beingThrown = false;
            }
        }
    }

    public void IsGrabed(PlayerController Graber)
    {
        graber = Graber;
        IsGrabed(graber.GetComponent<Collider>());
    }

    void IsGrabed(Collider other)
    {
        transform.localPosition = new Vector3(0, 0, 0);
        transform.localRotation = Quaternion.identity;
        rb.isKinematic = true;
        Physics.IgnoreCollision(collider, other, true);
    }
    public void IsDropped(PlayerController dropper)
    {
        IsDropped(dropper.GetComponent<Collider>());
    }

    void IsDropped(Collider other)
    {
        rb.isKinematic = false;
        Physics.IgnoreCollision(collider, other, false);
    }

    public void Yeet(Vector3 force, PlayerController dropper)
    {
        IsDropped(dropper);
        rb.AddForce(force);
        beingThrown = true;
    }
}
