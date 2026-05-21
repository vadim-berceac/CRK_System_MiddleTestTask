using UnityEngine;

[CreateAssetMenu(fileName = "EnergySettings", menuName = "Scriptable Objects/EnergySettings")]
public class EnergySettings : ScriptableObject
{
    [field: SerializeField] public float MaxEnergy { get; private set; } = 1000f;
    [field: SerializeField] public float StartEnergy { get; private set; } = 250;
    [field: SerializeField] public float UpdateInterval { get; private set; } = 10f;
    [field: SerializeField] public float EnergyUpdateAmount { get; private set; } = 10f;
    
    [field: Header("Sound Effects")]
    [field: SerializeField] public SoundSettings UpdateSound { get; private set; } 
}
