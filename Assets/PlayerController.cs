using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    GameManager gameManager;
    PlayerInput playerInput;

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
}
