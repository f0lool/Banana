using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using YG;

public class SpawnerController : MonoBehaviour
{
    [SerializeField] private GameObject[] _prefabs;
    [SerializeField] private GameObject _spawnEnemyPrefab;

    [SerializeField] private List<Transform> spawnPoints;

    [SerializeField] private Transform _player;

    [SerializeField] private float _cooldownTime = 2f;
    private float _currentCooldownTime;

    private void Start()
    {
        _currentCooldownTime = _cooldownTime;
    }

    private void Update()
    {
        _currentCooldownTime -= Time.deltaTime;
        if( _currentCooldownTime < 0 )
        {
            if (_cooldownTime > 1.5f)
                _cooldownTime -= 0.1f;
            StartCoroutine(SpawnEnemy());
            _currentCooldownTime = _cooldownTime;
        }
    }


    private IEnumerator SpawnEnemy()
    {
        
        var spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];

        var pointSprite = Instantiate(_spawnEnemyPrefab, spawnPoint.position, spawnPoint.rotation);
        yield return new WaitForSeconds(1.5f);
        pointSprite.SetActive(false);
        var enemy = Instantiate(_prefabs[Random.Range(0, _prefabs.Length)], spawnPoint.position, spawnPoint.rotation);
        enemy.GetComponent<EnemyMovement>().SetPlayer(_player);
    }
}
