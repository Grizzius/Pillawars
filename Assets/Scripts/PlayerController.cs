using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Services.Analytics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerController : MonoBehaviour
{
    GameManager gameManager;
    PlayerInput playerInput;

    [SerializeField] Transform grabTransform;
    [SerializeField] private ParticleSystem impactParticle;
    [SerializeField] private AudioSource throwSoundPrefab;
    [SerializeField] private Rigidbody rb;

    public float yeetStrenght;
    public float bonkStrenght;

    private Coroutine bonkRoutine;

    public grabable grabed;
    Coroutine grabItemRoutine = null;
    private List<grabable> canBeGrabbed;
    
    
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        playerInput = GetComponent<PlayerInput>();
        canBeGrabbed = new();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<grabable>())
        {
            Debug.Log("Grabable can be grabed");
            canBeGrabbed.Add(other.GetComponent<grabable>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (canBeGrabbed.Contains(other.GetComponent<grabable>()))
        {
            canBeGrabbed.Remove(other.GetComponent<grabable>());
        }
    }

    public void SendInputToGameManager(InputAction.CallbackContext callbackContext)
    {
        if(gameManager != null)
        {
            gameManager.ReceivePlayerInput(playerInput);
        }
    }

    public void OnGrabItem(InputAction.CallbackContext context)
    {
        if (context.started && grabItemRoutine == null)
        {
            grabItemRoutine = StartCoroutine(HoldItem());
        }
        IEnumerator HoldItem()
        {
            yield return new WaitForEndOfFrame();
            if (context.canceled)
            {
                grabItemRoutine = null;
                yield break;
            }
            TestForGrab();
            if(grabed == null)
            {
                grabItemRoutine = null;
                yield break;
            }
            yield return new WaitUntil(context.ReadValueAsButton);
            while (context.ReadValueAsButton())
            {
                yield return new WaitForEndOfFrame();
            }
            ThrowItem(transform.forward);
            yield return new WaitForSeconds(1f);
            grabItemRoutine = null;
        }
    }
    void TestForGrab()
    {
        Debug.Log("testing for grab");
        Dictionary<grabable, float> distances = new();
;       foreach(grabable grabable in canBeGrabbed)
        {
            Debug.Log("found items to grab");
            if (!distances.ContainsKey(grabable))
            {
                distances.Add(grabable, Vector3.Distance(transform.position, grabable.transform.position));
            }
            
        }

        if(distances.Count > 0)
        {
            float minDistance = distances.Values.Min();

            foreach (grabable key in distances.Keys)
            {
                if (distances[key] == minDistance)
                {
                    GrabItem(key);
                    break;
                }
            }
        }
    }

    void GrabItem(grabable item)
    {
        item.transform.parent = grabTransform;
        grabed = item;
        grabed.IsGrabed(this);
    }

    public IEnumerator SpawnSound(AudioSource source)
    {
        var audio = Instantiate(source, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(source.clip.length);
        Destroy(audio);
    }

    void ThrowItem(Vector3 direction)
    {
        StartCoroutine(SpawnSound(throwSoundPrefab));

        grabed.transform.parent = null;
        StartCoroutine(grabed.Yeet(direction * yeetStrenght, this));
        grabed = null;
    }

    public void Bonk(Vector3 bonkForce)
    {
        if(bonkRoutine == null)
        {
            bonkRoutine = StartCoroutine(BonkCoroutine(bonkForce));
        }
        
    }

    IEnumerator BonkCoroutine(Vector3 bonkForce)
    {
        print("Start BONK");
        Instantiate(impactParticle, transform.position, Quaternion.identity);
        Vector3 dir = new Vector3(bonkForce.x, 0, bonkForce.z);

        float initT = 0.5f;
        float t = initT;
        while(t > 0)
        {
            playerInput.enabled = false;
            rb.AddForce(dir * bonkStrenght * (1 - t / initT), ForceMode.Acceleration);
            t -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        playerInput.enabled = true;
        bonkRoutine = null;
    }
}
