using System;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    private const string CURRENT_LEVEL_KEY = "CurrentLevel";
    private const string CURRENT_STAGE_KEY = "CurrentStage";
    private const string ABILITIES_KEY = "Abilitites";

    public static ProgressManager Instance { get; private set; } = null;

    public int CurrentLevel
    {
        get => PlayerPrefs.GetInt(CURRENT_LEVEL_KEY, 0);
        set => PlayerPrefs.SetInt(CURRENT_LEVEL_KEY, value);
    }

    public string[] Abilities
    {
        get
        {
            m_abilities ??= JsonUtility.FromJson<StringArrayWrapper>(
                PlayerPrefs.GetString(ABILITIES_KEY, @"{""array"":[]}")
            ).array;

            return m_abilities;
        }
        set
        {
            m_abilities = value;

            PlayerPrefs.SetString(
                ABILITIES_KEY,
                JsonUtility.ToJson(new StringArrayWrapper() { array = value })
            );
        }
    }

    [Serializable]
    private class StringArrayWrapper
    {
        public string[] array;
    }

    private string[] m_abilities = null;

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
}
