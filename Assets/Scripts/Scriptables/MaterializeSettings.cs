using UnityEngine;

[CreateAssetMenu(
    fileName = "MaterializeSettings",
    menuName = "Game/Abilities/Materialize Settings"
)]
public class MaterializeSettings : ScriptableObject
{
    public LayerMask groundMask;
    public Collider bridgePrefab;
    public float bridgeMoveTime = 0f;
}
