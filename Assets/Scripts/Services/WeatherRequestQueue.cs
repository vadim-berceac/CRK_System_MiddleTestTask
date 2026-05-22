using System;
using System.Collections.Generic;
using UnityEngine;

public class WeatherRequestQueue
{
    private readonly Queue<WeatherRequestItem> _queue = new();
    private WeatherRequestItem _currentRequest;
    
    public event Action<WeatherRequestItem> OnRequestDequeued;
    public bool HasActiveRequest => _currentRequest != null;
    public int QueueCount => _queue.Count;
    
    public void Enqueue(WeatherRequestItem item)
    {
        if (_currentRequest != null)
        {
            Debug.Log("Request already in progress, queuing...");
            _queue.Enqueue(item);
            return;
        }
        
        _currentRequest = item;
        OnRequestDequeued?.Invoke(item);
    }
    
    public void Dequeue()
    {
        _currentRequest = null;
        
        if (_queue.Count > 0)
        {
            var nextRequest = _queue.Dequeue();
            _currentRequest = nextRequest;
            OnRequestDequeued?.Invoke(nextRequest);
        }
    }
    
    public void Clear()
    {
        _queue.Clear();
        _currentRequest = null;
        Debug.Log("Request queue cleared");
    }
}

public class WeatherRequestItem
{
    public System.Action<WeatherData> OnSuccess { get; set; }
    public System.Action<string> OnError { get; set; }
}