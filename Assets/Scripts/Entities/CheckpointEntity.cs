using UnityEngine;

public class CheckpointEntity : PickupEntity
{
    public Transform PlayerSpawn => m_playerSpawn;

    [SerializeField]
    private Transform m_playerSpawn;
}
