using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Datas/Level Loaded", fileName = "Level Loaded")]
public class LevelLoadedData : ScriptableObject
{
    public PathCreator PlayerOnePath;
    public PathCreator PlayerTwoPath;
    public BallSpawner BallSpawner;
    public BallMovement SpawnedBall;
    public float PlayerMoveSpeed;
    public float PlayerLength;
}
