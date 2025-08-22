using UnityEngine;

public enum LevelType
{
    Adventure,
    Deathmatch
}

[CreateAssetMenu(menuName ="Stats/GameConfig")]
public class GameConfig : ScriptableObject
{
    public WeaponStats WeaponStats;
    public PlayerStats PlayerStats;

    public LevelType LevelType;

    public int LevelIndex;

    public LevelConfig LevelConfig;
}
