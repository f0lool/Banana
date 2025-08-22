using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected EnemyStats _stats;
    private int _currentHealth;

    [SerializeField] private float _healthModifier = 1.0f;

    private ImpactFlash _flash;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _flash = GetComponent<ImpactFlash>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {       
        _currentHealth = (int)(_stats.MaxHealth * _healthModifier);
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        //_flash.Flash(_spriteRenderer, 0.05f, Color.black);
        if (_currentHealth <= 0)
        {
            EnemyDeath();
        }
    }

    public void EnemyDeath()
    {
        StatsController.AddScore();
        var deathSprite = Instantiate(_stats.DeathSprites[Random.Range(0,_stats.DeathSprites.Length)], transform.position, transform.rotation);
        Destroy(gameObject);
        Destroy(deathSprite, 2.5f);
    }
}   
