using UnityEngine;

[CreateAssetMenu(fileName = "CameraSettings", menuName = "Game/Camera Settings")]
public class CameraSettings : ScriptableObject
{
    public float moveSmoothTime = 1.0f;
    public float moveMaxSpeed = 1.0f;
    public Vector3 lookDirection = Vector3.forward;
    public float lookDistance = 1.0f;
}
