using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;

public class PlayerInSight : Conditional
{
    public float detectDistance;
    private float _playerEnemyDistance;
    private EnemyMovement _enemyMovement;

    public override void OnAwake()
    {
        _enemyMovement = GetComponent<EnemyMovement>();
    }

    public bool CheckValid()
    {
        float playerEnemyDistanceAbs = Mathf.Abs(_playerEnemyDistance);
        return playerEnemyDistanceAbs > detectDistance;
    }

    public override TaskStatus OnUpdate()
    {
        _playerEnemyDistance = _enemyMovement.PlayerTransform.position.x - transform.position.x;

        if (!CheckValid())
        {
            _enemyMovement.StopMovement();
            return TaskStatus.Success;
        }
        else
            return TaskStatus.Failure;
    }
}
