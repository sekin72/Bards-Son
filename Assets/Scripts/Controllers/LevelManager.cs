using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelData _levelData;
    [SerializeField] private Player _player;
    [SerializeField] private List<Enemy> _enemies;

    private void Start()
    {
        _player = _levelData.Player.GetComponent<Player>();
        //Instantiate(_player);

        _enemies = new List<Enemy>();
        for (int i = 0; i < _levelData.Enemies.Count; i++)
        {
            _enemies.Add(_levelData.Enemies[i].GetComponent<Enemy>());
            Instantiate(_enemies[_enemies.Count - 1]);
        }
    }
}
