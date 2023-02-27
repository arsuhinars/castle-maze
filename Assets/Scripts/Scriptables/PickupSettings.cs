using UnityEngine;

[CreateAssetMenu(fileName = "PickupSettings", menuName = "Game/Pickup Settings")]
public class PickupSettings : ScriptableObject
{
    public string targetTag = string.Empty;
    public bool disableOnPickup = true;
    public bool resetOnReload = true;
    [Header("Animation settings")]
    public float rotationTime = 1.0f;
    public float moveAmplitude = 1.0f;
    public float moveTime = 1.0f;
}
