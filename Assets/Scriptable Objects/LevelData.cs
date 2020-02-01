using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "LevelData/LevelData", order = 1)]
public class LevelData : ScriptableObject
{
    public int levelIndex;
    public GameObject Player;
    public List<GameObject> Enemies;
}
