using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; } = null;

    [SerializeField] private UIView[] m_views;

    private UIView m_activeView;
    private Dictionary<string, UIView> m_viewsByName;

    public void ToggleView(string name)
    {
        if (m_activeView != null)
        {
            m_activeView.Hide();
        }

        m_activeView = m_viewsByName[name];
        m_activeView.Show();
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;
    }

    private void Start()
    {
        m_viewsByName = new();
        foreach (var view in m_views)
        {
            view.gameObject.SetActive(true);
            view.Hide();

            m_viewsByName[view.ViewName] = view;
        }
    }
}
