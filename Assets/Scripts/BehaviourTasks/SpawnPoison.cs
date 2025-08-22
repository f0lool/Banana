using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;

public class SpawnPoison : Action
{
    public GameObject _poisonPrefab;
    public Vector2 _offsetPoison;

    public override void OnStart()
    {
        var pois = Object.Instantiate(_poisonPrefab, (Vector2)transform.position + (_offsetPoison * transform.localScale.x), transform.rotation);
        Object.Destroy(pois, 2);
    }
}
