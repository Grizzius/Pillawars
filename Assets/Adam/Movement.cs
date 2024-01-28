using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public Animator animator;
    private Vector2 PlayerMovementInput;
    private Vector2 lastPlayerDirection;

    [SerializeField] private PlayerInput input;
    
    [SerializeField] private Rigidbody playerBody;
    [SerializeField] private Transform PlayerCamera;

    //Jump System
    Vector3 jumpDirection;
    [SerializeField] private LayerMask Floor;
    [SerializeField] private Transform Feet;
    public float jumpUpAngle = 10;
    public float timerJump = 0;
    public float jumpDuration = 0;
    public float maxJumpDuration = 2;
    public float turnSpeed;
    public bool isJumpings = false;

    enum JumpCharge
    {
        normal,
        loin,
        tresLoin
    }

    /// <summary>
    /// Temps de touche maintenue requis en cl� et le temps de saut � appliquer en valeur
    /// </summary>
    Dictionary<float, float> jumpStep = new Dictionary<float, float>()
    {
        { 0.0f, 0.5f }, {2, .8f}
    };


    [SerializeField] private float Speed;
    [SerializeField] private float Sensitivity;
    [SerializeField] private float JumpForce;

    private void Start()
    {

    }

    public void OnMovement(InputAction.CallbackContext callbackContext)
    {
        PlayerMovementInput = callbackContext.ReadValue<Vector2>().normalized;
        if(PlayerMovementInput != Vector2.zero ) 
        {
            animator.SetBool("move", true);
            lastPlayerDirection = PlayerMovementInput;
        }
        else
        {
            animator.SetBool("move", false);
        }
    }

    void RotateForward(Vector2 direction)
    {
        transform.forward = new Vector3(direction.x, 0, direction.y);
        //Quaternion rotGoal = Quaternion.LookRotation(new Vector3(direction.x,0,direction.y));
        //transform.rotation = Quaternion.Slerp(transform.rotation, rotGoal, turnSpeed);
    }

    public void OnJump(InputAction.CallbackContext callbackContext)
    {
        Debug.Log("Jump");

        if (callbackContext.started)
        {
            Debug.Log("CCCCOMMMMMMMMENCCCCE");
            StartCoroutine(HoldButtonRoutine());
        }

        IEnumerator HoldButtonRoutine()
        {
            Debug.Log("Jump started");
            timerJump = 0;
            yield return new WaitForEndOfFrame();
            if (callbackContext.canceled)
            {
                yield break;
            }
            yield return new WaitUntil(callbackContext.ReadValueAsButton);
            while (callbackContext.ReadValueAsButton())
            {
                Debug.Log("holding Jump");
                timerJump += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForEndOfFrame();
            Debug.Log("Finished jump, jump force : " + timerJump);
            Jump(timerJump);
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {        
        if(isJumpings)
        {
            playerBody.AddForce(new Vector3(jumpDirection.x * JumpForce, 0, jumpDirection.y * JumpForce), ForceMode.VelocityChange);
        }
        else
        {
            Vector3 MoveVector = new Vector3(PlayerMovementInput.x, 0, PlayerMovementInput.y) * Speed * Time.deltaTime;
            transform.position += MoveVector;
            if (PlayerMovementInput != Vector2.zero)
            {
                RotateForward(PlayerMovementInput);
            } 
        }
    }

    /// <summary>
    /// Effectue le saut du prsonnage
    /// </summary>
    /// <param name="t"></param>
    public void Jump( float t)
    {
        jumpDuration = 0;
        //Va checker quel longueur de saut correspond au temps de maintiens de la touche de saut
        foreach(KeyValuePair<float, float> keyValuePair in jumpStep)
        {
            if(keyValuePair.Key < timerJump)
            {
                jumpDuration = keyValuePair.Value;
            }
        }
        if (jumpDuration > 0)
        {
            //playerBody.AddForce((Vector3.up * jumpUpAngle), ForceMode.Impulse);
            jumpDirection = new Vector3(lastPlayerDirection.x, 0, lastPlayerDirection.y);
            StartCoroutine(JumpDuration(jumpDuration));
        }
        timerJump = 0;
    }

    IEnumerator JumpDuration(float deltaTime)
    {
        float t = deltaTime;
        animator.SetBool("jump", true);
        isJumpings = true;
        jumpDirection = transform.forward * JumpForce;
        playerBody.AddForce( new Vector3(0, jumpUpAngle, 0), ForceMode.VelocityChange);
        while (t > 0)
        {
            playerBody.AddForce(jumpDirection, ForceMode.VelocityChange);
            t -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("isJumpings set to false");
        playerBody.velocity = Vector3.zero;
        isJumpings = false;
        animator.SetBool("jump", false);
    }

    public float GetJumpCharge()
    {
        var res = jumpStep.Last().Key;
        if(timerJump > res)
        {
            res = 1;
        }
        else
        {
            res = timerJump / res;
        }
        return res;
    }
}
