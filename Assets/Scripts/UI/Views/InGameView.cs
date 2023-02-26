using System.Collections.Generic;
using UnityEngine;

public class InGameView : UIView
{
    public override string ViewName => Defines.IN_GAME_VIEW;

    public SavePointerZone SaveZone => m_saveZone;

    [SerializeField] private SavePointerZone m_saveZone;
    [SerializeField] private RectTransform m_abilitiesContainer;
    [SerializeField] private AbilityIcon m_abilityIconPrefab;

    private List<AbilityIcon> m_abilitiesIcons = new();

    private void Start()
    {
        GameManager.Instance.OnStart += OnGameStart;
    }

    private void OnGameStart()
    {
        // Очищаем иконки
        foreach (var abilityIcon in m_abilitiesIcons)
        {
            Destroy(abilityIcon.gameObject);
        }
        m_abilitiesIcons.Clear();

        // Добавляем иконки каждой способности
        int index = 0;
        foreach (var ability in GameManager.Instance.AvailableAbilities)
        {
            var currAbility = ability;
            int currIndex = index;

            var abilityIcon = Instantiate(
                m_abilityIconPrefab, m_abilitiesContainer
            );
            abilityIcon.IconImage.sprite = ability.icon;
            abilityIcon.Button.onClick.AddListener(
                () => OnAbilitySelect(currIndex, currAbility)
            );

            m_abilitiesIcons.Add(abilityIcon);

            index++;
        }

        // Если доступных способностей нет, то скрываем список
        m_abilitiesContainer.gameObject.SetActive(index > 0);
    }

    private void OnAbilitySelect(int index, PlayerAbility ability)
    {
        foreach (var abilityIcon in m_abilitiesIcons)
        {
            abilityIcon.IsSelected = false;
        }

        m_abilitiesIcons[index].IsSelected = true;

        GameManager.Instance.PlayerEntity.ActiveAbility = ability;
    }
}
