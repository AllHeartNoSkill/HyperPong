using System;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "SO/Gameplay/Create Power Card", fileName = "New Power Card")]
public class PowerCard : ScriptableObject
{
    public string id;
    public Sprite cardSprite;
    public string activeDescription;
    public float duration;
    public float chargeTime;
    public string passiveDescription;
    public int type;
    public PowerUpClass PowerUpPrefab;
}
