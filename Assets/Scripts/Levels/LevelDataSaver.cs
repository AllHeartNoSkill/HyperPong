using System;
using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;

public class LevelDataSaver : MonoBehaviour
{
    [SerializeField] private LevelLoadedData _levelLoadedData;
    [SerializeField] private PathCreator _playerOnePath;
    [SerializeField] private PathCreator _playerTwoPath;
    [SerializeField] private float _playerMoveSpeed = 5f;
    [SerializeField] private float _playerLength = 1f;

    private void Start()
    {
        _levelLoadedData.PlayerOnePath = _playerOnePath;
        _levelLoadedData.PlayerTwoPath = _playerTwoPath;
        _levelLoadedData.PlayerMoveSpeed = _playerMoveSpeed;
        _levelLoadedData.PlayerLength = _playerLength;
    }
}
