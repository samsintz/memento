using UnityEngine.UI;
using UnityEngine;

/// <summary>
/// Central manager for all game UI
/// </summary>
public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _gameOverText;

    [SerializeField]
    private Text _replayText;

    [SerializeField]
    private Text _startText;

    private void Start()
    {
        GameState.OnPreGame += ShowPreGame;
        GameState.OnInGame += ShowInGame;
        GameState.OnLost += ShowLost;
        GameState.OnReplayRunning += ShowReplay;
    }

    private void OnDestroy()
    {
        GameState.OnPreGame -= ShowPreGame;
        GameState.OnInGame -= ShowInGame;
        GameState.OnLost -= ShowLost;
        GameState.OnReplayRunning -= ShowReplay;
    }

    private void ShowLost()
    {
        _gameOverText.gameObject.SetActive(true);
    }

    private void ShowReplay()
    {
        _gameOverText.gameObject.SetActive(false);
        _replayText.gameObject.SetActive(true);
    }

    private void ShowPreGame()
    {
        _replayText.gameObject.SetActive(false);
        _gameOverText.gameObject.SetActive(false);
        _startText.gameObject.SetActive(true);
    }

    private void ShowInGame()
    {
        _startText.gameObject.SetActive(false);
    }
}
