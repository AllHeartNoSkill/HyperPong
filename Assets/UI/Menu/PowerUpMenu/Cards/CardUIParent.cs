using UnityEngine;
using UnityEngine.InputSystem.UI;
using TMPro;

public class CardUIParent : MonoBehaviour
{
    [SerializeField] CardUI cardUI;
    [SerializeField] GameObject confirmationUI;
    [SerializeField] GameObject confirmButton;
    [SerializeField] PowerUpSelectMenu gUI;

    [SerializeField] TMP_Text titleText;
    [SerializeField] TMP_Text activeText;
    [SerializeField] TMP_Text passiveText;

    private MultiplayerEventSystem _eventSystem;
    private PowerCard _powerCard;
    private PlayerType _player;

    public void InitCard(PowerCard powerUpData, PlayerType player, MultiplayerEventSystem eventSystem) {
        _powerCard = powerUpData;
        _eventSystem = eventSystem;
        _player = player;
        
        cardUI.AssignImage(_powerCard.cardSprite);
        titleText.text = _powerCard.id;
        activeText.text = _powerCard.activeDescription;
        passiveText.text = _powerCard.passiveDescription;
    }
    
    public void SelectCard(){
        confirmationUI.SetActive(true);
        cardUI.gameObject.SetActive(false);
        _eventSystem.SetSelectedGameObject(confirmButton);
    }

    public void AssignPower(bool active){
        gUI.SelectCard(_player, active, _powerCard);
    }
}
