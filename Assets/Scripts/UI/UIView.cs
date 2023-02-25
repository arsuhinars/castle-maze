using UnityEngine;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(CanvasGroup))]
public abstract class UIView : MonoBehaviour
{
    public abstract string ViewName { get; }
    public Canvas Canvas => m_canvas;
    public CanvasGroup CanvasGroup => m_canvasGroup;
    public bool IsShowed => m_isShowed;

    private Canvas m_canvas;
    private CanvasGroup m_canvasGroup;
    private bool m_isShowed = false;

    public virtual void Show()
    {
        m_canvas.enabled = true;
        m_canvasGroup.blocksRaycasts = true;
        m_isShowed = true;
    }

    public virtual void Hide()
    {
        m_canvas.enabled = false;
        m_canvasGroup.blocksRaycasts = false;
        m_isShowed = false;
    }

    private void Awake()
    {
        m_canvas = GetComponent<Canvas>();
        m_canvasGroup = GetComponent<CanvasGroup>();
    }
}
