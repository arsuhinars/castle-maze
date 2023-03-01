using UnityEngine;
using UnityEngine.Events;

public class TriggerEntity : MonoBehaviour
{
    [SerializeField]
    private TriggerSettings m_settings;
    [SerializeField]
    private UnityEvent m_onTriggerEnter = new();
    [SerializeField]
    private UnityEvent m_onTriggerExit = new();

    private bool m_enterTriggerFlag = false;
    private bool m_exitTriggerFlag = false;

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
        if (m_enterTriggerFlag && m_settings.triggerOnce)
        {
            return;
        }

        if (other.CompareTag(m_settings.targetTag))
        {
            m_enterTriggerFlag = true;
            m_onTriggerEnter.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (m_exitTriggerFlag && m_settings.triggerOnce)
        {
            return;
        }

        if (other.CompareTag(m_settings.targetTag))
        {
            m_exitTriggerFlag = true;
            m_onTriggerExit.Invoke();
        }
    }

    private void OnGameStart()
    {
        if (m_settings.resetOnGameRestart)
        {
            m_enterTriggerFlag = false;
        }
    }
}
