using UnityEngine;

[CreateAssetMenu(fileName = "CreatureSettings", menuName = "Game/Creature Settings")]
public class CreatureSettings : ScriptableObject
{
    public float gravityScale = 1.0f;
    public float moveAcceleration = 1.0f;
    public float moveAccelerationInAir = 1.0f;
    public float jumpSpeed = 1.0f;
    public float horizontalDrag = 1.0f;
    public float verticalDrag = 1.0f;
    public float rotationSmoothTime = 1.0f;
    public float rotationMaxSpeed = 1.0f;
    public float slideThreshold = 1.0f;
    public float slideScale = 1.0f;
}
