using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; } = null;

    private UIView m_activeView = null;
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

    public UIView GetView(string name) => m_viewsByName[name];

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;

        m_viewsByName = new();
        foreach (var view in GetComponentsInChildren<UIView>(true))
        {
            view.gameObject.SetActive(true);
            view.Hide();

            m_viewsByName[view.ViewName] = view;
        }
    }
}
