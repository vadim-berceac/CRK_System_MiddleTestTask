using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class IconLoader
{
    private Dictionary<string, Texture2D> _iconCache = new();
    
    public async UniTask<Texture2D> LoadIcon(string iconUrl)
    {
        if (string.IsNullOrEmpty(iconUrl))
            return null;
        
        if (_iconCache.TryGetValue(iconUrl, out var cachedTexture))
        {
            return cachedTexture;
        }
        
        try
        {
            using var request = UnityEngine.Networking.UnityWebRequestTexture.GetTexture(iconUrl);
            var asyncOp = request.SendWebRequest();
            
            await asyncOp.WithCancellation(System.Threading.CancellationToken.None);
            
            if (request.result != UnityEngine.Networking.UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Failed to load icon: {request.error}");
                return null;
            }
            
            var texture = ((UnityEngine.Networking.DownloadHandlerTexture)request.downloadHandler).texture;
            
            _iconCache[iconUrl] = texture;
            
            return texture;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Icon loading error: {ex.Message}");
            return null;
        }
    }
}