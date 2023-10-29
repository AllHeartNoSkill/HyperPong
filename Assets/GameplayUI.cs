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

    [SerializeField] GameObject p1Cards;
    [SerializeField] GameObject p1Notif;

    [SerializeField] GameObject p2Cards;
    [SerializeField] GameObject p2Notif;

    bool isOtherPlayer = false;


    private void Start() {
        InitCards();
    }

    private void InitCards(){
        isOtherPlayer = false;
        foreach(CardUIParent card in p1CardParents){
            int index = Random.Range(0, 5);
            card.InitCard(index);
        }

        foreach(CardUIParent card in p2CardParents){
            int index = Random.Range(0, 5);
            card.InitCard(index);
        }
    }

    public void SelectCard(PlayerType player, bool active, PowerCard card){
        if(active){
            assignActive.TriggerEvent(player, card);
        }
        else{
            assignActive.TriggerEvent(player, card);
        }

        

        if (!isOtherPlayer){
            if(player == PlayerType.PlayerOne){
                p1Cards.SetActive(false);
                p1Notif.SetActive(true);
            }
            else{
                p2Cards.SetActive(false);
                p2Notif.SetActive(true);
            }
        }
        else{
            uIInputManager.betweenRoundsMenu.SetActive(false);
            startMatch.TriggerEvent();
        }
    }
}
