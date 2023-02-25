using UnityEngine;

public class StageController : MonoBehaviour
{
    public bool IsActive
    {
        get => m_isActive;
        set
        {
            m_isActive = value;

            m_objectsRoot.SetActive(value);

            if (m_transitionRoot != null)
            {
                m_transitionRoot.SetActive(value);
            }
        }
    }

    public CheckpointEntity CurrentCheckpoint => m_checkpoints[m_checkpointIndex];

    public GameObject TransitionRoot => m_transitionRoot;

    [SerializeField]
    [Tooltip(
        "Массив чекпоинтов текущей стадии. Распологаются в порядке их" +
        "прохождения игроком"
    )]
    private CheckpointEntity[] m_checkpoints;
    [SerializeField] private GameObject m_objectsRoot = null;
    [SerializeField] private GameObject m_transitionRoot = null;

    private bool m_isActive = false;
    private int m_checkpointIndex = 0;

    private void Start()
    {
        for (int i = 0; i < m_checkpoints.Length; i++)
        {
            int index = i;
            m_checkpoints[i].OnPickup += () => HandleCheckpointPickup(index);
        }

        GameManager.Instance.OnReload += OnGameReload;
        GameManager.Instance.OnStart += OnGameStart;
    }

    private void OnGameReload()
    {
        m_checkpointIndex = 0;
    }

    private void OnGameStart()
    {
        // Сбрасываем все чекпоинты
        foreach (var checkpoint in m_checkpoints)
        {
            checkpoint.gameObject.SetActive(false);
        }

        // Активируем только следующий чекпоинт
        if (m_checkpointIndex + 1 < m_checkpoints.Length)
        {
            m_checkpoints[m_checkpointIndex + 1].gameObject.SetActive(true);
        }
    }

    private void HandleCheckpointPickup(int index)
    {
        if (index <= m_checkpointIndex)
            return;

        // Деактивируем все предыдущие чекпоинты
        for (int i = m_checkpointIndex; i < index; i++)
        {
            m_checkpoints[i].gameObject.SetActive(false);
        }

        // Активируем следующий чекпоинт
        if (index + 1 < m_checkpoints.Length)
        {
            m_checkpoints[index + 1].gameObject.SetActive(true);
        }

        m_checkpointIndex = index;
    }
}
