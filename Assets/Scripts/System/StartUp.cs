using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUp : MonoBehaviour
{
    [SerializeField] private Menu _mainMenu;
    private void Start()
    {
        OpenMenu();
    }

    private void OpenMenu()
    {
        UIRoot.instance.OpenMenu(_mainMenu);
    }
}
