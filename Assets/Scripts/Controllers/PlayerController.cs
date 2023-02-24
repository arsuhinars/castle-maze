using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    private PlayerEntity m_playerEntity;

    private void Awake()
    {
        m_playerEntity = GetComponent<PlayerEntity>();
    }

    public void OnMoveInputAction(InputAction.CallbackContext ctx)
    {
        m_playerEntity.MoveVector = ctx.ReadValue<Vector2>();
    }

    public void OnJumpInputAction(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Started)
        {
            m_playerEntity.Jump();
        }
    }
}