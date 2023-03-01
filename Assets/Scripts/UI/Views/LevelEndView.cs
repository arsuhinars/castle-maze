using UnityEngine;
using UnityEngine.UI;

public class LevelEndView : UIView
{
    public override string ViewName => Defines.LEVEL_END_VIEW;

    [SerializeField] private Button m_nextLevelButton;
    [SerializeField] private Button m_leaveButton;

    private void Start()
    {
        m_nextLevelButton.onClick.AddListener(
            () => GameManager.Instance.ChangeLevel(
                ProgressManager.Instance.CurrentLevel + 1
            )
        );
        m_leaveButton.onClick.AddListener(
            () => GameManager.Instance.LeaveGame()
        );
    }

    public override void Show()
    {
        base.Show();

        var currLevel = ProgressManager.Instance.CurrentLevel;
        var levelCount = GameManager.Instance.Settings.levelsScenes.Length;

        m_nextLevelButton.interactable = currLevel + 1 < levelCount;
    }
}
