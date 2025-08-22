using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private ProjectileStats _stats;

    private int _currentInsight = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("IgnoreBullet"))
        {
            Destroy(gameObject);
            return;
        }
        _currentInsight++;
        if (_currentInsight >= _stats.Insight + 1)
            Destroy(gameObject);
        if (collision.GetComponent<Enemy>())
        {
            collision.GetComponent<Enemy>().TakeDamage(_stats.Damage);
            var hit = Instantiate(_stats.SpriteHits[Random.Range(0, _stats.SpriteHits.Length)], collision.transform.position, collision.transform.rotation);
            Destroy(hit, 0.1f);
        }
    }
}
