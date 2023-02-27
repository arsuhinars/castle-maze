using System.Collections.Generic;
using UnityEngine;

public class InGameView : UIView
{
    public override string ViewName => Defines.IN_GAME_VIEW;

    public PointerCapturer PointerCapturer => m_pointerCapturer;

    [SerializeField] private PointerCapturer m_pointerCapturer;
    [SerializeField] private RectTransform m_abilitiesContainer;
    [SerializeField] private AbilityIcon m_abilityIconPrefab;

    private List<AbilityIcon> m_abilitiesIcons = new();
    private PlayerEntity m_player;

    private void Start()
    {
        m_player = GameManager.Instance.PlayerEntity;
        m_player.OnAbilitiesChange += UpdateAbilities;
    }

    private void UpdateAbilities()
    {
        // Очищаем старые иконки
        foreach (var abilityIcon in m_abilitiesIcons)
        {
            Destroy(abilityIcon.gameObject);
        }
        m_abilitiesIcons.Clear();

        // Временно активируем контейнер способностей
        m_abilitiesContainer.gameObject.SetActive(true);

        // Добавляем иконки для каждой способности
        int i = 0;
        foreach (var abilityName in m_player.AbilitiesNames)
        {
            int index = i;
            var currName = abilityName;
            var ability = GameManager.Instance.GetAbilityByName(abilityName);

            var abilityIcon = Instantiate(
                m_abilityIconPrefab, m_abilitiesContainer
            );
            abilityIcon.IconImage.sprite = ability.icon;
            abilityIcon.Button.onClick.AddListener(
                () => OnAbilitySelect(index, currName)
            );

            m_abilitiesIcons.Add(abilityIcon);

            i++;
        }

        // Если доступных способностей нет, то скрываем список
        m_abilitiesContainer.gameObject.SetActive(i > 0);
    }

    private void OnAbilitySelect(int index, string name)
    {
        foreach (var abilityIcon in m_abilitiesIcons)
        {
            abilityIcon.IsSelected = false;
        }

        m_abilitiesIcons[index].IsSelected = true;

        m_player.ActiveAbilityName = name;
    }
}
