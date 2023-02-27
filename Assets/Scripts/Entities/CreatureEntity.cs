using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CreatureEntity : MonoBehaviour
{
    public CreatureSettings Settings
    {
        get => m_settings;
        set => m_settings = value;
    }

    public CharacterController CharacterController => m_charController;

    /// <summary>
    /// Текущий вектор движения существа.
    /// Длина, равная 1, означает максимальную скорость сущности.
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

    /// <summary>
    /// Угол на который направлен взгляд сущности.
    /// Если равен <c>null</c>, то сущность смотрит по
    /// направлению движения
    /// </summary>
    public float? TargetRotation
    {
        get => m_targetRot;
        set
        {
            m_targetRot = value;
        }
    }

    public bool IsAlive => m_isAlive;

    [SerializeField] private CreatureSettings m_settings;
    private CharacterController m_charController;
    private Vector2 m_moveVector = Vector2.zero;
    private Vector3 m_velocity = Vector3.zero;
    private float m_lastMoveAngle = 0.0f;
    private float m_currRot = 0.0f;
    private float m_rotVel = 0.0f;
    private float? m_targetRot = null;
    private bool m_jumpFlag = false;
    private bool m_isAlive = true;

    public void Jump()
    {
        if (!m_charController.isGrounded)
            return;

        m_jumpFlag = true;
    }

    public void Spawn()
    {
        if (m_isAlive)
            return;

        m_moveVector = Vector2.zero;
        m_velocity = Vector3.zero;
        m_lastMoveAngle = 0f;
        m_currRot = transform.rotation.eulerAngles.y;
        m_rotVel = 0f;
        m_jumpFlag = false;
        m_isAlive = true;
        gameObject.SetActive(true);
        OnSpawned();
    }

    public void Kill()
    {
        if (m_settings.isInvulnerable || !m_isAlive)
        {
            return;
        }

        m_isAlive = false;
        OnKilled();

        if (m_settings.disableOnDead)
        {
            gameObject.SetActive(false);
        }
    }

    protected virtual void OnSpawned() { }

    protected virtual void OnKilled() { }

    private void Awake()
    {
        m_charController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        Spawn();
    }

    protected virtual void Update()
    {
        if (!m_isAlive)
        {
            return;
        }

        float moveAccel = m_charController.isGrounded ?
            m_settings.moveAcceleration :
            m_settings.moveAccelerationInAir;

        // Расчитываем горизонтальную скорость через ускорение
        // и сопротивление движению
        var horVel = new Vector2(
            m_velocity.x,
            m_velocity.z
        );
        horVel += moveAccel * Time.deltaTime * MoveVector;
        horVel -= m_settings.horizontalDrag * Time.deltaTime * horVel;

        m_velocity.x = horVel.x;
        m_velocity.z = horVel.y;

        // Учитываем флаг прыжка
        if (m_jumpFlag)
        {
            m_jumpFlag = false;
            m_velocity.y = m_settings.jumpSpeed;
        }
        // Если игрок на земле
        if (!m_charController.isGrounded)
        {
            m_velocity.y += Physics.gravity.y * m_settings.gravityScale * Time.deltaTime;
        }

        // Учитываем ускорение свободного падения и сопротивление движению
        
        m_velocity.y -= m_settings.verticalDrag * m_velocity.y * Time.deltaTime;

        // Применяем скорость к контроллеру
        m_charController.Move(m_velocity * Time.deltaTime);


        float rotAngle = m_lastMoveAngle;

        if (m_targetRot != null)
        {
            rotAngle = (float)m_targetRot;
        }
        // Если вектор движения не близок к нулю и игрок находится на земле
        else if (m_charController.isGrounded && (
            !Mathf.Approximately(m_moveVector.x, 0.0f) ||
            !Mathf.Approximately(m_moveVector.y, 0.0f))
            )
        {
            // Находим текущий угол направления движения
            rotAngle = Mathf.Atan2(m_moveVector.x, m_moveVector.y) * Mathf.Rad2Deg;
            m_lastMoveAngle = rotAngle;
        }

        // Находим новый угол вращения
        m_currRot = Mathf.SmoothDampAngle(
            m_currRot,
            rotAngle,
            ref m_rotVel,
            m_settings.rotationSmoothTime,
            m_settings.rotationMaxSpeed
        );

        // Применяем вращение
        var rotation = Quaternion.Euler(0.0f, m_currRot, 0.0f);
        m_charController.transform.rotation = rotation;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!m_isAlive)
        {
            return;
        }

        var center = m_charController.bounds.center;
        float bottom = m_charController.bounds.min.y + m_charController.radius;

        // Если сущность касается пола
        if (m_charController.isGrounded && hit.point.y <= bottom)
        {
            // Расстояние от точки касания до центра
            float sqrDist = (
                new Vector2(hit.point.x, hit.point.z) -
                new Vector2(center.x, center.z)
            ).sqrMagnitude;

            if (sqrDist > (m_settings.slideThreshold * m_settings.slideThreshold))
            {
                // Заставляем сущность "соскальзывать" с краев
                var delta = new Vector3(
                    center.x - hit.point.x,
                    0.0f,
                    center.z - hit.point.z
                );
                delta *= m_settings.slideScale * Time.timeScale;
                m_velocity += delta;
            }

            return;
        }

        // Если это физический объект
        if (hit.rigidbody != null)
        {
            var dir = hit.point - center;
            dir.y = 0.0f;

            var impact = Vector3.Project(
                new Vector3(MoveVector.x, 0.0f, MoveVector.y),
                dir
            );

            // Обрабатываем воздействие на этот объект
            hit.rigidbody.AddForce(
                impact * m_settings.rigidbodyImpactScale
            );
        }
    }
}
