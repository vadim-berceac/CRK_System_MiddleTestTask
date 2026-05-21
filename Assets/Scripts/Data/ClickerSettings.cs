using UnityEngine;

[CreateAssetMenu(fileName = "ClickerSettings", menuName = "Scriptable Objects/ClickerSettings")]
public class ClickerSettings : ScriptableObject
{
    [field: SerializeField] public float MoneyPerClick { get; private set; } = 1f;
    [field: SerializeField] public float AutoClickTime { get; private set; } = 3f;
    [field: SerializeField] public float ClickCost { get; private set; } = 1f;
}
