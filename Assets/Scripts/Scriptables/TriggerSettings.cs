using UnityEngine;

[CreateAssetMenu(fileName = "TriggerSettings", menuName = "Game/Trigger Settings")]
public class TriggerSettings : ScriptableObject
{
    public string targetTag = string.Empty;
    public bool triggerOnce = true;
    public bool resetOnGameRestart = true;
}
