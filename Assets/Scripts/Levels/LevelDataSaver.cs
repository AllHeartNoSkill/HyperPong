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

    private void Start()
    {
        _levelLoadedData.PlayerOnePath = _playerOnePath;
        _levelLoadedData.PlayerTwoPath = _playerTwoPath;
    }
}
