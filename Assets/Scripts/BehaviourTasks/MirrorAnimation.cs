using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class MirrorAnimation : Action
{
    public string NameAnimationBoolMirror;
    private Animator _animator;

    public override void OnStart()
    {
        _animator = GetComponent<Animator>();
        
    }

    public override TaskStatus OnUpdate()
    {
        if (transform.localScale.x == 1)
            _animator.SetBool(NameAnimationBoolMirror, false);
        else
            _animator.SetBool(NameAnimationBoolMirror, true);

        return TaskStatus.Success;
    }
}
