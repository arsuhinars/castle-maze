using UnityEngine;

public class PlayerEntity : CreatureEntity
{
    private void Start()
    {
        GameManager.Instance.OnStart += OnGameStart;
    }

    private void OnGameStart()
    {
        GameManager.Instance.CameraController.TargetPoint = transform.position;
    }

    protected override void Update()
    {
        base.Update();

        // Двигаем камеру к игроку
        var camController = GameManager.Instance.CameraController;
        var camTarget = new Vector3(
            transform.position.x,
            camController.TargetPoint.y,
            transform.position.z
        );
        camController.TargetPoint = camTarget;
    }
}
