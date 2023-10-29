using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;

public class CardUIParent : MonoBehaviour
{
    [SerializeField] CardUI cardUI;
    [SerializeField] GameObject confirmationUI;
    [SerializeField] GameObject confirmButton;
    [SerializeField] MultiplayerEventSystem eventSystem;

    // [SerializeField] int powerCardId;
    public PowerCard powerCard;

    public void InitCard(int id) {
        powerCard = PowerReference.instance.powerCards[id];
        cardUI.AssignImage(powerCard.cardSprite);
    }
    
    public void SelectCard(){
        confirmationUI.SetActive(true);
        cardUI.gameObject.SetActive(false);
        eventSystem.SetSelectedGameObject(confirmButton);
    }
}
