using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MatchSystem : Singleton<MatchSystem>
{
    [Header("Test Param")]
    [SerializeField] private bool _playOnAwake = true;
    [SerializeField] private int _testLevel = 1;

    [Header("Game Events")] 
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

    private void Start()
    {
        if (_playOnAwake)
        {
            StartGame(_testLevel);
        }
    }

    private void ResetGameData()
    {
        _playersMatchScore = new int[2];
        _matchCount = 0;
    }

    private void RestartMatchData()
    {
        _playersRoundScore = new int[2];
        _currentRound = 0;
        
    }

    private int GetLevel()
    {
        return _currentLevel + 1;
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
        
    }

    private void PrepareMatch(int level)
    {
        if(_isMatchOnGoing) return;
        _isMatchOnGoing = true;
        _currentLevel = level;
        
        RestartMatchData();
        LevelLoader.instance.LoadLevel(level, _matchCount == 0);
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
        
        _playersMatchScore[(int)winner] += 1;
        if (_playersMatchScore[(int)winner] >= 3)
        {
            Debug.Log($"match ended and game too");
            EndGame(winner);
            return;
        }
        
        Debug.Log($"unload level");
        LevelLoader.instance.UnloadLevel(_currentLevel);
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
        if (_playersRoundScore[(int)roundWinner] >= 3)
        {
            MatchEnd(roundWinner);
            return;
        }
        
        StartCoroutine(CountdownToStartRound(1));
    }
}
