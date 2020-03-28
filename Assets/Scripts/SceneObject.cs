using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for all player-facing game objects in the scene
/// </summary>
public class SceneObject : MonoBehaviour 
{
    protected Rigidbody2D rigidBody2D;

    protected Vector2 _position;

    protected void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        _position = transform.position;
    }

    public virtual void Freeze()
    {
        if(rigidBody2D)
        {
            rigidBody2D.velocity = Vector2.zero;
        }
    }

    public virtual void Reset()
    {
        transform.position = _position;
    }
}
