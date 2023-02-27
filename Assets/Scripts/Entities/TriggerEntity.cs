using UnityEngine;
using UnityEngine.Events;

public class TriggerEntity : MonoBehaviour
{
    [SerializeField]
    private TriggerSettings m_settings;
    [SerializeField]
    private UnityEvent m_onTriggerEnter = new();

    private bool m_triggeredFlag = false;

    private void Start()
    {
        GameManager.Instance.OnStart += OnGameStart;
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnStart -= OnGameStart;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (m_triggeredFlag && m_settings.triggerOnce)
        {
            return;
        }

        if (other.CompareTag(m_settings.targetTag))
        {
            m_triggeredFlag = true;
            m_onTriggerEnter.Invoke();
        }
    }

    private void OnGameStart()
    {
        if (m_settings.resetOnGameRestart)
        {
            m_triggeredFlag = false;
        }
    }
}
