using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Central game controller holding refrences to different components
/// </summary>
public class Client : MonoBehaviour 
{
    public static Client instance;

    [SerializeField]
    private GameObject _playerPaddlePrefab;

    [SerializeField]
    private GameObject _aiPaddlePrefab;

    [SerializeField]
    private GameObject _ballPrefab;

    [SerializeField]
    private Vector2 _playerPaddleSpawnPosition;
    public GameState gameState;

    [SerializeField]
    private DifficultyManager _difficultyManager;
    public DifficultyManager difficultyManager
    {
        get { return _difficultyManager; }
    }

    private Player _player;
    public Player player
    {
        get { return _player; }
    }
    private AIPaddle _aiPaddle;
    public AIPaddle aiPaddle
    {
        get { return _aiPaddle; }
    }
    private Ball _ball;
    public Ball ball
    {
        get { return _ball; }
    }
    private SimulationManager<InputState> _inputStateSimulationManager;
    public SimulationManager <InputState> inputStateSimulationManager
    {
        get { return _inputStateSimulationManager; }
    }

    private SimulationManager<BallState> _ballStateSimulationManager;
    public SimulationManager <BallState> ballStateSimulationManager
    {
        get { return _ballStateSimulationManager; }
    }

    private Caretaker<InputState> _inputStateCaretaker;
    public Caretaker<InputState> inputStateCaretaker
    {
        get { return _inputStateCaretaker; }
    }

    private Caretaker<BallState> _ballStateCaretaker;
    public Caretaker<BallState> ballStateCaretaker
    {
        get { return _ballStateCaretaker; }
    }

    private InputManager _inputManager;
    public InputManager inputManager
    {
        get { return _inputManager; } 
    }

    private void Awake() {
        if (instance == null)
        {
            instance = this;
        }
        gameState = new GameState();
    }

    private void Start()
    {
        _ball = Instantiate(_ballPrefab, Vector3.zero, Quaternion.identity).GetComponent<Ball>();
        _aiPaddle = Instantiate(_aiPaddlePrefab, new Vector3(_playerPaddleSpawnPosition.x, -_playerPaddleSpawnPosition.y), Quaternion.identity).GetComponent<AIPaddle>();
        _player = Instantiate(_playerPaddlePrefab, new Vector3(_playerPaddleSpawnPosition.x, _playerPaddleSpawnPosition.y), Quaternion.identity).GetComponent<Player>();

        _aiPaddle.SetBallTarget(_ball.transform);
        _inputManager = new InputManager();

        _inputStateCaretaker = new Caretaker<InputState>(_inputManager);
        _ballStateCaretaker = new Caretaker<BallState>(_ball);

        _inputStateSimulationManager = new SimulationManager<InputState>(_inputStateCaretaker);
        _ballStateSimulationManager = new SimulationManager<BallState>(_ballStateCaretaker);

        gameState.state = GameStateEnum.InGame;
    }

    public void OnDestroy()
    {
        _inputManager.Dispose();
        _inputManager = null;

        _inputStateSimulationManager.Dispose();
        _inputStateSimulationManager = null;

        _ballStateSimulationManager.Dispose();
        _ballStateSimulationManager = null;
    }

    public void Timer(float time, System.Action callback)
    {
        StartCoroutine(TimerCoroutine(time, callback));
    }

    private IEnumerator TimerCoroutine(float time, System.Action callback)
    {
        yield return new WaitForSeconds(time);
        callback();
    }
}
