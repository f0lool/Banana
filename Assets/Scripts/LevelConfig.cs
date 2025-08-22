using UnityEngine;

[CreateAssetMenu(menuName ="LevelConfig")]
public class LevelConfig : ScriptableObject
{
    public int Index;
    public bool Unlock;
    public bool Complete;
    public string NameScene;
    public LevelConfig NeedLevelComplete;
    public Sprite IconLevelSprite;
}
