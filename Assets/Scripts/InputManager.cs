using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// Concrete originator in this example
/// </summary>
public class InputManager : IOriginator<InputState>, IDisposable 
{
    private float _inputState;
    private Coroutine _inputPollCoroutine;

    public InputManager()
    {
        GameState.OnInGame += PollForInput;
        GameState.OnLost += StopInputPolling;
    }

    public void Dispose()
    {
        GameState.OnInGame -= PollForInput;
        GameState.OnLost -= StopInputPolling;
    }

    public void PollForInput()
    {
        // Only monobehaviors can start coroutines
        _inputPollCoroutine = Client.instance.StartCoroutine(PollforInputHelper());
    }

    private IEnumerator PollforInputHelper()
    {
        while(true)
        {
            float horizontal = -2f; //invalid value
            if(Input.GetKey(KeyCode.LeftArrow))
            {
                horizontal = -1f;
            }
            if(Input.GetKey(KeyCode.RightArrow))
            {
                horizontal = 1f;
            }
            
            if(Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
            {
                horizontal = 0f;
            }

            if(horizontal != -2f)
            {
                if(_inputState != horizontal)
                {
                    // new input value
                    _inputState = horizontal;
                    Client.instance.inputStateCaretaker.AddMemento(CreateMemento());
               }
            }
            // poll each frame
            yield return null;
        }
    }

    private void StopInputPolling()
    {
        Client.instance.StopCoroutine(_inputPollCoroutine);

        // make sure movement does not stick
        _inputState = 0;
        Client.instance.inputStateCaretaker.AddMemento(CreateMemento());
    }

    public float GetInput()
    {
        return _inputState;
    }

    public Memento<InputState> CreateMemento()
    {
        InputState inputState = new InputState(_inputState);
        Memento<InputState> memento = new Memento<InputState>(inputState, Time.timeSinceLevelLoad);
        return memento;
    }

    public void SetState(InputState mementoState)
    {
        _inputState = mementoState.state;
    }
}

