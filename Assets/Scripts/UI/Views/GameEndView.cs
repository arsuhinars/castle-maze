using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameEndView : UIView
{
    public override string ViewName => Defines.GAME_END_VIEW;

    [SerializeField] private Button m_retryButton;
    [SerializeField] private Button m_restartButton;
    [SerializeField] private Button m_leaveButton;
    [SerializeField] private TextMeshProUGUI m_livesCountText;

    private void Start()
    {
        m_retryButton.onClick.AddListener(
            () => GameManager.Instance.StartGame()
        );
        m_restartButton.onClick.AddListener(
            () => GameManager.Instance.ReloadGame()
        );
        m_leaveButton.onClick.AddListener(
            () => GameManager.Instance.LeaveGame()
        );
    }

    public override void Show()
    {
        // Создаем строку формата "x<КОЛ-ВО ЖИЗНЕЙ>"
        var strBuilder = new StringBuilder(3);
        strBuilder.Append('x');
        strBuilder.Append(GameManager.Instance.PlayerLives);
        
        m_livesCountText.text = strBuilder.ToString();

        m_retryButton.interactable = GameManager.Instance.PlayerLives > 0;
        m_restartButton.gameObject.SetActive(!m_retryButton.interactable);

        base.Show();
    }
}
