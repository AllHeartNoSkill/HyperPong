using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemRoot : Singleton<SystemRoot>
{
    private Dictionary<Type, MySystem> _systems = new Dictionary<Type, MySystem>();
    
    protected override void Awake()
    {
        base.Awake();
        MySystem[] systems = transform.GetComponentsInChildren<MySystem>();
        foreach (var system in systems)
        {
            _systems.Add(system.GetType(), system);
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
}
