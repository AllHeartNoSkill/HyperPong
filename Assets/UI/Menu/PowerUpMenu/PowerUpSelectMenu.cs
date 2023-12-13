using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PowerUpSelectMenu : Menu
{
    [Header("Card Data")] 
    [SerializeField] private List<PowerCard> _powerUpDatas = new List<PowerCard>();
    
    [Header("GameEvents")]
    [SerializeField] GameEvent_PlayerType_PowerCard assignActive;
    [SerializeField] GameEvent_PlayerType_PowerCard assignPassive;
    [SerializeField] GameEvent startMatch;

    [Header("Player One")]
    [SerializeField] CardUIParent[] p1CardParents;
    [SerializeField] private GameObject p1Canvas;
    [SerializeField] GameObject p1Cards;
    [SerializeField] GameObject p1Notif;

    [Header("Player Two")]
    [SerializeField] CardUIParent[] p2CardParents;
    [SerializeField] private GameObject p2Canvas;
    [SerializeField] GameObject p2Cards;
    [SerializeField] GameObject p2Notif;

    private int _playerChoose = 0;
    private int _playerCount = 0;

    public override void PrePush()
    {
        base.PrePush();
        InitCards();

        PlayerType lastRoundWinner = SystemRoot.instance.GetSystem<MatchSystem>().LastMatchWinner;
        if (lastRoundWinner == PlayerType.PlayerOne)
        {
            p2Canvas.SetActive(true);
            _playerCount = 1;
        }
        else if (lastRoundWinner == PlayerType.PlayerTwo)
        {
            p1Canvas.SetActive(true);
            _playerCount = 1;
        }
        else
        {
            p1Canvas.SetActive(true);
            p2Canvas.SetActive(true);
            _playerCount = 2;
        }
        
        _uiInputManager.SetPlayerOneInput(p1Canvas, p1CardParents[0].transform.GetChild(2).gameObject);
        _uiInputManager.SetPlayerTwoInput(p2Canvas, p2CardParents[0].transform.GetChild(2).gameObject);
    }

    private void InitCards()
    {
        _playerChoose = 0;
        //TODO: Fix RNG, this is just a temp RNG

        // List<PowerCard> playerPowerCards = _powerUpDatas.Except().ToList();
        foreach(CardUIParent card in p1CardParents){
            int index = Random.Range(0, _powerUpDatas.Count);
            card.InitCard(_powerUpDatas[index], PlayerType.PlayerOne, _uiInputManager.P1EventSystem);
        }

        foreach(CardUIParent card in p2CardParents){
            int index = Random.Range(0, _powerUpDatas.Count);
            card.InitCard(_powerUpDatas[index], PlayerType.PlayerTwo, _uiInputManager.P2EventSystem);
        }
    }

    public void SelectCard(PlayerType player, bool active, PowerCard card){
        if(active){
            assignActive.TriggerEvent(player, card);
        }
        else{
            assignPassive.TriggerEvent(player, card);
        }
        
        if(player == PlayerType.PlayerOne){
            p1Cards.SetActive(false);
            p1Notif.SetActive(true);
        }
        else{
            p2Cards.SetActive(false);
            p2Notif.SetActive(true);
        }
        _playerChoose += 1;
        
        if (_playerChoose >= _playerCount){
            // uIInputManager.betweenRoundsMenu.SetActive(false);
            startMatch.TriggerEvent();
            Kill();
        }
    }
}
