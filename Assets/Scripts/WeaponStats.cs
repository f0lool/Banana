using Unity.VisualScripting;
using UnityEngine;

public enum ConditionType { CompleteLevel, FindParticles }

[CreateAssetMenu(menuName = "Stats/WeaponStats")]
public class WeaponStats : ScriptableObject
{
    public Sprite WeaponSprite;
    public Sprite IconWeaponSprite;
    public string WeaponName;
    public int MagazineSize;
    public float ReloadCooldown;

    public ProjectileStats ProjectileStats;
    public Vector2 MuzzlePosition;

    public float ShootForce;
    public float CooldownBetweenShots;

    public float BulletLifeTime;
    public GameObject MuzzleFlash;

    public bool IsAutoFire;
    public bool Unlock;
    public bool IsLaser;

    public ConditionType ConditionType;

    public LevelConfig NeedLevelComplete;
}

