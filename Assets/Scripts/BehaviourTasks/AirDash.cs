using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;

public class AirDash : Action
{
    public float dashForce = 5.0f;
    public float jumpForce = 10.0f;
    public Transform player;
    private Rigidbody2D body;

    private EnemyMovement _enemyMovement;

    public float buildupTime;
    public float jumpTime;
    public float dashTime;

    public string animationTriggerName = "IsAttack";
    public bool shakeCameraOnLanding;

    private bool _hasLanded;
    private float defaultGravity;
    private Quaternion defaultRotation;

    private Tween buildupTween;
    private Tween jumpTween;
    private Tween dashTween;

    private Animator _animator;
    public override void OnAwake()
    {
        _animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        _enemyMovement = GetComponent<EnemyMovement>();
        player = _enemyMovement.PlayerTransform;
    }

    public override void OnStart()
    {
        defaultGravity = body.gravityScale;
        defaultRotation = body.transform.rotation;
        body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        jumpTween = DOVirtual.DelayedCall(jumpTime, StartBuildup, false);
    }

    private void StartBuildup()
    {
        var newRotation = new Vector3(defaultRotation.x, defaultRotation.y, 45 * body.transform.localScale.normalized.x);
        
        body.gravityScale = 0;
        body.linearVelocity = Vector2.zero;
        body.transform.DORotate(newRotation, buildupTime);
        _animator.SetTrigger(animationTriggerName);
        buildupTween = DOVirtual.DelayedCall(buildupTime, StartDash, false);
    }

    private void StartDash()
    {
        body.gravityScale = defaultGravity;

        var direction = transform.localScale.normalized.x;
        var distance = Mathf.Abs(transform.position.x - player.transform.position.x);
        var downwardsDirection = distance < 4f ? -1 : -0.5f;

        body.AddForce(new Vector2(direction, downwardsDirection) * dashForce, ForceMode2D.Impulse);
        

        dashTween = DOVirtual.DelayedCall(dashTime, () =>
        {
            _hasLanded = true;
            body.linearVelocity = Vector2.zero;
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
        dashTween?.Kill();
        _hasLanded = false;
        body.gravityScale = defaultGravity;
        body.transform.rotation = defaultRotation;
    }
}
