using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// Manages game state and sequence.
/// </summary>
public partial class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    /// <summary>
    /// Invoked when the game starts.
    /// </summary>
    public Action OnGameStart;

    /// <summary>
    /// Invoked when the game ends.
    /// </summary>
    public Action OnGameEnd;

    [Tooltip("Fighters list.")]
    [SerializeField]
    private List<Fighter> _fighters;

    /// <summary>
    /// Fighters list.
    /// </summary>
    public List<Fighter> Fighters => _fighters;

    private GameState _currentState;

    void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        foreach (var fighter in _fighters)
            fighter.OnDie += () => ChangeState(GameState.PostGame);
        
    }

    private void Start()
    {
        ChangeState(GameState.Playing);
    }

    public void ChangeState(GameState newState)
    {
        if (_currentState == newState) return;
        _currentState = newState;
        switch (_currentState)
        {
            case GameState.PreGame:
                break;
            case GameState.Playing:
                OnGameStart?.Invoke();
                break;
            case GameState.PostGame:
                EndGame();
                break;
        }
    }

    private void EndGame()
    {
        foreach (var fighter in _fighters)
            fighter.Freeze();
        
        OnGameEnd?.Invoke();
    }

}