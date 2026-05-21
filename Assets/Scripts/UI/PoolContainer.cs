using System.Linq;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "PoolContainer", menuName = "Scriptable Objects/PoolContainer")]
public class PoolContainer : ScriptableObject
{
    [SerializeField] private UIFloatingObjectPool[] pools;

    [Inject]
    private void Construct(Canvas parentCanvas)
    {
        foreach (var p in pools)
        {
            p.Initialize(parentCanvas.transform);
        }
    }
    
    public UIFloatingObjectPool GetPool(string poolName)
    {
        var result = pools.FirstOrDefault(p => p.PoolName == poolName);
        return result;
    }
}
