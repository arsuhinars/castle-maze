
public class LevelEndPickup : PickupEntity
{
    protected override void HandlePickup()
    {
        GameManager.Instance.EndGame(
            GameManager.GameEndReason.LevelEnds
        );
    }
}
