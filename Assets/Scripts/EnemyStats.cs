using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Stats/EnemyStats")]
public class EnemyStats : ScriptableObject
{
    public float MovementSpeed;
    public int MaxHealth;
    public int Damage;

    public float CooldownAttack;

    public GameObject[] DeathSprites;
}
