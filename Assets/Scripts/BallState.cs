using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// State container for the Ball.
/// Acts as a specific type stored in a memento
/// </summary>
public class BallState
{
    public Vector2 position;
    public Vector2 velocity;

    public BallState(Vector2 position, Vector2 velocity)
    {
        this.position = position;
        this.velocity = velocity;
    }
}
