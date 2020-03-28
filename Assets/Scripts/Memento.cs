using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains basic state storage and retrieval capabilities
/// This is the object that is stored
/// </summary>
public class Memento<T> : IMemento<T>
{
    private T _state;
    public float creationTime;

    public Memento(T state, float creationTime)
    {
        _state = state;
        this.creationTime = creationTime;
    }

    public T GetState()
    {
        return _state;
    }
}
