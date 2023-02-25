using System;
using UnityEngine;

public class PickupEntity : MonoBehaviour
{
    public event Action OnPickup;

    [SerializeField] private Transform m_model;
    [SerializeField] private PickupSettings m_settings;

    private void Start()
    {
        // Анимируем модель пикапа
        if (m_model != null)
        {
            m_model.rotation = Quaternion.identity;
            m_model.LeanRotateAroundLocal(
                Vector3.up, 360.0f, m_settings.rotationTime
            ).setLoopClamp();

            m_model.localPosition = new Vector3(0.0f, -m_settings.moveAmplitude, 0.0f);
            var moveLean = m_model.LeanMoveLocal(
                new Vector3(0.0f, m_settings.moveAmplitude, 0.0f), m_settings.moveTime
            );
            moveLean.setLoopPingPong();
            moveLean.setEaseInOutSine();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Обрабатываем подбор пикапа
        if (other.CompareTag(m_settings.targetTag))
        {
            OnPickup?.Invoke();
            HandlePickup();

            if (m_settings.disableOnPickup)
            {
                gameObject.SetActive(false);
            }
        }
    }

    protected virtual void HandlePickup() { }
}
