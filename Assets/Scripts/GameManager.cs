using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public List<PlayerInput> playerInputList;
    public InGameMenu gameMenu;
    public PlayerInputManager playerInputManager;
    public PlayerInput player1Prefab;
    PlayerInput player1;

    // Start is called before the first frame update
    void Start()
    {
        playerInputList = new List<PlayerInput>();
        playerInputManager = GetComponent<PlayerInputManager>();
        playerInputManager.EnableJoining();
        //player1 = Instantiate(player1Prefab);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddPlayer(PlayerInput newPlayer)
    {
        playerInputList.Add(newPlayer);
        print("Player " + (LastPlayerID()) + " joins !");
        gameMenu.PlayerJoin(LastPlayerID());
       
    }

    int LastPlayerID()
    {
        return playerInputList.ToArray().Length - 1;
    }
}
