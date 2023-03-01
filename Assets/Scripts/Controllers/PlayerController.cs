using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerEntity))]
public class PlayerController : MonoBehaviour
{
    private PlayerEntity m_playerEntity;

    private void Awake()
    {
        m_playerEntity = GetComponent<PlayerEntity>();
    }

    private void Start()
    {
        GameManager.Instance.OnEnd += OnGameEnd;
    }

    public void OnMoveInputAction(InputAction.CallbackContext ctx)
    {
        if (!GameManager.Instance.IsStarted)
            return;

        m_playerEntity.MoveVector = ctx.ReadValue<Vector2>();
    }

    public void OnJumpInputAction(InputAction.CallbackContext ctx)
    {
        if (!GameManager.Instance.IsStarted)
            return;

        if (ctx.phase == InputActionPhase.Started)
        {
            m_playerEntity.Jump();
        }
    }

    public void OnPauseInputAction(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Started)
        {
            var gameManager = GameManager.Instance;
            if (gameManager.IsPaused)
            {
                gameManager.ResumeGame();
            }
            else
            {
                gameManager.PauseGame();
            }
        }
    }

    private void OnGameEnd(GameManager.GameEndReason reason)
    {
        m_playerEntity.MoveVector = Vector2.zero;
    }
}
