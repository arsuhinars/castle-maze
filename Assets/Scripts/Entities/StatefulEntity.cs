using UnityEngine;

/// <summary>
/// Данный класс предназначен для сущностей, которым необходимо
/// сохранять свое состояние и восстанавливать его после перезапуска
/// игры.
/// </summary>
public class StatefulEntity : MonoBehaviour
{
    private Vector3 m_initialPosition;
    private Quaternion m_initialRotation;

    private void Start()
    {
        m_initialPosition = transform.localPosition;
        m_initialRotation = transform.localRotation;

        GameManager.Instance.OnStart += OnGameStart;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnStart -= OnGameStart;
    }

    private void OnGameStart()
    {
        transform.localPosition = m_initialPosition;
        transform.localRotation = m_initialRotation;
    }
}
