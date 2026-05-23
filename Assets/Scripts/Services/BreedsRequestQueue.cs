using System;
using System.Collections.Generic;
using UnityEngine;

public enum BreedRequestType
{
    ListBreeds,
    BreedDetails
}

public class BreedRequest
{
    public BreedRequestType Type { get; set; }
    public string BreedId { get; set; }  // null для List
}

public class BreedsRequestQueue
{
    private Queue<BreedRequest> _queue = new();
    private BreedRequest _currentRequest;
    
    public event Action<BreedRequest> OnRequestDequeued;
    
    public void Enqueue(BreedRequest request)
    {
        if (_currentRequest != null)
        {
            _queue.Enqueue(request);
            return;
        }
        
        _currentRequest = request;
        OnRequestDequeued?.Invoke(request);
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
    }
    
    public bool HasActiveRequest => _currentRequest != null;
    public int QueueCount => _queue.Count;
}