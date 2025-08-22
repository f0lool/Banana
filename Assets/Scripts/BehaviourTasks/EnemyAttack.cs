using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private int _damage = 15;

    private void OnTriggerStay2D(Collider2D collision)
    {
        string layerName = LayerMask.LayerToName(collision.gameObject.layer);

        if (layerName == "Player")
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(_damage);
        }
    }
}
