using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } = null;

    public event Action OnStart;
    public event Action OnEnd;
    public event Action OnPause;
    public event Action OnResume;

    public bool IsStarted
    {
        get => m_isStarted;
        set
        {
            if (!m_isStarted && value)
            {
                HandleStart();
            }
            else if (m_isStarted && !value)
            {
                HandleEnd();
            }

            m_isStarted = value;
        }
    }
    public bool IsPaused
    {
        get => m_isPaused;
        set
        {
            if (!m_isPaused && value)
            {
                HandlePause();
            }
            else if (m_isPaused && !value)
            {
                HandleResume();
            }

            m_isPaused = value;
        }
    }

    public PlayerEntity PlayerEntity => m_playerEntity;
    public CameraController CameraController => m_cameraController;

    private PlayerEntity m_playerEntity = null;
    private CameraController m_cameraController = null;
    private bool m_isStarted = false;
    private bool m_isPaused = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;

        m_playerEntity = FindObjectOfType<PlayerEntity>();
        m_cameraController = FindObjectOfType<CameraController>();
    }

    private IEnumerator Start()
    {
        // Пропускаем один кадр для того, чтобы все
        // объекты успели подписаться на события
        yield return null;

        m_isStarted = true;
        HandleStart();
    }

    private void HandleStart()
    {
        IsPaused = false;
        OnStart?.Invoke();
    }

    private void HandleEnd()
    {
        OnEnd?.Invoke();
    }

    private void HandlePause()
    {
        Time.timeScale = 0.0f;
        OnPause?.Invoke();
    }

    private void HandleResume()
    {
        Time.timeScale = 1.0f;
        OnResume?.Invoke();
    }
}
