using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public PlayerInventoryData InventoryData => PlayerDataHandler.Instance.PlayerInventoryData;
}