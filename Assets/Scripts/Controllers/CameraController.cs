using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera TargetCamera
    {
        get => m_targetCamera;
        set => m_targetCamera = value;
    }
    
    /// <summary>
    /// Объект настроек камеры. Обратите внимание, что для применения изменений нужно
    /// заново присвоить объект.
    /// </summary>
    public CameraSettings Settings
    {
        get => m_settings;
        set
        {
            m_settings = value;
            ApplySettings();
        }
    }

    public Vector3 TargetPoint
    {
        get => m_targetPoint;
        set => m_targetPoint = value;
    }

    [SerializeField] private Camera m_targetCamera;
    [SerializeField] private CameraSettings m_settings;
    
    private Vector3 m_normalizedLookDir = Vector3.forward;
    private Vector3 m_targetPoint = Vector3.zero;
    private Vector3 m_velocity = Vector3.zero;

    /// <summary>
    /// Метод для перемещения камеры к <c>TargetPoint</c> мгновенно, без плавного перехода.
    /// </summary>
    public void MoveInstantly()
    {
        m_targetCamera.transform.position = CalculateCameraPosition();
        m_velocity = Vector3.zero;
    }
    
    private void Start()
    {
        ApplySettings();
    }

    private void Update()
    {
        // Плавно перемещаем камеру к заданной точке
        m_targetCamera.transform.position = Vector3.SmoothDamp(
            m_targetCamera.transform.position,
            CalculateCameraPosition(),
            ref m_velocity,
            m_settings.moveSmoothTime,
            m_settings.moveMaxSpeed
        );
    }

    private void ApplySettings()
    {
        m_normalizedLookDir = m_settings.lookDirection.normalized;

        m_targetCamera.transform.rotation = Quaternion.LookRotation(
            m_normalizedLookDir
        );
    }

    private Vector3 CalculateCameraPosition()
    {
        return m_targetPoint - m_normalizedLookDir * m_settings.lookDistance;
    }
}
