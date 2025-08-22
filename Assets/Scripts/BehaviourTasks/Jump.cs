using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using System.Runtime.InteropServices;

public class Jump : Action
{
    public float horizontalForce = 5.0f;
    public float jumpForce = 10.0f;
    public Transform player;
    private Rigidbody2D body;

    private EnemyMovement _enemyMovement;

    public float builupTime;
    public float jumpTime;
    public float dashTime;

    public string animationTriggerName = "IsAttack";
    public bool shakeCameraOnLanding;

    private bool _hasLanded;

    private Animator _animator;

    private Tween buildupTween;
    private Tween jumpTween;

    public override void OnAwake()
    {
        _animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        _enemyMovement = GetComponent<EnemyMovement>();
    }

    public override void OnStart()
    {
        buildupTween = DOVirtual.DelayedCall(builupTime, StartJump, false);
        player = _enemyMovement.PlayerTransform;
    }

    private void StartJump()
    {
        _animator.SetTrigger(animationTriggerName);

        var direction = player.transform.position.x < transform.position.x ? -1 : 1;
        body.AddForce(new Vector2(horizontalForce * direction, jumpForce), ForceMode2D.Impulse);

        jumpTween = DOVirtual.DelayedCall(jumpTime, () =>
        {
            _hasLanded = true;
            
        }, false);
    }

    public override TaskStatus OnUpdate()
    {
        return _hasLanded ? TaskStatus.Success : TaskStatus.Running;
    }

    public override void OnEnd()
    {
        buildupTween?.Kill();
        jumpTween?.Kill();
        _hasLanded = false;
    }
}
