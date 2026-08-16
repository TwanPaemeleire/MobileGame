using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public PlayerInventoryData InventoryData { get { return PlayerDataHandler.Instance.PlayerInventoryData; } set { PlayerDataHandler.Instance.PlayerInventoryData = value; } }
}