using UnityEngine;
using UnityEngine.UI;

public class GameEndView : UIView
{
    public override string ViewName => Defines.GAME_END_VIEW;

    [SerializeField] private Button m_retryButton;
    [SerializeField] private Button m_restartButton;
    [SerializeField] private Button m_leaveButton;

    private void Start()
    {
        m_retryButton.onClick.AddListener(
            () => GameManager.Instance.StartGame()
        );
        // TODO
        m_leaveButton.onClick.AddListener(
            () => GameManager.Instance.LeaveGame()
        );
    }
}
