using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; } = null;

    public StageController CurrentStage => m_stages[m_currStageIndex];

    [SerializeField]
    [Tooltip(
        "Массив стадий текущего уровня. Стадии распологаются " +
        "в порядке их прохождения игроком"
    )]
    private StageController[] m_stages;

    private int m_currStageIndex = 0;

    public void SetActiveStage(int index)
    {
        // Отключаем объекты текущей стадии
        m_stages[m_currStageIndex].IsActive = false;
        
        // Включаем объекты новой
        m_currStageIndex = index;
        m_stages[m_currStageIndex].IsActive = true;
        
        // Включаем переход к следующей стадии
        if (m_currStageIndex + 1 < m_stages.Length)
        {
            var transition = m_stages[m_currStageIndex + 1].TransitionRoot;
            if (transition != null)
            {
                transition.SetActive(true);
            }
        }
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    private void Start()
    {
        GameManager.Instance.OnReload += OnGameReload;
    }

    private void OnGameReload()
    {
        m_currStageIndex = 0;

        foreach (var stage in m_stages)
        {
            stage.IsActive = false;
        }

        SetActiveStage(m_currStageIndex);
    }
}
