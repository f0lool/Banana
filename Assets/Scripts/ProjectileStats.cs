using UnityEngine;

[CreateAssetMenu(menuName ="Stats/ProjectileStats")]
public class ProjectileStats : ScriptableObject
{
    public GameObject ProjectilePrefab;
    public int Damage;
    public GameObject[] SpriteHits;

    public int Insight;
}
