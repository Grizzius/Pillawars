using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    protected GameState state;
    public void SetState(GameState State)
    {
        state = State;
        StartCoroutine(state.Start());
    }
}
