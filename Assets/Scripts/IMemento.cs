using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface to be used by state containers for mementos such as the InputState class
/// </summary>
public interface IMemento<T> {
    T GetState();
}
