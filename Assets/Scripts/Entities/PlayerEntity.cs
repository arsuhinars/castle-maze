using UnityEngine;

public class PlayerEntity : CreatureEntity
{
    private void Start()
    {
        GameManager.Instance.OnStart += OnGameStart;
    }

    private void OnGameStart()
    {
        // Перемещаем игрока к чекпоинту
        var currCheckpoint = StageManager.Instance.CurrentStage.CurrentCheckpoint;

        transform.position = currCheckpoint.PlayerSpawn.position;

        // Перемещаем камеру к игроку
        var camController = GameManager.Instance.CameraController;
        camController.TargetPoint = transform.position;
        camController.MoveInstantly();

        // Спавним игрока
        Spawn();
    }

    protected override void Update()
    {
        base.Update();

        // Двигаем камеру к игроку
        var camController = GameManager.Instance.CameraController;
        var camTarget = transform.position;

        // Если игрок в полете, то не двигаем камеру по вертикали
        if (!CharacterController.isGrounded)
        {
            camTarget.y = camController.TargetPoint.y;
        }

        camController.TargetPoint = camTarget;
    }

    protected override void OnKilled()
    {
        GameManager.Instance.EndGame();
    }
}
