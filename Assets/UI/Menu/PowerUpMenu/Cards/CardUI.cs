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
        sequence.ResetToInitialState();
        sequence.Play();
        // description.SetActive(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        sequence.PlayBackwards();
        // description.SetActive(false);
    }

    public void OnSubmit(BaseEventData eventData)
    {
        description.SetActive(false);
        cardParent.SelectCard();
    }

    public void AssignImage(Sprite sprite){
        cardImage.sprite = sprite;
    }
}