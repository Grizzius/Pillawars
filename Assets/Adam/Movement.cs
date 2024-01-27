using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{

    private Vector2 PlayerMovementInput;
    private Vector2 PlayerMouseInput;
    private float xRot;


    
    [SerializeField] private Rigidbody playerBody;
    [SerializeField] private Transform PlayerCamera;

    [SerializeField] private LayerMask Floor;
    [SerializeField] private Transform Feet;

    [SerializeField] private float Speed;
    [SerializeField] private float Sensitivity;
    [SerializeField] private float JumpForce;

    private void Start()
    {

    }

    public void OnMovement(InputAction.CallbackContext callbackContext)
    {
        PlayerMovementInput = callbackContext.ReadValue<Vector2>();
    }

    private void Update()
    {
        PlayerMouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        MovePlayer();
    }

    private void MovePlayer()
    {
        Vector3 MoveVector = transform.TransformDirection(PlayerMovementInput.x,0, PlayerMovementInput.y) * Speed;
        playerBody.velocity = new Vector3(MoveVector.x, playerBody.velocity.y, MoveVector.z);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(Physics.CheckSphere(Feet.position, 0.1f, Floor))
            {
                playerBody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
            }
        }
    }
  
}
