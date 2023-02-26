using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } = null;

    public event Action OnReload;
    public event Action OnStart;
    public event Action OnEnd;
    public event Action OnPause;
    public event Action OnResume;

    public GameSettings Settings => m_settings;
    public bool IsStarted { get; private set; }
    public bool IsPaused { get; private set; }
    public int PlayerLives { get; private set; }
    public PlayerAbility[] AvailableAbilities => m_settings.playerAbilities;

    public PlayerEntity PlayerEntity => m_playerEntity;
    public CameraController CameraController => m_cameraController;
    public GameUIController GameUIController => m_gameUIGontroller;

    [SerializeField] private GameSettings m_settings;
    private PlayerEntity m_playerEntity = null;
    private CameraController m_cameraController = null;
    private GameUIController m_gameUIGontroller = null;
    private Dictionary<string, PlayerAbility> m_abilities;

    /// <summary>
    /// Метод запуска игры. Если <c>PlayerLives</c> равен или меньше нуля,
    /// то ничего не делает.
    /// </summary>
    public void StartGame()
    {
        if (PlayerLives <= 0)
            return;

        PlayerLives--;

        ResumeGame();
        IsStarted = true;
        OnStart?.Invoke();
    }

    public void EndGame()
    {
        if (!IsStarted)
            return;

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
        PlayerLives = m_settings.initialLivesCount + 1;
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

    public PlayerAbility GetAbilityByName(string name) => m_abilities[name];

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;

        m_playerEntity = FindObjectOfType<PlayerEntity>();
        m_cameraController = FindObjectOfType<CameraController>();
        m_gameUIGontroller = FindObjectOfType<GameUIController>();
    }

    private IEnumerator Start()
    {
        // TODO добавить подгрузку способностей вместо хардкодинга
        m_abilities = new();
        foreach (var ability in m_settings.playerAbilities)
        {
            m_abilities[ability.name] = ability;
        }

        // Пропускаем один кадр для того, чтобы все
        // объекты успели подписаться на события
        yield return null;

        ReloadGame();
    }
}
