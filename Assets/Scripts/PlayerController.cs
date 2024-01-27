using System.Collections;
using System.Collections.Generic;
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
    public grabable grabed;
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
        if (other.GetType() == typeof(grabable))
        {
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
        if (context.started)
        {
            StartCoroutine(HoldItem());
        }
        IEnumerator HoldItem()
        {
            yield return new WaitForEndOfFrame();
            if (context.canceled)
            {
                yield break;
            }
            TestForGrab();
            if(grabed == null)
            {
                yield break;
            }
            yield return new WaitUntil(context.ReadValueAsButton);
            while (context.ReadValueAsButton())
            {
                yield return new WaitForEndOfFrame();
            }
            ThrowItem();
        }
    }
    void TestForGrab()
    {
        Dictionary<grabable, float> distances = new();
;       foreach(grabable grabable in canBeGrabbed)
        {
            distances.Add(grabable, Vector3.Distance(transform.position, grabable.transform.position));
        }
        Dictionary<>
    }
    void GrabItem(grabable item)
    {
        item.transform.parent = grabTransform;
        grabed = item;
        grabed.IsGrabed(this);
    }

    void ThrowItem()
    {
        Debug.Log("yeet");
        grabed.transform.parent = null;
        grabed.Yeet(transform.forward * 1000, this);
    }
}
