using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } = null;

    public event Action OnReload;
    public event Action OnStart;
    public event Action OnEnd;
    public event Action OnPause;
    public event Action OnResume;

    public bool IsStarted { get; private set; }
    public bool IsPaused { get; private set; }

    public PlayerEntity PlayerEntity => m_playerEntity;
    public CameraController CameraController => m_cameraController;

    private PlayerEntity m_playerEntity = null;
    private CameraController m_cameraController = null;

    public void StartGame()
    {
        ResumeGame();
        IsStarted = true;
        OnStart?.Invoke();
    }

    public void EndGame()
    {
        ResumeGame();
        IsStarted = false;
        OnEnd?.Invoke();
    }

    public void PauseGame()
    {
        if (!IsStarted || IsPaused)
            return;

        IsPaused = true;
        Time.timeScale = 0f;
        OnPause?.Invoke();
    }

    public void ResumeGame()
    {
        if (!IsStarted || !IsPaused)
            return;

        IsPaused = false;
        Time.timeScale = 1f;
        OnResume?.Invoke();
    }

    /// <summary>
    /// Метод полной перезагрузки игры. При этом вызывается событие <c>OnReload</c>,
    /// при котором все компоненты должны сбросить свое состояние до начального.
    /// </summary>
    public void ReloadGame()
    {
        OnReload?.Invoke();
        StartGame();
    }

    /// <summary>
    /// Метод для выхода из игры. Должен возвращать в главное меню либо,
    /// закрывать игру.
    /// </summary>
    public void LeaveGame()
    {
        Application.Quit();
    }

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

        ReloadGame();
    }
}
