using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Movement : MonoBehaviour
{

    private Vector3 PlayerMovementInput;
    private Vector2 PlayerMouseInput;
    private float xRot;

    [SerializeField] private PlayerInput input;
    
    [SerializeField] private Rigidbody playerBody;
    [SerializeField] private Transform PlayerCamera;

    [SerializeField] private LayerMask Floor;
    [SerializeField] private Transform Feet;

    [SerializeField] private float Speed;
    [SerializeField] private float Sensitivity;
    [SerializeField] private float JumpForce;


<<<<<<< Updated upstream
=======
    private void Awake()
    {
        playerBody = GetComponent<Rigidbody>();
        input = GetComponent<PlayerInput>();
    }

    public void OnMovement(InputAction.CallbackContext callbackContext)
    {
        PlayerMovementInput = callbackContext.ReadValue<Vector2>();
    }
>>>>>>> Stashed changes

    private void Update()
    {
        PlayerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        PlayerMouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        MovePlayer();
    }


    public void OnJump(InputAction.CallbackContext callbackContext)
    {        
        if (callbackContext.performed && Physics.CheckSphere(Feet.position, 0.2f, Floor))
        {
            playerBody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
        }
    }

    private void MovePlayer()
    {
        Vector3 MoveVector = transform.TransformDirection(PlayerMovementInput) * Speed;
        playerBody.velocity = new Vector3(MoveVector.x, playerBody.velocity.y, MoveVector.z);

        if (input.actions["Jump"].triggered)
        {
            if(Physics.CheckSphere(Feet.position, 0.1f, Floor))
            {
                playerBody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
            }
        }
    }
  
}
