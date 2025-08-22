using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;
using YG.Utils.LB;

public class PlayerHealth : MonoBehaviour
{
    private UIController _uiController;
    private PlayerStats _stats;
    private Rigidbody2D _rigidbody;
    private int _health;
    private Action<int, int> OnHealtChange;
    [SerializeField] private Color _invulnerableColor;
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private Material _flashMaterial;
    private Material _originMaterial;

    private PlayerAnimator _animator;


    public void Init(PlayerStats stats, UIController uIController)
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _originMaterial = _spriteRenderer.material;
        _rigidbody = GetComponent<Rigidbody2D>();
        _stats = stats;
        _health = _stats.MaxHealth;
        _uiController = uIController;
        _uiController.ChangePlayerHealthText(_health, _stats.MaxHealth);
        OnHealtChange += _uiController.ChangePlayerHealthText;
        _animator = GetComponent<PlayerAnimator>();
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        if (_health <= 0)
            Death();

        gameObject.layer = LayerMask.NameToLayer("PlayerInvulnerable");
        OnHealtChange?.Invoke(_health, _stats.MaxHealth);

        Vector2 newVelocity;
        newVelocity.x = 0;
        newVelocity.y = 0;
        _rigidbody.linearVelocity = newVelocity;

        _spriteRenderer.material = _flashMaterial;

        Vector2 newForce;
        newForce.x = transform.localScale.x * _stats.HurtRecoil.x;
        newForce.y = _stats.HurtRecoil.y;
        _rigidbody.AddForce(newForce, ForceMode2D.Impulse);

        _stats.IsInputEnabled = false;

        StartCoroutine(RecoverFromHurtCoroutine());
    }

    private IEnumerator RecoverFromHurtCoroutine()
    {
        yield return new WaitForSeconds(.125f);
        _spriteRenderer.material = _originMaterial;
        yield return new WaitForSeconds(_stats.HurtRecoilTime);
        _stats.IsInputEnabled = true;
        yield return new WaitForSeconds(_stats.HurtRecoveryTime);
        gameObject.layer = LayerMask.NameToLayer("Player");
    }

    private IEnumerator DeathAnimation()
    {
        if (StatsController.CurrentScore > YandexGame.savesData.Score)
        {
            YandexGame.savesData.Score = StatsController.CurrentScore;
            YandexGame.NewLeaderboardScores("ScoreBoard", StatsController.CurrentScore);
            YandexGame.SaveProgress();
        }

        yield return new WaitForSeconds(.5f);
        
        _uiController.OpenLosePanel();
        _uiController.SaveVolume();
        MusicManager.PlayBackgroundMusicMenu();

        gameObject.layer = LayerMask.NameToLayer("Player");
        _stats.IsInputEnabled = true;
    }

    private void Death()
    {
        _stats.IsInputEnabled = false;
        gameObject.layer = LayerMask.NameToLayer("PlayerInvulnerable");
        _rigidbody.linearVelocity = Vector2.zero;
        _animator.SetTrigger("Death");

        StartCoroutine(DeathAnimation());
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("DeathZone"))
        {
            Death();
        }
    }
}


