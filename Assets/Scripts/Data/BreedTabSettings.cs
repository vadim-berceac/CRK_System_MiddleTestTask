using UnityEngine;

[CreateAssetMenu(fileName = "BreedTabSettings", menuName = "Scriptable Objects/BreedTabSettings")]
public class BreedTabSettings : ScriptableObject
{
    [field: SerializeField] public string Address { get; private set; }
}
