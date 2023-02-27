using UnityEngine;

[CreateAssetMenu(
    fileName = "TelekinesisSettings",
    menuName = "Game/Abilities/Telekinesis Settings"
)]
public class TelekinesisSettings : ScriptableObject
{
    public LayerMask pickableEntityMask;
    public LayerMask groundMask;
    public float entityMoveTime = 0f;
    public float entitySpacing = 0f;
}
