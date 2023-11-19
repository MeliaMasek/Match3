using UnityEngine;

public class StoreManager : MonoBehaviour
{
    public IntData playerCoins; // Reference to the IntData ScriptableObject

    // Method to purchase items
    public void Purchase(int cost)
    {
        if (playerCoins.value >= cost)
        {
            playerCoins.value -= cost;
            // Perform item purchase logic here
            Debug.Log("Item purchased!");
        }
        else
        {
            Debug.Log("Insufficient coins!");
            // Show a message to the player or handle the lack of coins
        }
    }
}