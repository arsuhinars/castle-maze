using UnityEngine;
using UnityEngine.UI;

public class PauseView : UIView
{
    public override string ViewName => Defines.PAUSE_VIEW;

    [SerializeField] private Button m_continueButton;
    [SerializeField] private Button m_leaveButton;

    private void Start()
    {
        m_continueButton.onClick.AddListener(
            () => GameManager.Instance.ResumeGame()
        );
        m_leaveButton.onClick.AddListener(
            () => GameManager.Instance.LeaveGame()
        );
    }
}
