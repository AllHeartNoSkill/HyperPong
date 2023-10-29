using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class UIInputManager : MonoBehaviour
{
    PlayerInput p1PlayerInput;
    PlayerInput p2PlayerInput;

    [SerializeField] InputSystemUIInputModule  p1InputModule;
    [SerializeField] MultiplayerEventSystem p1EventSystem;
    [SerializeField] InputSystemUIInputModule  p2InputModule;
    [SerializeField] MultiplayerEventSystem p2EventSystem;

    [SerializeField] GameObject mainMenu;
    [SerializeField] Canvas mainMenuCanvas;
    [SerializeField] Button mainMenuFirstSelected;

    [SerializeField] GameObject betweenRoundsMenu;
    [SerializeField] Canvas p1RoundCanvas;
    [SerializeField] Button p1RoundCanvasFirstSelected;
    [SerializeField] Canvas p2RoundCanvas;
    [SerializeField] Button p2RoundCanvasFirstSelected;

    void Start()
    {
        StartCoroutine(waitForScenes());
    }

    IEnumerator waitForScenes(){
        yield return new WaitForSeconds(2);
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
        mainMenuCanvas.gameObject.SetActive(false);
        betweenRoundsMenu.gameObject.SetActive(true);
        SetRoundsMenuInputs();
    }
}
