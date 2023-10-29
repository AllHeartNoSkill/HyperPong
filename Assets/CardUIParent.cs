using System.Collections;
using System.Collections.Generic;
using BrunoMikoski.AnimationSequencer;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using BrunoMikoski.AnimationSequencer;
using TMPro;

public class CardUIParent : MonoBehaviour
{
    [SerializeField] CardUI cardUI;
    [SerializeField] GameObject confirmationUI;
    [SerializeField] GameObject confirmButton;
    [SerializeField] MultiplayerEventSystem eventSystem;
    [SerializeField] GameplayUI gUI;

    [SerializeField] PlayerType player;

    [SerializeField] TMP_Text titleText;
    [SerializeField] TMP_Text activeText;
    [SerializeField] TMP_Text passiveText;

    // [SerializeField] int powerCardId;
    public PowerCard powerCard;

    public void InitCard(int id) {
        powerCard = PowerReference.instance.powerCards[id];
        cardUI.AssignImage(powerCard.cardSprite);
        titleText.text = powerCard.id;
        activeText.text = powerCard.activeDescription;
        passiveText.text = powerCard.passiveDescription;
    }
    
    public void SelectCard(){
        confirmationUI.SetActive(true);
        cardUI.gameObject.SetActive(false);
        eventSystem.SetSelectedGameObject(confirmButton);
    }

    public void AssignPower(bool active){
        gUI.SelectCard(player, active, powerCard);
    }
}
