using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private PlayerInventoryData _inventoryData { get { return PlayerDataHandler.Instance.PlayerInventory; } set { PlayerDataHandler.Instance.PlayerInventory = value; } }
}