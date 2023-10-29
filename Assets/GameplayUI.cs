using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayUI : MonoBehaviour
{
    [SerializeField] CardUIParent[] p1CardParents;
    [SerializeField] CardUIParent[] p2CardParents;

    private void Start() {
        InitCards();
    }

    private void InitCards(){
        foreach(CardUIParent card in p1CardParents){
            card.InitCard(0);
        }

        foreach(CardUIParent card in p2CardParents){
            card.InitCard(0);
        }
    }
}
