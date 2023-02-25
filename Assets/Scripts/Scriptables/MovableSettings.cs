using UnityEngine;

[CreateAssetMenu(fileName = "MovableSettings", menuName = "Game/Movable Settings")]
public class MovableSettings : ScriptableObject
{
    public LeanTweenType easeType;
    public Vector3 moveOffset = Vector3.zero;
    public Vector3 rotationAxis = Vector3.up;
    public float rotationAngle = 0f;
    public float animationTime = 1f;
    public float startDelay = 0f;
    public float endDelay = 0f;
    public bool usePingPong = true;
    public bool isLooped = true;
    public bool playFromStart = true;
    public bool finishedFromStart = false;
}
