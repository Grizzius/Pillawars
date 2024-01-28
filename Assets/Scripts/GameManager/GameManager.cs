using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : StateMachine
{
    public static GameManager Instance { get; set; }
    public List<PlayerInput> playerInputList;
    public Dictionary<PlayerInput, bool> playerInputDictionary;
    public InGameMenu gameMenu;
    public PlayerInputManager playerInputManager;
    public PlayerInput player1Prefab;
    public CameraPivot cameraPivot;
    public SoundManager soundManager;

    public AudioClip combatMusic;

    public static float musicVolume = 1f;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerInputList = new List<PlayerInput>();
        playerInputManager = GetComponent<PlayerInputManager>();
        playerInputManager.EnableJoining();

        SetState(new StatePlayerJoin(this));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddPlayer(PlayerInput newPlayer)
    {
        state.AddPlayer(newPlayer);
    }

    public void ReceivePlayerInput(PlayerInput player)
    {
        state.ReceivePlayerInput(player);
    }
}
