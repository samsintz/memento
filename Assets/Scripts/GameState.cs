using UnityEngine;
using UnityEngine.SceneManagement;
using System;
public enum GameStateEnum 
{
    Invalid = 0,
    PreGame = 1,
    InGame = 2,
    Lost = 3,
    ReplayRunning = 4,
}

/// <summary>
/// Central GameState class with events for each state change
/// The bulk of the game runs on these state transition hooks
/// </summary>
public class GameState
{
    public static event Action OnPreGame;

    public static event Action OnInGame;

    public static event Action OnLost;

    public static event Action OnReplayRunning;

    private GameStateEnum _state;
    public GameStateEnum state
    {
        get { return _state; }
        set
        {
            if(_state != value)
            {
                HandleGameStateChange(_state, value);
            }
        }
    }

    public GameState()
    {
        state = GameStateEnum.Invalid;
    }

    private void HandleGameStateChange(GameStateEnum currentState, GameStateEnum nextState)
    {
        // dont let replay transition to lost
        if(currentState == GameStateEnum.ReplayRunning && nextState == GameStateEnum.Lost)
        {
            Debug.Log("Illegal state transition: Replay -> Lost");
            return;
        }

        // set the internal state
        _state = nextState;

        switch(nextState)
        {
            case GameStateEnum.Lost:
                OnLost();
                Client.instance.Timer(1f, () => state = GameStateEnum.ReplayRunning);
                break;
            case GameStateEnum.ReplayRunning:
                OnReplayRunning();
                break;
            case GameStateEnum.PreGame:
                OnPreGame();
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;
            case GameStateEnum.InGame:
                OnInGame();
                break;
            default:
                break;
        }
    }
}