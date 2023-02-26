using UnityEngine;

public class PlayerEntity : CreatureEntity
{
    /// <summary>
    /// Активная способность игрока. Если равна <c>null</c>, то
    /// никакая способность в данный момент не активна.
    /// </summary>
    public PlayerAbility ActiveAbility
    {
        get => m_activeAbility;
        set => m_activeAbility = value;
    }

    private PlayerAbility m_activeAbility = null;

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
