using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{

    private Vector2 PlayerMovementInput;
    private Vector2 lastPlayerDirection;
    private bool playerJumpInput;
    private Vector2 PlayerMouseInput;
    private float xRot;


    
    [SerializeField] private Rigidbody playerBody;
    [SerializeField] private Transform PlayerCamera;

    //Jump System
    Vector3 jumpDirection;
    [SerializeField] private LayerMask Floor;
    [SerializeField] private Transform Feet;
    public float jumpUpAngle = 10;
    public float timerJump = 0;
    public float DefautJumpDuration = 1;
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
    /// Temps de touche maintenue requis en clé et le temps de saut à appliquer en valeur
    /// </summary>
    Dictionary<float, float> jumpStep = new Dictionary<float, float>()
    {
        { 0.25f, 0.1f }, {1, 0.15f}, {2, 0.3f}
    };


    [SerializeField] private float Speed;
    [SerializeField] private float Sensitivity;
    [SerializeField] private float JumpForce;
    public List<Rigidbody2D> rbList;
    public Animator animator;

    private void Start()
    {
        setKinematic(true);


    }

    public void OnMovement(InputAction.CallbackContext callbackContext)
    {
        Debug.Log("Movement update");
        PlayerMovementInput = callbackContext.ReadValue<Vector2>();
        if(PlayerMovementInput != Vector2.zero ) 
        {
            lastPlayerDirection = PlayerMovementInput;
        }
    }

    public void OnJump(InputAction.CallbackContext callbackContext)
    {
        playerJumpInput = callbackContext.ReadValueAsButton();

        if (callbackContext.started)
        {
            StartCoroutine(HoldButtonRoutine());
        }

        IEnumerator HoldButtonRoutine()
        {
            Debug.Log("Jump started");
            timerJump = DefautJumpDuration;
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
    }

    /// <summary>
    /// Gestion de l'input de saut
    /// </summary>
    /// <returns></returns>
    public bool HoldJump()
    {
        if (Physics.OverlapSphere(Feet.position, 0.1f, Floor).Length > 0)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                timerJump = DefautJumpDuration;
                return true;
            }
            else if (Input.GetKey(KeyCode.Space))
            {
                timerJump += Time.deltaTime;
                return true;
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                Debug.Log("Saute " + timerJump);
                Jump(Mathf.Clamp(timerJump, DefautJumpDuration, maxJumpDuration));
                return false;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// true quand on veut que le personnage soit contrôlable
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
        Debug.Log("coroutine " + deltaTime);
        isJumpings = true;
        yield return new WaitForSeconds(deltaTime);
        playerBody.velocity = Vector3.zero;
        isJumpings = false;
    }
}
