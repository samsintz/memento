using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// State container for the Input manager.
/// Acts as a specific type to be stored in a memento
/// </summary>
public class InputState
{
    public float state = 0f;

    public InputState(float state)
    {
        this.state = state;
    }
}
