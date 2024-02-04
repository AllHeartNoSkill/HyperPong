using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemRoot : Singleton<SystemRoot>
{
    private Dictionary<Type, MySystem> _systems = new Dictionary<Type, MySystem>();

    private Dictionary<PlayerType, PlayerMovement>
        _playerMovements = new Dictionary<PlayerType, PlayerMovement>();
    
    protected override void Awake()
    {
        base.Awake();
        MySystem[] systems = transform.GetComponentsInChildren<MySystem>();
        foreach (var system in systems)
        {
            _systems.Add(system.GetType(), system);
        }

        PlayerMovement[] players = transform.GetComponentsInChildren<PlayerMovement>();
        foreach (var player in players)
        {
            _playerMovements.Add(player.PlayerType, player);
        }
    }

    public bool SystemExist<T>() where T : MySystem
    {
        return _systems.ContainsKey(typeof(T));
    }

    public static bool TryGetSystem<T>(out T system) where T : MySystem
    {
        if (instance != null && instance.SystemExist<T>())
        {
            system = instance.GetSystem<T>();
            return true;
        }
        system = null;
        return false;
    }
    
    public T GetSystem<T>() where T : MySystem
    {
        return _systems[typeof(T)] as T;
    }

    public PlayerMovement GetPlayerMovement(PlayerType playerType)
    {
        return _playerMovements[playerType];
    }

    public PlayerPowerHandler GetPlayerPowerHandler(PlayerType playerType)
    {
        return _playerMovements[playerType].GetComponent<PlayerPowerHandler>();
    }
}
