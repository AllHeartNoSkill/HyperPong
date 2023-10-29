using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayUI : MonoBehaviour
{
    [SerializeField] CardUIParent[] p1CardParents;
    [SerializeField] CardUIParent[] p2CardParents;
    [SerializeField] GameEvent_PlayerType_PowerCard assignActive;
    [SerializeField] GameEvent_PlayerType_PowerCard assignPassive;
    [SerializeField] GameEvent startMatch;

    [SerializeField] UIInputManager uIInputManager;

    bool isOtherPlayer = false;


    private void Start() {
        InitCards();
    }

    private void InitCards(){
        isOtherPlayer = false;
        foreach(CardUIParent card in p1CardParents){
            card.InitCard(0);
        }

        foreach(CardUIParent card in p2CardParents){
            card.InitCard(0);
        }
    }

    public void SelectCard(PlayerType player, bool active, PowerCard card){
        if(active){
            assignActive.TriggerEvent(player, card);
        }
        else{
            assignActive.TriggerEvent(player, card);
        }

        if (!isOtherPlayer) isOtherPlayer = true;
        else{
            uIInputManager.betweenRoundsMenu.SetActive(false);
            startMatch.TriggerEvent();
        }
    }
}
