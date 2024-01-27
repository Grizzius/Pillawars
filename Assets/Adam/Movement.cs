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
    private bool playerJumpInput;
    private Vector2 PlayerMouseInput;
    private float xRot;

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
        { 0.0f, 0.15f }, {2, 0.3f}
    };


    [SerializeField] private float Speed;
    [SerializeField] private float Sensitivity;
    [SerializeField] private float JumpForce;

    private void Start()
    {
        setKinematic(true);
    }

    public void OnMovement(InputAction.CallbackContext callbackContext)
    {
        PlayerMovementInput = callbackContext.ReadValue<Vector2>();
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

    public void OnJump(InputAction.CallbackContext callbackContext)
    {
        Debug.Log("Jump");
        playerJumpInput = callbackContext.ReadValueAsButton();

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

    private void Update()
    {
        PlayerMouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
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
            Vector3 MoveVector = transform.TransformDirection(PlayerMovementInput.x, 0, PlayerMovementInput.y) * Speed;
            playerBody.velocity = new Vector3(MoveVector.x, playerBody.velocity.y, MoveVector.z);
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
            playerBody.AddForce(Vector3.up * jumpUpAngle, ForceMode.Impulse);
            jumpDirection = lastPlayerDirection;
            StartCoroutine(JumpDuration(jumpDuration));
        }
        timerJump = 0;
    }

    /// <summary>
    /// true quand on veut que le personnage soit contr�lable
    /// false lorsque l'on veut profiter de la ragdoll
    /// </summary>
    /// <param name="newValue"></param>
    void setKinematic(bool newValue)
    {

        //Get an array of components that are of type Rigidbody
        Component[] components = GetComponentsInChildren(typeof(Rigidbody));

        //For each of the components in the array, treat the component as a Rigidbody and set its isKinematic and detectCollisions property
        foreach (Component c in components)
        {
            (c as Rigidbody).isKinematic = newValue;
            (c as Rigidbody).detectCollisions = !newValue;
        }

        //Sets PLAYER rigid body as opposite
        playerBody.isKinematic = !newValue;
        playerBody.detectCollisions = newValue;
    }

    IEnumerator JumpDuration(float deltaTime)
    {
        animator.SetBool("jump", true);
        isJumpings = true;
        yield return new WaitForSeconds(deltaTime);
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
