using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerMovement _playerMovement;

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
    }

    public void Move(InputAction.CallbackContext context)
    {
        if(_playerMovement && !_playerMovement.isActiveAndEnabled) return;
        _playerMovement.MoveInput(context.ReadValue<float>());
    }
}
