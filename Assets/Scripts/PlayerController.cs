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

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 10f))
        {
            Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.blue);
            Debug.Log("Did hit");
            grabable hitGrabable = hit.transform.gameObject.GetComponent<grabable>();
            if (hitGrabable != null)
            {
                GrabItem(hitGrabable);
            }
        }
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
