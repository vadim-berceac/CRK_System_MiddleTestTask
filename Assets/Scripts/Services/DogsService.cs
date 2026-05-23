using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

public class DogsService
{
    [Inject] private readonly BreedTabSettings _breedTabSettings;

    public event Action<BreedData[]> OnBreedsListFetched;
    public event Action<BreedDetailsData> OnBreedDetailsFetched;
    public event Action<string> OnError;

    private System.Threading.CancellationTokenSource _listRequestCts;
    private System.Threading.CancellationTokenSource _detailRequestCts;

    public async UniTask FetchBreedsList(System.Threading.CancellationToken externalToken = default)
    {
        _listRequestCts?.Cancel();
        _listRequestCts?.Dispose();

        _listRequestCts = new System.Threading.CancellationTokenSource();
        var linkedCts = System.Threading.CancellationTokenSource.CreateLinkedTokenSource(
            externalToken,
            _listRequestCts.Token
        );

        try
        {
            using var request = UnityWebRequest.Get(_breedTabSettings.Address);
            var asyncOp = request.SendWebRequest();

            await asyncOp.WithCancellation(linkedCts.Token);

            if (request.result != UnityWebRequest.Result.Success)
            {
                OnError?.Invoke($"Breeds list error: {request.error}");
                return;
            }

            var json = request.downloadHandler.text;
            var breeds = ParseBreedsListResponse(json);

            Debug.Log($"[DogsService] Fetched {breeds.Length} breeds");
            OnBreedsListFetched?.Invoke(breeds);
        }
        catch (OperationCanceledException)
        {
            Debug.Log("[DogsService] Breeds list request cancelled");
        }
        catch (Exception ex)
        {
            OnError?.Invoke(ex.Message);
        }
    }

    public async UniTask FetchBreedDetails(string breedId, System.Threading.CancellationToken externalToken = default)
    {
        _detailRequestCts?.Cancel();
        _detailRequestCts?.Dispose();

        _detailRequestCts = new System.Threading.CancellationTokenSource();
        var linkedCts = System.Threading.CancellationTokenSource.CreateLinkedTokenSource(
            externalToken,
            _detailRequestCts.Token
        );

        try
        {
            var url = $"{_breedTabSettings.Address}/{breedId}";

            using var request = UnityWebRequest.Get(url);
            var asyncOp = request.SendWebRequest();

            await asyncOp.WithCancellation(linkedCts.Token);

            if (request.result != UnityWebRequest.Result.Success)
            {
                OnError?.Invoke($"Breed details error: {request.error}");
                return;
            }

            var json = request.downloadHandler.text;

            var details = ParseBreedDetailsResponse(json);
            OnBreedDetailsFetched?.Invoke(details);
        }
        catch (Exception ex)
        {
            OnError?.Invoke(ex.Message);
        }
    }

    public void CancelListRequest()
    {
        _listRequestCts?.Cancel();
    }

    public void CancelDetailRequest()
    {
        _detailRequestCts?.Cancel();
    }

    private BreedData[] ParseBreedsListResponse(string json)
    {
        var response = JsonUtility.FromJson<BreedsListResponse>(json);

        if (response?.data != null && response.data.Length > 0)
        {
            var breeds = new BreedData[response.data.Length];
            for (var i = 0; i < response.data.Length; i++)
            {
                breeds[i] = new BreedData
                {
                    Id = response.data[i].id,
                    Name = response.data[i].attributes.name
                };
            }

            return breeds;
        }

        throw new Exception("Invalid breeds list response");
    }

    private BreedDetailsData ParseBreedDetailsResponse(string json)
    {
        var response = JsonUtility.FromJson<BreedDetailsResponse>(json);
    
        if (response?.data?.attributes != null)
        {
            return new BreedDetailsData
            {
                Id = response.data.id,
                Name = response.data.attributes.name,
                Description = response.data.attributes.description
            };
        }
    
        throw new Exception("Invalid breed details response");
    }
}

public class BreedData
{
    public string Id { get; set; }
    public string Name { get; set; }
}

public class BreedDetailsData
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}

[System.Serializable]
public class BreedsListResponse
{
    public BreedItem[] data;
    public Meta meta;
}

[System.Serializable]
public class BreedItem
{
    public string id;
    public string type;
    public BreedAttributes attributes;
}

[System.Serializable]
public class BreedDetailsResponse
{
    public BreedDetailItem data;
}

[System.Serializable]
public class BreedDetailItem
{
    public string id;
    public string type;
    public BreedAttributes attributes;
}

[System.Serializable]
public class BreedAttributes
{
    public string name;
    public string description;
}

[System.Serializable]
public class Meta
{
    public Pagination pagination;
}

[System.Serializable]
public class Pagination
{
    public int current;
    public int records;
}