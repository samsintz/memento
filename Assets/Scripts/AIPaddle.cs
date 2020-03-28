using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enumeration of possible AISKill levels
/// </summary>
public enum AISkill 
{
    Invalid = 0,
    Easy = 1,
    Medium = 2, 
    Hard = 3,
}

/// <summary>
/// AI Paddle
/// Moves in relation to the ball's x position
/// </summary>
public class AIPaddle : SceneObject 
{
    private float _distanceMultiplier;
    private bool _canTick = true;
    private Transform _target; // object x position tracks

	public void Start()
	{
	    GameState.OnPreGame += Reset;
	    GameState.OnReplayRunning += Reset;

        _distanceMultiplier = Client.instance.difficultyManager.MapDifficultyToAISkill();
	}

    private void OnDestroy()
    {
        GameState.OnPreGame -= Reset;
        GameState.OnReplayRunning -= Reset;
    }

    private void Update()
    {
        if(_canTick == false || _target == null)
        {
            return;
        }

        Vector2 position = transform.position;

        float xPosition = Mathf.Lerp(position.x, _target.position.x, Time.deltaTime * _distanceMultiplier);
        position.x = xPosition;

        transform.position = position;
    }

    public override void Freeze()
    {
        _canTick = false;
    }

    public override void Reset()
    {
        base.Reset();
        _canTick = true;
    }

    public void SetBallTarget(Transform target)
    {
        _target = target;
    }
}
