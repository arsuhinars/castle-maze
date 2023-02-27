using UnityEngine;

public class AbilityTelekinesis : MonoBehaviour
{
    [SerializeField] private TelekinesisSettings m_settings;

    /// <summary>
    /// Вспомогательный класс для подобранного объекта.
    /// Содержит поля с его компонентами для оптимизации их
    /// поиска.
    /// </summary>
    private class PickedEntity
    {
        public Transform transform;
        public Rigidbody rigidbody;

        public float radius = 0f;
        public Vector3 bottomDelta = Vector3.zero;
        public Vector3 velocity = Vector3.zero;
    }

    private Camera m_camera;
    private PlayerEntity m_player;
    private PointerCapturer m_pointerCapturer;
    private PickedEntity m_pickedEntity = null;
    private Vector3 m_targetPos = Vector3.zero;

    private void Start()
    {
        m_camera = GameManager.Instance.CameraController.TargetCamera;
        m_player = GameManager.Instance.PlayerEntity;

        var inGameView = UIManager.Instance.GetView(
            Defines.IN_GAME_VIEW
        ) as InGameView;

        m_pointerCapturer = inGameView.PointerCapturer;
        m_pointerCapturer.OnPointerClick += OnPointerClick;

        GameManager.Instance.OnStart += OnGameStart;
    }

    private void OnDestroy()
    {
        ReleasePickedEntity();

        m_pointerCapturer.OnPointerClick -= OnPointerClick;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnStart -= OnGameStart;
        }
    }

    private void Update()
    {
        if (!GameManager.Instance.IsStarted ||
            m_pickedEntity == null)
            return;

        // Пускаем луч и ищем поверхность земли
        if (Physics.Raycast(
                m_camera.ScreenPointToRay(m_pointerCapturer.PointerPos),
                out var hitInfo,
                Mathf.Infinity,
                m_settings.groundMask
            ))
        {
            m_targetPos = hitInfo.point + m_pickedEntity.bottomDelta;
            m_targetPos += Vector3.up * m_settings.entitySpacing;
        }

        // Плавно перемещаем сущность к заданной точке
        m_pickedEntity.transform.position = Vector3.SmoothDamp(
            m_pickedEntity.transform.position,
            m_targetPos,
            ref m_pickedEntity.velocity,
            m_settings.entityMoveTime
        );

        // Заставляем игрока смотреть в направлении объекта
        var lookDir =
            m_pickedEntity.transform.position -
            m_player.transform.position;
        m_player.TargetRotation = Mathf.Atan2(lookDir.x, lookDir.z) * Mathf.Rad2Deg;
    }

    private void OnPointerClick()
    {
        // Если уже выделен объект, то скидываем его
        if (m_pickedEntity != null)
        {
            ReleasePickedEntity();
            return;
        }

        // Пускаем луч и ищем подходящую сущность
        if (!Physics.Raycast(
                m_camera.ScreenPointToRay(m_pointerCapturer.PointerPos),
                out var hitInfo,
                Mathf.Infinity,
                m_settings.pickableEntityMask
            ))
        {
            return;
        }

        var bounds = hitInfo.collider.bounds;
        var bottomCenter = new Vector3(
            bounds.center.x, bounds.min.y, bounds.center.z
        );

        // Ищем разность координат между центром сущности и нижним краем
        var bottomDelta = hitInfo.transform.position - bottomCenter;
        
        m_pickedEntity = new PickedEntity
        {
            transform = hitInfo.transform,
            rigidbody = hitInfo.rigidbody,
            radius = Mathf.Max(bounds.size.x, bounds.size.z),
            bottomDelta = bottomDelta
        };

        m_pickedEntity.rigidbody.detectCollisions = false;
        m_pickedEntity.rigidbody.isKinematic = true;
    }

    private void OnGameStart()
    {
        ReleasePickedEntity();
    }

    private void ReleasePickedEntity()
    {
        if (m_pickedEntity == null)
            return;

        m_player.TargetRotation = null;

        if (m_pickedEntity.rigidbody != null)
        {
            m_pickedEntity.rigidbody.detectCollisions = true;
            m_pickedEntity.rigidbody.isKinematic = false;
        }

        m_pickedEntity = null;
    }
}
