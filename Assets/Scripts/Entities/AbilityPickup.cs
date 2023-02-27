using UnityEngine;

public class AbilityPickup : PickupEntity
{
    [SerializeField] private string m_abilityName;

    protected override void HandlePickup()
    {
        GameManager.Instance.PlayerEntity.AddAbility(m_abilityName);
    }
}
