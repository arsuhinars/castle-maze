using UnityEngine;

public class AbilityMaterialize : MonoBehaviour
{
    [SerializeField] private MaterializeSettings m_settings;

    private Camera m_camera;
    private PlayerEntity m_player;
    private PointerCapturer m_pointerCapturer;
    private Collider m_bridge = null;
    private Vector3? m_lastTargetPos = null;
    private Vector3 m_velocity = Vector3.zero;
    private bool m_isBridgedPlaced = false;

    private void Start()
    {
        m_camera = GameManager.Instance.CameraController.TargetCamera;
        m_player = GameManager.Instance.PlayerEntity;

        var inGameView = UIManager.Instance.GetView(
            Defines.IN_GAME_VIEW
        ) as InGameView;

        m_pointerCapturer = inGameView.PointerCapturer;
        m_pointerCapturer.OnPointerClick += OnPointerClick;
        m_pointerCapturer.OnPointerExit += OnPointerExit;

        GameManager.Instance.OnStart += OnGameStart;

        CreateBridge();
    }

    private void OnDestroy()
    {
        m_pointerCapturer.OnPointerClick -= OnPointerClick;
        m_pointerCapturer.OnPointerExit -= OnPointerExit;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnStart -= OnGameStart;
        }

        ReleaseBridge();
    }

    private void Update()
    {
        if (!GameManager.Instance.IsStarted || m_isBridgedPlaced)
            return;

        var targetPos = RaycastFromPointer();

        if (m_lastTargetPos == null && targetPos != null)
        {
            m_bridge.gameObject.SetActive(true);
            m_bridge.transform.position = (Vector3)targetPos;
            m_velocity = Vector3.zero;
        }
        else if (m_lastTargetPos != null && targetPos == null)
        {
            m_player.TargetRotation = null;
            m_bridge.gameObject.SetActive(false);
        }
        else if (targetPos != null)
        {
            m_bridge.transform.position = Vector3.SmoothDamp(
                m_bridge.transform.position,
                (Vector3)targetPos,
                ref m_velocity,
                m_settings.bridgeMoveTime
            );

            Vector3 direction = m_bridge.transform.position
                - m_player.transform.position;

            m_player.TargetRotation = Utils.RotationFromVector2(
                new Vector2(direction.x, direction.z)
            );
        }

        m_lastTargetPos = targetPos;
    }

    private void OnPointerClick()
    {
        if (m_isBridgedPlaced)
        {
            m_isBridgedPlaced = false;
            m_lastTargetPos = null;
            m_bridge.gameObject.SetActive(true);
            m_bridge.enabled = false;
            return;
        }

        if (m_bridge != null && m_bridge.gameObject.activeInHierarchy)
        {
            m_isBridgedPlaced = true;
            m_bridge.enabled = true;

            m_player.TargetRotation = null;
        }
    }

    private void OnPointerExit()
    {
        if (m_bridge != null && !m_isBridgedPlaced)
        {
            m_bridge.gameObject.SetActive(false);
            m_lastTargetPos = null;
        }
    }

    private void OnGameStart()
    {
        ReleaseBridge();
        CreateBridge();
    }

    private void CreateBridge()
    {
        var pos = RaycastFromPointer();
        m_bridge = Instantiate(
            m_settings.bridgePrefab,
            pos ?? Vector3.zero,
            Quaternion.identity
        );
        m_bridge.gameObject.SetActive(pos != null);
        m_bridge.enabled = false;

        m_velocity = Vector3.zero;
        m_lastTargetPos = null;
        m_player.TargetRotation = null;
    }

    private void ReleaseBridge()
    {
        if (m_bridge != null)
        {
            Destroy(m_bridge.gameObject);
            m_isBridgedPlaced = false;
            m_bridge = null;

            m_player.TargetRotation = null;
        }
    }

    private Vector3? RaycastFromPointer()
    {
        if (!Physics.Raycast(
                m_camera.ScreenPointToRay(m_pointerCapturer.PointerPos),
                out var hitInfo,
                Mathf.Infinity,
                m_settings.groundMask
            ))
        {
            return null;
        }

        return hitInfo.point;
    }
}
