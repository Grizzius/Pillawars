using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class grabable : MonoBehaviour
{
    Rigidbody rb;
    public Collider _collider;
    public PlayerController graber;
    bool beingThrown = false;
    [SerializeField] private ParticleSystem particle;

    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (beingThrown)
        {
            EndThrow();
        }
    }

    void StartThrow(Vector3 force)
    {
        rb.AddForce(force, ForceMode.Impulse);
        beingThrown = true;
        particle.gameObject.SetActive(true);
    }

    void EndThrow()
    {
        beingThrown = false;
        particle.gameObject.SetActive(false);
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
        Physics.IgnoreCollision(_collider, other, true);
    }
    public void IsDropped(PlayerController dropper)
    {
        StartCoroutine(IsDropped(dropper.GetComponent<Collider>()));
    }

    IEnumerator IsDropped(Collider other)
    {
        graber = null;
        yield return new WaitForEndOfFrame();
        rb.isKinematic = false;
        yield return new WaitForSeconds(0.1f);
        Physics.IgnoreCollision(_collider, other, false);
    }

    public IEnumerator Yeet(Vector3 force, PlayerController dropper)
    {
        IsDropped(dropper);
        yield return new WaitForEndOfFrame();
        StartThrow(force);
    }
}
