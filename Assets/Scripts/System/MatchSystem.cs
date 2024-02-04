using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class MatchSystem : MySystem
{
    [Header("Match Data")]
    [SerializeField] private int _roundWinCount = 3;
    [SerializeField] private int _matchWinCount = 6;
    [SerializeField] private int _levelCount = 11;

    [Header("UI")] 
    [SerializeField] private Menu _powerUpSelecMenu;

    [Header("Game Events")] 
    [SerializeField] private GameEvent _levelLoadedEvent;
    [SerializeField] private GameEvent _matchReadyEvent;
    [SerializeField] private GameEvent _matchBeginEvent;
    [SerializeField] private GameEvent_PlayerType _matchEndEvent;
    [SerializeField] private GameEvent_Int _startRoundEvent;
    [SerializeField] private GameEvent_PlayerType _roundEndEvent;
    [SerializeField] private GameEvent _levelUnloadedEvent;

    private bool _isGameOnGoing = false;
    private bool _isMatchOnGoing = false;
    
    //game datas
    private int[] _playersMatchScore;
    private int _matchCount;
    
    //match datas
    private int[] _playersRoundScore;
    private int _currentRound;
    private int _currentLevel;
    private PlayerType _lastMatchWinner = PlayerType.Default;

    private LevelLoader _levelLoader;

    public PlayerType LastMatchWinner => _lastMatchWinner;

    private void Start()
    {
        _levelLoader = SystemRoot.instance.GetSystem<LevelLoader>();
    }

    private void ResetGameData()
    {
        _playersMatchScore = new int[2];
        _matchCount = 0;
        _lastMatchWinner = PlayerType.Default;
    }

    private void RestartMatchData()
    {
        _playersRoundScore = new int[2];
        _currentRound = 0;
    }

    private int GetLevel()
    {
        int nextLevel = Random.Range(2, _levelCount + 1);
        if (nextLevel == _currentLevel)
        {
            if(nextLevel != _levelCount)
            {
                nextLevel += 1;
            }
            else
            {
                nextLevel -= 1;
            }
        }
        return nextLevel;
    }

    public void StartGame(int startingLevel)
    {
        if(_isGameOnGoing) return;
        _isGameOnGoing = true;
        
        ResetGameData();
        PrepareMatch(startingLevel);
    }

    public void EndGame(PlayerType winner)
    {
        _isGameOnGoing = false;
        SceneManager.LoadScene(1);
    }

    private void PrepareMatch(int level)
    {
        if(_isMatchOnGoing) return;
        _isMatchOnGoing = true;
        _currentLevel = level;
        
        RestartMatchData();
        _levelLoader.LoadLevel(level, _matchCount == 0);
        _levelLoadedEvent.AddListener(OnLevelLoaded);
    }

    private void OnLevelLoaded()
    {
        _levelLoadedEvent.RemoveListener(OnLevelLoaded);
        
        UIRoot.instance.OpenMenu(_powerUpSelecMenu);
        _matchReadyEvent.AddListener(OnMatchReady);
    }

    private void OnMatchReady()
    {
        _matchReadyEvent.RemoveListener(OnMatchReady);
        StartMatch();
    }

    private void StartMatch()
    {
        _matchBeginEvent.TriggerEvent();
        StartCoroutine(CountdownToStartRound(3));
    }

    private void MatchEnd(PlayerType winner)
    {
        Debug.Log($"match ended");
        _matchEndEvent.TriggerEvent(winner);
        _isMatchOnGoing = false;
        _matchCount += 1;
        
        _lastMatchWinner = winner;
        _playersMatchScore[(int)winner] += 1;
        if (_playersMatchScore[(int)winner] >= _matchWinCount)
        {
            Debug.Log($"match ended and game too");
            _levelLoader.UnloadLevel(_currentLevel);
            EndGame(winner);
            return;
        }
        
        Debug.Log($"unload level");
        _levelLoader.UnloadLevel(_currentLevel);
        _levelUnloadedEvent.AddListener(OnLevelUnloaded);
    }

    private void OnLevelUnloaded()
    {
        Debug.Log($"level unloaded");
        _levelUnloadedEvent.RemoveListener(OnLevelUnloaded);
        PrepareMatch(GetLevel());
    }

    IEnumerator CountdownToStartRound(int second)
    {
        for (int i = second; i > 0; i--)
        {
            Debug.Log($"Round Start In.. {i}");
            yield return new WaitForSeconds(1f);
        }
        StartRound();
    }

    private void StartRound()
    {
        _startRoundEvent.TriggerEvent(_currentRound);

        _currentRound += 1;
        _roundEndEvent.AddListener(RoundEnd);
    }

    private void RoundEnd(PlayerType roundWinner)
    {
        _roundEndEvent.RemoveListener(RoundEnd);

        _playersRoundScore[(int)roundWinner] += 1;
        if (_playersRoundScore[(int)roundWinner] >= _roundWinCount)
        {
            MatchEnd(roundWinner);
            return;
        }
        
        StartCoroutine(CountdownToStartRound(1));
    }
}
