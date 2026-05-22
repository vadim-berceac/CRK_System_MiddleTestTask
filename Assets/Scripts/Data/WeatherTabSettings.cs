using UnityEngine;

[CreateAssetMenu(fileName = "WeatherTabSettings", menuName = "Scriptable Objects/WeatherTabSettings")]
public class WeatherTabSettings : ScriptableObject
{
    [field: SerializeField] public string Address { get; private set; }
    [field: SerializeField] public float UpdatingInterval { get; private set; }
}
