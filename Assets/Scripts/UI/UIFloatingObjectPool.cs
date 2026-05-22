using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UIFloatingObjectPool
{
    [field: SerializeField] public string PoolName { get; set; }
    [SerializeField] private FloatingUIObject prefab;
    [SerializeField] private int initialPoolSize = 10;
 
    private readonly Queue<FloatingUIObject> _pool = new ();
    private Transform _poolParent;
    private Transform _canvas;
 
    public void Initialize(Transform canvas)
    {
        _canvas = canvas;
        _poolParent = new GameObject(PoolName, typeof(RectTransform)).transform;
        _poolParent.SetParent(_canvas, false);
        
        var rect = _poolParent as RectTransform;
        rect.anchoredPosition = Vector2.zero;
        rect.sizeDelta = Vector2.zero;
        rect.localScale = Vector3.one;
        
        for (var i = 0; i < initialPoolSize; i++)
        {
            CreateNewInstance();
        }
    }
 
    private FloatingUIObject CreateNewInstance()
    {
        var instance = Object.Instantiate(prefab, _poolParent);
        instance.Initialize(this);
        instance.gameObject.SetActive(false);
        _pool.Enqueue(instance);
        return instance;
    }
 
    public FloatingUIObject GetFromPool()
    {
        FloatingUIObject obj;
 
        if (_pool.Count > 0)
        {
            obj = _pool.Dequeue();
        }
        else
        {
            obj = CreateNewInstance();
            _pool.Enqueue(obj);
            obj = _pool.Dequeue();
        }
 
        obj.gameObject.SetActive(true);
        obj.ResetState();
        return obj;
    }
 
    public void ReturnToPool(FloatingUIObject instance)
    {
        instance.gameObject.SetActive(false);
        instance.ResetState();
        _pool.Enqueue(instance);
    }
}
