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
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerFX fx;
    [SerializeField] private TrailRenderer trail;

    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private AudioSource landSound;

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
    public bool canMove = true;

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
        { 0.0f, 0.5f }, {2, 1}
    };


    [SerializeField] private float Speed;
    [SerializeField] private float Sensitivity;
    [SerializeField] private float JumpForce;

    private void Start()
    {
        CalculateTrailColor();
    }

    void CalculateTrailColor()
    {
        print(fx.colorPlayer);
        var gradient = new Gradient();

        var colorKeys = new GradientColorKey[2];
        colorKeys[0] = new GradientColorKey(fx.colorPlayer, 0f);
        colorKeys[1] = new GradientColorKey(fx.colorPlayer, 1f);

        var alpha = new GradientAlphaKey[2];
        alpha[0] = new GradientAlphaKey(1f, 0f);
        alpha[1] = new GradientAlphaKey(1f, 1f);

        gradient.SetKeys(colorKeys, alpha);

        print(gradient.Evaluate(0.5f));

        trail.colorGradient = gradient;
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
        if (callbackContext.started && !isJumpings && Physics.OverlapSphere(Feet.transform.position, 0.5f, Floor).Count() > 0)
        {
            StartCoroutine(HoldButtonRoutine());
        }

        IEnumerator HoldButtonRoutine()
        {
            timerJump = 0;
            yield return new WaitForEndOfFrame();
            if (callbackContext.canceled)
            {
                yield break;
            }
            yield return new WaitUntil(callbackContext.ReadValueAsButton);
            while (callbackContext.ReadValueAsButton())
            {
                timerJump += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForEndOfFrame();
            Jump(timerJump);
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            MovePlayer();
        }
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
        trail.enabled = true;
        StartCoroutine(playerController.SpawnSound(jumpSound));
        jumpDirection = transform.forward * JumpForce;
        playerBody.AddForce( new Vector3(0, jumpUpAngle, 0), ForceMode.VelocityChange);
        while (t > 0)
        {
            playerBody.AddForce(jumpDirection, ForceMode.VelocityChange);
            t -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        playerBody.velocity = Vector3.zero;
        isJumpings = false;
        trail.enabled = false;
        StartCoroutine(playerController.SpawnSound(landSound));
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
