using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for Originators
/// Originators CreateMementos of their internal state 
/// and allow for their state to be set from an external source
/// </summary>
public interface IOriginator<T>
{
    Memento<T> CreateMemento();

    void SetState(T state);
}
