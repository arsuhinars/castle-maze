using UnityEngine;

public class AbilitySpirit : MonoBehaviour
{
    [SerializeField] private GameObject m_spiritAura;
    [SerializeField] private SpiritSettings m_settings;

    private PlayerEntity m_player;
    private PointerCapturer m_pointerCapturer;
    private int m_initialPlayerLayer;
    private bool m_isActive = false;

    private void Start()
    {
        m_player = GameManager.Instance.PlayerEntity;

        var inGameView = UIManager.Instance.GetViewByName(
            Defines.IN_GAME_VIEW
        ) as InGameView;

        m_pointerCapturer = inGameView.PointerCapturer;
        m_pointerCapturer.OnPointerClick += OnPointerClick;

        GameManager.Instance.OnStart += OnGameStart;

        DeactivateAura();
    }

    private void OnDestroy()
    {
        m_pointerCapturer.OnPointerClick -= OnPointerClick;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnStart -= OnGameStart;
        }

        DeactivateAura();
    }

    private void OnPointerClick()
    {
        if (!m_isActive)
        {
            ActivateAura();
        }
        else
        {
            DeactivateAura();
        }
    }

    private void OnGameStart()
    {
        DeactivateAura();
    }

    private void ActivateAura()
    {
        m_initialPlayerLayer = m_player.gameObject.layer;

        m_player.gameObject.layer = LayerMask.NameToLayer(
            m_settings.playerSpiritLayerName
        );

        m_spiritAura.SetActive(true);

        m_isActive = true;
    }

    private void DeactivateAura()
    {
        m_player.gameObject.layer = m_initialPlayerLayer;

        m_spiritAura.SetActive(false);

        m_isActive = false;
    }
}
