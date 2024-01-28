using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.InputSystem;

public class StatePlayerJoin : GameState
{
    List<bool> playerReadyState;
    Coroutine startCountdownCoroutine;

    public StatePlayerJoin(GameManager GameManager) : base(GameManager)
    {

    }

    public override IEnumerator Start()
    {
        playerReadyState = new();
        gameManager.soundManager.PlayNewClip(gameManager.combatMusic);
        yield break;
    }

    public override void AddPlayer(PlayerInput newPlayer)
    {
        gameManager.playerInputList.Add(newPlayer);
        playerReadyState.Add(false);
        gameManager.cameraPivot.AddPlayer(newPlayer);
        Debug.Log("Player " + (LastPlayerID()) + " joins !");
        gameManager.gameMenu.PlayerJoin(LastPlayerID());


        newPlayer.transform.position = LevelManager.Instance.bacALoser.position;
        //Faire Spawn dans la salle d'attente
        Debug.Log("Spawn : " + newPlayer.gameObject);
    }

    int LastPlayerID()
    {
        return gameManager.playerInputList.ToArray().Length - 1;
    }

    public IEnumerator StartSoon()
    {
        int countdown = 10;
        while (countdown > 0)
        {
            countdown--;
            gameManager.gameMenu.playerJoinMenu.UpdateCountDown(countdown);
            yield return new WaitForSeconds(1);
        }
        StartGame();
    }

    public void StartGame()
    {
        startCountdownCoroutine = null;
        gameManager.playerInputManager.DisableJoining();
        gameManager.gameMenu.playerJoinMenu.gameObject.SetActive(false);
        gameManager.SetState(new StateMainGame(gameManager));
        LevelManager.Instance.LoadRandomLevel();
    }

    public override void ReceivePlayerInput(PlayerInput player)
    {
        if (gameManager.playerInputList.Contains(player))
        {
            int ID = gameManager.playerInputList.IndexOf(player);
            Debug.Log("Player " + ID + " is ready !");
            gameManager.gameMenu.PlayerReady(ID);
            playerReadyState[ID] = true;

            int readyCount = 0;
            for(int i = 0; i < gameManager.playerInputList.Count; i++)
            {
                if (playerReadyState[i] == true)
                {
                    readyCount ++;
                }
            }
            
            if(readyCount == gameManager.playerInputList.Count && startCountdownCoroutine == null)
            {
                Debug.Log("Starting game soon");
                startCountdownCoroutine = gameManager.StartCoroutine(StartSoon());
            }
        }
    }
}
