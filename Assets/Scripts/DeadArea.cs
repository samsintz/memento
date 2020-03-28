using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Collider for the level end bounds behind each paddle.
/// Handles transitioning ot the 'Lost' gameState
/// </summary>
public class DeadArea : MonoBehaviour 
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<Ball>())
        {
            Client.instance.gameState.state = GameStateEnum.Lost;
        }
    }
}
