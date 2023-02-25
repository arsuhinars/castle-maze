using UnityEngine;

public class KillZone : MonoBehaviour
{
    [SerializeField] private KillZoneSettings m_settings;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(m_settings.targetTag) &&
            other.TryGetComponent<CreatureEntity>(out var creature))
        {
            creature.Kill();
        }
    }
}
