using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStack : MonoBehaviour
{
    private List<Menu> _stack = new List<Menu>();

    public bool DoPush(Menu menu)
    {
        if (_stack.Contains(menu))
        {
            Debug.LogWarning("Trying to open menu that already open");
            return false;
        }
        
        Menu spawnedMenu = Instantiate(menu, Vector3.zero, Quaternion.identity, transform);
        spawnedMenu.CurrentStack = this;
        _stack.Add(spawnedMenu);
        
        spawnedMenu.PrePush();
        return true;
    }

    public void DoPopMenu(Menu menu)
    {
        if (!_stack.Contains(menu))
        {
            Debug.LogWarning("Trying to close menu that not open");
            return;
        }

        _stack.Remove(menu);
        Destroy(menu.gameObject);
    }
}
