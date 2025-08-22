using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class FaceToPlayer : Action
{
    private float baseScaleX;
    private EnemyMovement _enemyMovement;

    public override void OnAwake()
    {
        base.OnAwake();
        baseScaleX = transform.localScale.x;
        _enemyMovement = GetComponent<EnemyMovement>();
    }

    public override TaskStatus OnUpdate()
    {
        var scale = transform.localScale;
        scale.x = transform.position.x > _enemyMovement.PlayerTransform.position.x ? -baseScaleX : baseScaleX;
        transform.localScale = scale;
        return TaskStatus.Success;
    }
}
