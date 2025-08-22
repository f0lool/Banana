using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class EnemyChase : Action
{
    private int _reachEdge;
    private float _playerEnemyDistance;

    private EnemyMovement _enemyMovement;

    public override void OnStart()
    {
        _enemyMovement = GetComponent<EnemyMovement>();
    }

    public override TaskStatus OnUpdate()
    {
        _playerEnemyDistance = _enemyMovement.PlayerTransform.position.x - transform.position.x;

        float dist = _playerEnemyDistance;

        _enemyMovement.Walk(Mathf.Abs(dist) < 1.5f ? 0 : dist, 1.5f);

        if(Mathf.Abs(_playerEnemyDistance) > 10f)
        {
            return TaskStatus.Failure;
        }
        return TaskStatus.Running;
    }
}
