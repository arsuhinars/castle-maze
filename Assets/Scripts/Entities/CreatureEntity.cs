using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

[RequireComponent(typeof(CharacterController))]
public class CreatureEntity : MonoBehaviour
{
    public CreatureSettings Settings
    {
        get => m_settings;
        set => m_settings = value;
    }

    /// <summary>
    /// “екущий вектор движени€ существа.
    /// ƒлина, равна€ 1, означает максимальную скорость сущности.
    /// </summary>
    public Vector2 MoveVector
    {
        get => m_moveVector;
        set => m_moveVector = value;
    }

    public float Rotation
    {
        get => m_currRot;
        set
        {
            m_lastMoveAngle = value;
            m_currRot = value;
            m_rotVel = 0.0f;
        }
    }

    [SerializeField] private CreatureSettings m_settings;
    private CharacterController m_charController;
    private Vector2 m_moveVector = Vector2.zero;
    private Vector3 m_velocity = Vector3.zero;
    private float m_lastMoveAngle = 0.0f;
    private float m_currRot = 0.0f;
    private float m_rotVel = 0.0f;

    public void Jump()
    {
        if (!m_charController.isGrounded)
            return;

        m_velocity.y = m_settings.jumpSpeed;
    }

    private void Awake()
    {
        m_charController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        float moveAccel = m_charController.isGrounded ?
            m_settings.moveAcceleration :
            m_settings.moveAccelerationInAir;

        // –асчитываем горизонтальную скорость через ускорение
        // и сопротивление движению
        var horVel = new Vector2(m_velocity.x, m_velocity.z);
        horVel += moveAccel * Time.deltaTime * MoveVector;
        horVel -= m_settings.horizontalDrag * Time.deltaTime * horVel;

        m_velocity.x = horVel.x;
        m_velocity.z = horVel.y;

        // ”читываем ускорение свободного падени€ и сопротивление движению
        m_velocity.y += Physics.gravity.y * Time.deltaTime;
        m_velocity.y -= m_settings.verticalDrag * Time.deltaTime;

        // ѕримен€ем скорость к контроллеру
        m_charController.Move(m_velocity * Time.deltaTime);


        // ƒалее обрабатываем вращение только если сущность находитс€ на земле
        //if (!m_charController.isGrounded)
        //    return;

        float moveAngle = m_lastMoveAngle;

        // ≈сли вектор движени€ не близок к нулю и игрок находитс€ на земле
        if (m_charController.isGrounded && (
            !Mathf.Approximately(m_moveVector.x, 0.0f) ||
            !Mathf.Approximately(m_moveVector.y, 0.0f))
            )
        {
            // Ќаходим текущий угол направлени€ движени€
            moveAngle = Mathf.Atan2(m_moveVector.x, m_moveVector.y) * Mathf.Rad2Deg;
            m_lastMoveAngle = moveAngle;
        }

        // Ќаходим новый угол вращени€
        m_currRot = Mathf.SmoothDampAngle(
            m_currRot,
            moveAngle,
            ref m_rotVel,
            m_settings.rotationSmoothTime,
            m_settings.rotationMaxSpeed
        );

        // ѕримен€ем вращение
        var rotation = Quaternion.Euler(0.0f, m_currRot, 0.0f);
        m_charController.transform.rotation = rotation;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        var center = m_charController.bounds.center;
        float bottom = m_charController.bounds.min.y + m_charController.radius;

        if (m_charController.isGrounded && hit.point.y <= bottom)
        {
            float sqrDist = (
                new Vector2(hit.point.x, hit.point.z) -
                new Vector2(center.x, center.z)
            ).sqrMagnitude;

            if (sqrDist > (m_settings.slideThreshold * m_settings.slideThreshold))
            {
                // «аставл€ем сущность "соскальзывать" с краев
                var delta = new Vector3(
                    center.x - hit.point.x,
                    0.0f,
                    center.z - hit.point.z
                );
                delta *= m_settings.slideScale * Time.timeScale;
                m_velocity += delta;
            }
        }
    }
}
