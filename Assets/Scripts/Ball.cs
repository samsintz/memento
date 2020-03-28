using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ball class
/// Handles ball movement
/// </summary>
public class Ball : SceneObject, IOriginator<BallState> 
{
    private float _speed;

    private float _lastCollisionTime = 0f;

    protected void Start()
    {
        _speed = Client.instance.difficultyManager.MapDifficultyToBallSpeed();

        GameState.OnLost += OnGameLost;
        GameState.OnPreGame += Reset;
        GameState.OnPreGame += Freeze;
        GameState.OnReplayRunning += Reset;

        MoveBallToStartGame();
    }

    private void OnDestroy()
    {
        GameState.OnLost -= OnGameLost;
        GameState.OnPreGame -= Reset;
        GameState.OnPreGame -= Freeze;
        GameState.OnReplayRunning -= Reset;
    }

    private void OnGameLost()
    {
        Freeze();
    }

    public void MoveBallToStartGame()
    {
        rigidBody2D.velocity = new Vector2(0f, -1f * _speed);
    }

    public Memento<BallState> CreateMemento()
    {
        BallState ballState = new BallState(transform.position, rigidBody2D.velocity);
        Memento<BallState> memento = new Memento<BallState>(ballState, Time.timeSinceLevelLoad);
        return memento;
    }

    public void SetState(BallState state)
    {
        transform.position = state.position;
        rigidBody2D.velocity = state.velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Only process collisions while in game
        if(Client.instance.gameState.state == GameStateEnum.ReplayRunning)
        {
            return;
        }

        Rigidbody2D colRigidBody = collision.gameObject.GetComponent<Rigidbody2D>();

        // verify this is a new collision
        if(colRigidBody && (Time.time - _lastCollisionTime > 0.2f))
        {
            Vector2 velocity = rigidBody2D.velocity;

            // set the velocity based on that of the colliding object
            velocity.x = (rigidBody2D.velocity.x * 0.5f) + (colRigidBody.velocity.x * 0.33f);
            int sign = velocity.y > 0 ? 1 : -1;
            // flip the sign to get the bounce correct
            velocity.y = _speed * -sign;

            rigidBody2D.velocity = velocity;

            _lastCollisionTime = Time.time;

            // Store a memento of this collision
            Memento<BallState> memento = CreateMemento();

            Client.instance.ballStateCaretaker.AddMemento(memento);
        }
    }
}
