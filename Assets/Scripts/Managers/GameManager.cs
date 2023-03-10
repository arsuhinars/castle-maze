using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } = null;

    public enum GameEndReason
    {
        LevelEnds = 0,
        PlayerDied = 1,
    }

    public event Action OnReload;
    public event Action OnStart;
    public event Action<GameEndReason> OnEnd;
    public event Action OnPause;
    public event Action OnResume;

    public GameSettings Settings => m_settings;
    public bool IsStarted { get; private set; }
    public bool IsPaused { get; private set; }
    public int PlayerLives { get; private set; }

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

    public void EndGame(GameEndReason reason)
    {
        if (!IsStarted)
            return;

        ResumeGame();
        IsStarted = false;
        OnEnd?.Invoke(reason);
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

    /// <summary>
    /// Метод смены текущего уровня
    /// </summary>
    /// <param name="levelNumber">Номер уровня, начинающийся с 0</param>
    public void ChangeLevel(int levelNumber)
    {
        ProgressManager.Instance.CurrentLevel = levelNumber;
        SceneManager.LoadScene(m_settings.levelsScenes[levelNumber]);
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

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    private IEnumerator Start()
    {
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
