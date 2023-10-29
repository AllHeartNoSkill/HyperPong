using System.Collections;
using System.Collections.Generic;
using BrunoMikoski.AnimationSequencer;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardUI : MonoBehaviour, ISelectHandler, IDeselectHandler, ISubmitHandler// required interface when using the OnSelect method.
{
    [SerializeField] GameObject description;
    [SerializeField] CardUIParent cardParent;
    [SerializeField] private Image cardImage;

    [SerializeField] AnimationSequencerController sequence;

    public void OnSelect(BaseEventData eventData)
    {
        sequence.Play();
        description.SetActive(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        description.SetActive(false);
    }

    public void OnSubmit(BaseEventData eventData)
    {
        cardParent.SelectCard();
    }

    public void AssignImage(Sprite sprite){
        cardImage.sprite = sprite;
    }
}