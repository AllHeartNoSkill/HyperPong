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

    [SerializeField] int powerCardId;
    public PowerCard powerCard;
    
    public void SelectCard(){
        confirmationUI.SetActive(true);
        cardUI.gameObject.SetActive(false);
        eventSystem.SetSelectedGameObject(confirmButton);
    }
}
