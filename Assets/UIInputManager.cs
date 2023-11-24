using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class UIInputManager : MonoBehaviour
{
    PlayerInput p1PlayerInput;
    PlayerInput p2PlayerInput;

    [SerializeField] InputSystemUIInputModule  p1InputModule;
    [SerializeField] MultiplayerEventSystem p1EventSystem;
    [SerializeField] InputSystemUIInputModule  p2InputModule;
    [SerializeField] MultiplayerEventSystem p2EventSystem;

    [SerializeField] public GameObject mainMenu;
    [SerializeField] Canvas mainMenuCanvas;
    [SerializeField] Button mainMenuFirstSelected;

    [SerializeField] public GameObject betweenRoundsMenu;
    [SerializeField] Canvas p1RoundCanvas;
    [SerializeField] Button p1RoundCanvasFirstSelected;
    [SerializeField] Canvas p2RoundCanvas;
    [SerializeField] Button p2RoundCanvasFirstSelected;
    
    [SerializeField] private GameEvent _levelLoadedEvent;
    [SerializeField] private GameEvent_PlayerType _matchEndEvent;
    [SerializeField] private GameEvent _matchReadyEvent;

    [SerializeField] GameplayUI gameplayUI;
    private bool firstPlay = true;

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

    void SetMainMenuInputs(){
        p1EventSystem.playerRoot = mainMenuCanvas.gameObject;
        p1EventSystem.SetSelectedGameObject(mainMenuFirstSelected.gameObject);
    }

    void SetRoundsMenuInputs(){
        p1EventSystem.playerRoot = p1RoundCanvas.gameObject;
        p2EventSystem.playerRoot = p2RoundCanvas.gameObject;
        p1EventSystem.SetSelectedGameObject(p1RoundCanvasFirstSelected.gameObject);
        p2EventSystem.SetSelectedGameObject(p2RoundCanvasFirstSelected.gameObject);
    }

    public void SetUpGame(){
        MatchSystem.instance.StartGame(1);
        _levelLoadedEvent.AddListener(OnLevelLoaded);
        _matchEndEvent.AddListener(OnMatchEnd);
    }

    private void ShowPowerUpSelectionMenu()
    {
        mainMenuCanvas.gameObject.SetActive(false);
        betweenRoundsMenu.gameObject.SetActive(true);
        SetRoundsMenuInputs();
    }

    private void OnMatchEnd(PlayerType obj)
    {
        Debug.Log("Bruh");
        _matchEndEvent.RemoveListener(OnMatchEnd);
    }

    private void OnLevelLoaded()
    {
        // _levelLoadedEvent.RemoveListener(OnLevelLoaded);
        if(firstPlay){
            ShowPowerUpSelectionMenu();
            firstPlay = false;
        }
        else{
            _matchReadyEvent.TriggerEvent();
        }
    }
}
