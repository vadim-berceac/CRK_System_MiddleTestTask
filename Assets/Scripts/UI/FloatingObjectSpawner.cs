using UnityEngine;
using Zenject;

public class FloatingObjectSpawner : MonoBehaviour
{
   [SerializeField] private string poolName;
   [SerializeField] private Vector3 spawnPosition;
   [SerializeField] private Vector3 endPosition;
   [SerializeField] private float moveDuration;
   [SerializeField] private float destroyDelay;
   
   [Inject] private readonly PoolContainer _poolContainer;
   private UIFloatingObjectPool _pool;

   private void Start()
   {
      _pool = _poolContainer.GetPool(poolName);
      PlayerResourcesService.OnMoneyAmountChanged += SpawnObject;
   }

   private void SpawnObject(float newValue, float difference)
   {
      var instance = _pool.GetFromPool();
      
      instance.PlayAnimation(spawnPosition, endPosition, moveDuration, destroyDelay);
   }

   private void OnDestroy()
   {
      PlayerResourcesService.OnMoneyAmountChanged -= SpawnObject;
   }
}
