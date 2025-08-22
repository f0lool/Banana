using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
public class GorohAttack : Action
{
    public GameObject ProjectilePrefab;
    private Animator _animator;
    public int SpawnCount;
    public Vector2 OffsetSpawnProjectiles;
    private EnemyMovement _enemyMovement;

    public float Force = 15;

    public override void OnStart()
    {
        _animator = GetComponent<Animator>();
        _enemyMovement = GetComponent<EnemyMovement>();
        _enemyMovement.StopMovement();

        _animator.SetTrigger("IsAttack");

        var dir = _enemyMovement.PlayerTransform.position - transform.position;
        var bul = Object.Instantiate(ProjectilePrefab, (Vector2)transform.position + (OffsetSpawnProjectiles * transform.localScale.x), Quaternion.identity);
        bul.GetComponent<Rigidbody2D>().linearVelocity = dir.normalized * Force;
        Object.Destroy(bul, 1.3f);
    }
}
