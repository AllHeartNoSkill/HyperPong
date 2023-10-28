using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardUI : MonoBehaviour, ISelectHandler, IDeselectHandler, ISubmitHandler// required interface when using the OnSelect method.
{
    [SerializeField] GameObject description;
    [SerializeField] CardUIParent cardParent;

    public void OnSelect(BaseEventData eventData)
    {
        description.SetActive(true);
        
    }

    public void OnDeselect(BaseEventData eventData)
    {
        description.SetActive(false);
        print("deselected" + gameObject.name);
    }

    public void OnSubmit(BaseEventData eventData)
    {
        cardParent.SelectCard();
    }
}