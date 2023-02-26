using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Outline))]
public class AbilityIcon : MonoBehaviour
{
    public Button Button => m_button;

    public Image IconImage => m_iconImage;

    public bool IsSelected
    {
        get => m_isSelected;
        set
        {
            if (!m_isSelected && value)
            {
                var size = m_bottomLine.sizeDelta;
                size.y = m_selectedLineHeight;
                m_bottomLine.LeanSize(size, m_animationTime).setEaseInOutCubic();
                LeanOutline(0f, m_selectedOutlineSize);
            }
            else if (m_isSelected && !value)
            {
                var size = m_bottomLine.sizeDelta;
                size.y = 0f;
                m_bottomLine.LeanSize(size, m_animationTime).setEaseInOutCubic();
                LeanOutline(m_selectedOutlineSize, 0f);
            }

            m_isSelected = value;
        }
    }

    [SerializeField] private Image m_iconImage;
    [SerializeField] private RectTransform m_bottomLine;
    [Header("Animation")]
    [SerializeField] private float m_selectedLineHeight;
    [SerializeField] private float m_selectedOutlineSize;
    [SerializeField] private float m_animationTime;

    private Button m_button;
    private Outline m_outline;
    private bool m_isSelected = false;

    private void Awake()
    {
        m_button = GetComponent<Button>();
        m_outline = GetComponent<Outline>();
    }

    private void Start()
    {
        m_isSelected = true;
        IsSelected = false;
    }

    private void LeanOutline(float from, float to)
    {
        LeanTween.value(
            m_outline.gameObject,
            (val) => m_outline.effectDistance = Vector2.one * val,
            from,
            to,
            m_animationTime
        ).setEaseInOutCubic();
    }
}
