using System.Security.Cryptography;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator _animatorController;

    private void Awake()
    {
        _animatorController = GetComponentInChildren<Animator>();
    }

    public void SetBool(string name, bool value)
    {
        _animatorController.SetBool(name, value);
    }

    public void SetFloat(string name, float value)
    {
        _animatorController.SetFloat(name, value);
    }

    public void SetTrigger(string name)
    {
        _animatorController.SetTrigger(name);
        
    }
}
