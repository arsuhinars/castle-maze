using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Game/Game Settings")]
public class GameSettings : ScriptableObject
{
    public int initialLivesCount = 1;
    public PlayerAbility[] playerAbilities;
}
