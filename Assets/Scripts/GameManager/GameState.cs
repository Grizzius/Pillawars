using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class GameState
{
    protected GameManager gameManager;
    public GameState(GameManager GameManager)
    {
        gameManager = GameManager;
    }

    public virtual IEnumerator Start()
    {
        yield break;
    }

    public virtual void AddPlayer(PlayerInput newPlayer)
    {

    }

    public virtual void ReceivePlayerInput(PlayerInput player)
    {

    }
}
