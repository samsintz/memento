# The Memento Pattern

## TL;DR

The Memento pattern is a great tool for caching the state of an object. It makes it simple to switch between states and even store those states for later use. Using the Memento pattern can make a problem like undo in a text editor or instant replay in a video game relatively easy to manage, but it is not without its trade-offs. 

## Background

One of my all-time favorite games is Rocket League -- the game where you play soccer with cars that can jump and fly through the air like rockets. Rocket League has a cool feature where, after a goal is scored, you see an instant replay of the moments leading up to the goal. After spending enough time with this game I found myself trying to figure out how the developers built different features. One likely technique that comes to mind for creating an instant replay feature is the Memento Pattern. This post will explore the Memento design pattern and dive into the core elements of its use with instant replay.

When not playing Rocket League I have been working on becoming a better software engineer by learning about design patterns such as the Memento pattern. I think of software design patterns as tools in my toolbox as a developer. In the case of Rocket League's instant replay feature, the Memento pattern is a good tool for the job.

## The Pattern

The Memento pattern has many potential uses cases related to managing state and is commonly used to create undo functionality in applications like text editors. Being someone excited about game development and Rocket League, I opted to create a simple pong game and add instant replay to it rather than use the traditional undo project to learn the pattern. To properly apply the pattern we must first understand the components and how they relate to the problem we are trying to solve.

The Memento pattern has three main components:

1. Memento: an object that contains basic state storage and retrieval capabilities. This is like a snapshot of the state of an object or like a save point in a video game.
2. Originator: an object that creates Mementos of its internal state. The originator allows for its state to be set by an external source. The originator is the object we are interested in storing the state of.
3. Caretaker: an object that holds a collection of all previous mementos of a given type. It can also store and retrieve mementos.

At its core, the Memento pattern is useful for storing snapshots of the state of an object and providing access to past states on demand. All we would need to do to create an instant playback feature is simply store the different states of the game and run a simulation using those states at the end of the game.

## Instant Replay

The first step is identifying what state we need to save snapshots of to create an instant replay. We will focus on storing the inputs entered by a player as the state in the memento so that we can create a simulation of the game later. In order to make a good simulation, we will want to make sure that all gameplay elements are driven by the state we are storing as that is the only data we will have when running our replay. We will also need to ensure that our game is deterministic, meaning that the result of the simulation is the same when run multiple times when given the same inputs. Mindful use of random numbers is likely all we will have to worry about for this in our simple example. 

Below are simplified examples of each component of the Memento pattern as used with a Pong game. The state being captured (Mementos) are the keyboard inputs and the owner of that state is an Input Manager class (Originator).

```csharp
// Object to store Key Input state
public class Memento
{
    private float state;

    public Memento(float state)
    {
        this.state = state;
    }

    public float GetState()
    {
        return state;
    }
}
```

```csharp
// Input manager class that creates mementos of its internal state
public class Originator
{
    // internal state of the object -- horizontal axis (-1,1)
    private float input;

    public void SetInputState(float input)
    {
        this.input = input;
    }

    public Memento CreateMemento()
    {
        Memento memento = new Memento(this.input);
        return memento;
    }

    /*
    * Input management code
    */
}
```

```csharp
// Holds all Mementos and provides access to them
public class Caretaker
{
    // This example uses a queue, but a list or stack would
    // also work depending on your use case
    private Queue<Memento> mementos = new Queue<Memento>();

    public void AddMemento(Memento memento)
    {
        mementos.Enqueue(memento);
    }

    public Memento GetMemento()
    {
        if(mementos.Count == 0)
        {
            return null;
        }

        Memento memento = mementos.Dequeue();
        return memento;
    }
}
```

## Challenges

