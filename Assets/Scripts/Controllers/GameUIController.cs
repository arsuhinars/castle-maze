using UnityEngine;

public class GameUIController : MonoBehaviour
{
    private void Start()
    {
        var gameManager = GameManager.Instance;

        gameManager.OnStart += OnGameStart;
        gameManager.OnEnd += OnGameEnd;
        gameManager.OnPause += OnGamePause;
        gameManager.OnResume += OnGameResume;
    }

    private void OnGameStart()
    {
        UIManager.Instance.SetView(Defines.IN_GAME_VIEW);
    }

    private void OnGameEnd(GameManager.GameEndReason reason)
    {
        switch (reason)
        {
            case GameManager.GameEndReason.LevelEnds:
                UIManager.Instance.SetView(Defines.LEVEL_END_VIEW);
                break;
            case GameManager.GameEndReason.PlayerDied:
                UIManager.Instance.SetView(Defines.GAME_END_VIEW);
                break;
        }
    }

    private void OnGamePause()
    {
        UIManager.Instance.SetView(Defines.PAUSE_VIEW);
    }

    private void OnGameResume()
    {
        UIManager.Instance.SetView(Defines.IN_GAME_VIEW);
    }
}
