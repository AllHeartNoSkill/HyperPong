using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIInputManager : MonoBehaviour
{
    PlayerInput p1PlayerInput;
    PlayerInput p2PlayerInput;

    [SerializeField] InputSystemUIInputModule  p1InputModule;
    [SerializeField] MultiplayerEventSystem p1EventSystem;
    [SerializeField] InputSystemUIInputModule  p2InputModule;
    [SerializeField] MultiplayerEventSystem p2EventSystem;
    
    [SerializeField] private GameEvent _levelLoadedEvent;
    [SerializeField] private GameEvent_PlayerType _matchEndEvent;
    [SerializeField] private GameEvent _matchReadyEvent;

    [SerializeField] PowerUpSelectMenu powerUpSelectMenu;
    private bool firstPlay = true;

    public MultiplayerEventSystem P1EventSystem => p1EventSystem;
    public MultiplayerEventSystem P2EventSystem => p2EventSystem;

    void Start()
    {
        AssignPlayerInputs();
        SetMainMenuInputs();
    }

    void AssignPlayerInputs(){
        p1PlayerInput = GameObject.FindGameObjectWithTag("Player1").GetComponent<PlayerInput>();
        p2PlayerInput = GameObject.FindGameObjectWithTag("Player2").GetComponent<PlayerInput>();
        p1PlayerInput.uiInputModule = p1InputModule;
        p2PlayerInput.uiInputModule = p2InputModule;
    }

    public void SetPlayerOneInput(GameObject playerRoot, GameObject firstSelected)
    {
        p1EventSystem.playerRoot = playerRoot;
        p1EventSystem.SetSelectedGameObject(firstSelected);
    }

    public void SetPlayerTwoInput(GameObject playerRoot, GameObject firstSelected)
    {
        p2EventSystem.playerRoot = playerRoot;
        p2EventSystem.SetSelectedGameObject(firstSelected);
    }

    void SetMainMenuInputs(){
        // p1EventSystem.playerRoot = mainMenuCanvas.gameObject;
        // p1EventSystem.SetSelectedGameObject(mainMenuFirstSelected.gameObject);
    }

    void SetRoundsMenuInputs(){
        // p1EventSystem.playerRoot = p1RoundCanvas.gameObject;
        // p2EventSystem.playerRoot = p2RoundCanvas.gameObject;
        // p1EventSystem.SetSelectedGameObject(p1RoundCanvasFirstSelected.gameObject);
        // p2EventSystem.SetSelectedGameObject(p2RoundCanvasFirstSelected.gameObject);
    }

    public void SetUpGame(){
        // MatchSystem.instance.StartGame(1);
        // _levelLoadedEvent.AddListener(OnLevelLoaded);
        // _matchEndEvent.AddListener(OnMatchEnd);
    }

    private void ShowPowerUpSelectionMenu()
    {
        // mainMenuCanvas.gameObject.SetActive(false);
        // betweenRoundsMenu.gameObject.SetActive(true);
        // SetRoundsMenuInputs();
    }

    private void OnMatchEnd(PlayerType obj)
    {
        // _matchEndEvent.RemoveListener(OnMatchEnd);
    }

    private void OnLevelLoaded()
    {
        if(firstPlay){
            ShowPowerUpSelectionMenu();
            firstPlay = false;
        }
        else{
            _matchReadyEvent.TriggerEvent();
        }
    }
}