While the memento pattern can be very useful in the right situation it is not always the best option. Mementos are great for storing a singular state (the state of just one object). A basic example of instant replay with the memento pattern works fine with storing the singular state of the input manager, but a more sophisticated example would store other states such as the locations of collisions of the ball with walls and paddles. We would need to create different Mementos types for each state to track and potentially different Caretaker objects for each associated Memento type. We will end up creating a generic Memento in the example and will structure the code such that we only needed one Caretaker for all  Mementos. See the source for details.

Another thing to be mindful of is how often we save Mementos. While we will be checking every frame for new input in the game, we would not want to make a new Memento each frame to avoid using up more memory than we need. Instead, we will only store Mementos when there is a change of input. This also means that we will want to keep a timestamp of when the memento is created so that we can give the replay the correct timing.

## Example

[Web GL Build](https://simmer.io/@samsintz/~cd552792-7d7e-6caf-77b7-9c8713ebfda2)

This is a super bare-bones example. It doesn't have anything too flashy, but it does a good job of featuring the memento pattern. With that, we can dive into some code samples. 

### Memento

Taking the memento class from above, giving it an interface, and making it a bit more generic we end up with something like this the 

```c#
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
```

This is a generic memento that is capable of storing any state object. This memento also stores the time it was created since the start of the game so that we know when to process this memento when running our replay simulation. 

An example of the generic state we can store in a memento is the state of inputs in the game:

```c#
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
```

Another state we might want to store is that of the ball:

```c#
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
```

The Memento's `GetState()` method grants easy access to whatever the generic state is.

### Originator

Similar to the memento examples, we can build upon the originator to allow it to handle generic mementos.

First we create an interface outlining what we want the originator to do.

```c#
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
```

Next we create the concrete class implementing this interface.

```c#
/// <summary>
/// Concrete originator in this example
/// </summary>
public class InputManager : IOriginator<InputState>, IDisposable 
{
    private float _inputState;

        public void PollForInput()
    {
        // Only monobehaviors can start coroutines
        // See the github project for full source
        _inputPollCoroutine = Client.instance.StartCoroutine(PollforInputHelper());
    }

    private IEnumerator PollforInputHelper()
    {
        while(true)
        {
            float horizontal = -2f; //invalid value
            if(Input.GetKey(KeyCode.LeftArrow))
            {
                horizontal = -1f;
            }
            if(Input.GetKey(KeyCode.RightArrow))
            {
                horizontal = 1f;
            }
            
            if(Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
            {
                horizontal = 0f;
            }

            if(horizontal != -2f)
            {
                if(_inputState != horizontal)
                {
                    // new input value
                    _inputState = horizontal;
                    Client.instance.inputStateCaretaker.AddMemento(CreateMemento());
               }
            }
            // poll each frame
            yield return null;
        }
    }

    public float GetInput()
    {
        return _inputState;
    }

    public Memento<InputState> CreateMemento()
    {
        InputState inputState = new InputState(_inputState);
        Memento<InputState> memento = new Memento<InputState>(inputState, Time.timeSinceLevelLoad);
        return memento;
    }

    public void SetState(InputState mementoState)
    {
        _inputState = mementoState.state;
    }
}
```

This class just handles keyboard input and checks if the input is unique from the last saved input. If it is a new input, create a memento and store it. Rather than save a memento every frame or for every input we only save the new ones to reduce our memory footprint.

### Caretaker

Finally, we can expand upon the base caretaker example. Remember, the memento pattern is designed to handle a singular state for an object. For this reason, we need to create a separate caretaker for each originator. This example project stores the states of the ball and input, but luckily we can just create two instances of the same Caretaker class as it is generic.

```c#
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
```

This is pretty similar to the example above; all we really added were some convenience methods and support for generic Memento states.



Check out the full source for more details! 

## Closing Thoughts

The Memento pattern is really useful when used in the right scenario; it can simplify complex problems relatively well and potentially force you to embrace a new way of designing your projects. If you enjoyed the Memento pattern, consider checking out the Command pattern as well. It can also be used to manage state but works a bit better for multiple objects.
