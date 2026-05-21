using UnityEngine;

[CreateAssetMenu(fileName = "ClickerSettings", menuName = "Scriptable Objects/ClickerSettings")]
public class ClickerSettings : ScriptableObject
{
    [field: SerializeField] public float StartMoneyAmount { get; private set; } = 10f;
    [field: SerializeField] public float MoneyPerUpdate { get; private set; } = 1f;
    [field: SerializeField] public float UpdateInterval { get; private set; } = 3f;
    [field: SerializeField] public float UpdateCost { get; private set; } = 1f;
   
    [field: Header("Sound Effects")]
    [field: SerializeField] public SoundSettings UpdateSound { get; private set; }
}
