using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class Patrol : Action
{
    private int _reachEdge;

    private EnemyMovement _enemyMovement;

    public override void OnStart()
    {
        _enemyMovement = GetComponent<EnemyMovement>();
    }

    public override TaskStatus OnUpdate()
    {
        var canMove = true;
        var canMovePlatform = true;
        Vector2 detectOffset;
        detectOffset.x = _enemyMovement.EdgeSafeDistance * transform.localScale.x;
        detectOffset.y = 0;
        _reachEdge = _enemyMovement.CheckGrounded(detectOffset) ? 0 : (transform.localScale.x > 0 ? 1 : -1);
        canMove = !_enemyMovement.ChechWall();
        canMovePlatform = _enemyMovement.CheckGround();

        _enemyMovement.Walk(transform.localScale.x * _enemyMovement.WalkSpeed, 1.5f);

        if (!canMove || !canMovePlatform)
        {
            _enemyMovement.StopMovement();
            var newScale = transform.localScale;
            newScale.x *= -1;
            transform.localScale = newScale;
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Running;
        }
    }
}
