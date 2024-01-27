using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StatePlayerJoin : GameState
{
    public StatePlayerJoin(GameManager GameManager) : base(GameManager)
    {

    }

    public override IEnumerator Start()
    {
        yield break;
    }

    public override void AddPlayer(PlayerInput newPlayer)
    {
        gameManager.playerInputList.Add(newPlayer);
        gameManager.cameraPivot.AddPlayer(newPlayer);
        Debug.Log("Player " + (LastPlayerID()) + " joins !");
        gameManager.gameMenu.PlayerJoin(LastPlayerID());
    }

    int LastPlayerID()
    {
        return gameManager.playerInputList.ToArray().Length - 1;
    }

    public void StartGame()
    {
        gameManager.playerInputManager.DisableJoining();
        gameManager.gameMenu.playerJoinMenu.gameObject.SetActive(false);
        gameManager.SetState(new StateMainGame(gameManager));
    }

    public override void ReceivePlayerInput(PlayerInput player)
    {
        if (gameManager.playerInputList.Contains(player))
        {
            int ID = gameManager.playerInputList.IndexOf(player);
            Debug.Log("Player " + ID + " is ready !");
            gameManager.gameMenu.PlayerReady(ID);
        }
    }
}
