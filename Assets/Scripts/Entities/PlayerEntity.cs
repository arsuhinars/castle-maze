using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : CreatureEntity
{
    public event Action OnAbilitiesChange;

    /// <summary>
    /// Перечисление доступных для игрока способностей
    /// </summary>
    public IEnumerable<string> AbilitiesNames => m_abilities;

    /// <summary>
    /// Название текущей активной способность игрока.
    /// Если равна <c>null</c>, то никакая способность в данный
    /// момент не активна.
    /// </summary>
    public string ActiveAbilityName
    {
        get => m_activeAbility.name;
        set
        {
            m_activeAbility = GameManager.Instance.GetAbilityByName(value);
            OnAbilityChange();
        }
    }


    private HashSet<string> m_abilities = new();
    private PlayerAbility m_activeAbility = null;
    private GameObject m_abilityObject;

    public void AddAbility(string abilityName)
    {
        m_abilities.Add(abilityName);

        // Обновляем список способностей
        var abilities = new string[m_abilities.Count];
        var enumerator = m_abilities.GetEnumerator();
        for (int i = 0; i < abilities.Length; i++)
        {
            enumerator.MoveNext();
            abilities[i] = enumerator.Current;
        }
        // Сохраняем его
        ProgressManager.Instance.Abilities = abilities;

        OnAbilitiesChange?.Invoke();
    }

    private void Start()
    {
        GameManager.Instance.OnStart += OnGameStart;
    }

    private void OnGameStart()
    {
        // Обновляем список способностей
        var abilities = ProgressManager.Instance.Abilities;
        foreach (var ability in abilities)
        {
            m_abilities.Add(ability);
        }
        OnAbilitiesChange?.Invoke();

        // Перемещаем игрока к чекпоинту
        var currCheckpoint = StageManager.Instance.CurrentStage.CurrentCheckpoint;

        transform.position = currCheckpoint.PlayerSpawn.position;

        // Перемещаем камеру к игроку
        var camController = GameManager.Instance.CameraController;
        camController.TargetPoint = transform.position;
        camController.MoveInstantly();

        // Спавним игрока
        Spawn();
    }

    protected override void Update()
    {
        base.Update();

        // Двигаем камеру к игроку
        var camController = GameManager.Instance.CameraController;
        var camTarget = transform.position;

        // Если игрок в полете, то не двигаем камеру по вертикали
        if (!CharacterController.isGrounded)
        {
            camTarget.y = camController.TargetPoint.y;
        }

        camController.TargetPoint = camTarget;
    }

    protected override void OnKilled()
    {
        GameManager.Instance.EndGame(
            GameManager.GameEndReason.PlayerDied
        );
    }

    private void OnAbilityChange()
    {
        if (m_abilityObject != null)
        {
            Destroy(m_abilityObject);
            m_abilityObject = null;
        }

        if (m_activeAbility == null)
            return;

        m_abilityObject = Instantiate(m_activeAbility.prefab, transform);
    }
}
