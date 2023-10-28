using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CardUI : MonoBehaviour, ISelectHandler, IDeselectHandler// required interface when using the OnSelect method.
{
    [SerializeField] GameObject description;



    public void OnSelect(BaseEventData eventData)
    {
        description.SetActive(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        description.SetActive(false);
        print("deselected" + gameObject.name);
    }
}