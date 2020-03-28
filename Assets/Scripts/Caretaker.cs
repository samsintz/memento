using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds a collection that contains all previous Mementoes of a given type. 
/// Can also store and retrieve Mementoes
/// </summary>
public class Caretaker<T> 
{
    private IOriginator<T> _originator;

    public IOriginator<T> originator
    {
        get { return _originator; }
    }
    
    public Caretaker(IOriginator<T> originator)
    {
        _originator = originator;
    }

    private Queue<Memento<T>> _mementos = new Queue<Memento<T>>();

    public void AddMemento(Memento<T> memento)
    {
        _mementos.Enqueue(memento);
    }

    public Memento<T> GetMemento()
    {
        if(_mementos.Count == 0)
        {
            return null;
        }

        Memento<T> memento = _mementos.Dequeue();
        return memento;
    }

    public Memento<T> PeekMemento()
    {
        if(_mementos.Count == 0)
        {
            return null;
        }

        Memento<T> memento = _mementos.Peek();
        return memento;
    }

    public int MementoCount()
    {
        return _mementos.Count;
    }
}
