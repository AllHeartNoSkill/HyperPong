using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.UI;

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
        
        PopulateCards(p1CardParents, _uiInputManager.P1EventSystem, PlayerType.PlayerOne);
        PopulateCards(p2CardParents, _uiInputManager.P2EventSystem, PlayerType.PlayerTwo);
    }

    private void PopulateCards(CardUIParent[] cardParents, MultiplayerEventSystem eventSystem, PlayerType playerType)
    {
        //TODO: Fix RNG, this is just a temp RNG
        List<PowerCard> playerPowerCards = _powerUpDatas
            .Except(SystemRoot.instance.GetPlayerPowerHandler(playerType).AllPowerCards).ToList();
        
        foreach(CardUIParent card in cardParents){
            if (playerPowerCards.Count == 0)
            {
                card.gameObject.SetActive(false);
                continue;
            }
            int index = Random.Range(0, playerPowerCards.Count);
            card.InitCard(playerPowerCards[index], playerType, eventSystem);
            playerPowerCards.RemoveAt(index);
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
