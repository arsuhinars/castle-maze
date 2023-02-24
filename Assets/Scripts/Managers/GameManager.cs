using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } = null;

    public event Action OnStart;
    public event Action OnPlayerDeath;
    public event Action OnPause;
    public event Action OnResume;

    public bool IsStarted { get; set; }
    public bool IsPaused { get; set; }

    public PlayerEntity PlayerEntity => m_playerEntity;
    public CameraController CameraController => m_cameraController;

    private PlayerEntity m_playerEntity = null;
    private CameraController m_cameraController = null;

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

        OnStart?.Invoke();
    }
}
