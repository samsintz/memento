using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// This class manages the instant replay simultion
/// </summary>
public class SimulationManager<T> : IDisposable 
{
    private Caretaker<T> _caretaker;
    public SimulationManager(Caretaker<T> caretaker)
    {
        _caretaker = caretaker;
        GameState.OnReplayRunning += ProcessReplay;
    }
    
    public void Dispose()
    {
        GameState.OnReplayRunning -= ProcessReplay;
    }

    private void ProcessReplay()
    {
        Client.instance.StartCoroutine(ProcessReplayHelper());
    }

    private IEnumerator ProcessReplayHelper()
    {
        float internalTime = 0f;
        // start the ball
        Client.instance.ball.MoveBallToStartGame();

        while(_caretaker.MementoCount() > 0)
        {
            Memento<T> memento = _caretaker.PeekMemento();
            float mementoTime = memento.creationTime;

            // process all mementos that occured during a frame
            if(Mathf.Abs(mementoTime - internalTime) <= Time.deltaTime) 
            {
                memento = _caretaker.GetMemento();
                T state = memento.GetState();
                _caretaker.originator.SetState(state);
                
            } else {
                internalTime += Time.deltaTime;
                yield return null;
            }
        }

        Client.instance.ball.Freeze();
        // Pad time to start game again to make it less jarring
        yield return new WaitForSeconds(0.5f);
        Client.instance.gameState.state = GameStateEnum.PreGame;
    }
}
