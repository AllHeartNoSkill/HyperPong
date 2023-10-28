using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameEvent _matchBeginEvent;
    [SerializeField] private GameEvent_PlayerType _matchEndEvent;

    private bool _canMove = false;
    private PlayerMovement _playerMovement;

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
    }

    public void Move(InputAction.CallbackContext context)
    {
        if(!_canMove) return;
        if(_playerMovement && !_playerMovement.isActiveAndEnabled) return;
        _playerMovement.MoveInput(context.ReadValue<float>());
    }

    private void OnMatchStart()
    {
        _canMove = true;
    }

    private void OnMatchEnd(PlayerType winner)
    {
        _canMove = false;
    }

    private void OnEnable()
    {
        _matchBeginEvent.AddListener(OnMatchStart);
        _matchEndEvent.AddListener(OnMatchEnd);
    }

    private void OnDisable()
    {
        _matchBeginEvent.RemoveListener(OnMatchStart);
        _matchEndEvent.RemoveListener(OnMatchEnd);
    }
}
