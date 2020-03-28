using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player class used to handle paddle movement
/// </summary>
public class Player : SceneObject 
{
    [SerializeField]
    private float _speed = 1f;

    protected void Start()
    {
        GameState.OnPreGame += Reset;
        GameState.OnPreGame += Freeze;
        GameState.OnReplayRunning += Freeze;
        GameState.OnReplayRunning += Reset;
    }

    private void OnDestroy()
    {
        GameState.OnPreGame -= Reset;
        GameState.OnPreGame -= Freeze;
        GameState.OnReplayRunning -= Freeze;
        GameState.OnReplayRunning -= Reset;
    }

    public void VelocityMovement(Vector2 velocity)
    {
        rigidBody2D.velocity = velocity * _speed;
    }

    public void Update()
    {
        Vector2 velocity = Vector2.zero;
        velocity.x = Client.instance.inputManager.GetInput();

        VelocityMovement(velocity);
    }
}
