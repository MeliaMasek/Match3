using UnityEngine;

public class StoreManager : MonoBehaviour
{
    public IntData playerCoins; // Reference to the IntData ScriptableObject

    public GameObject[] prefabSet1; // First set of prefabs
    public Sprite[] spriteSet1; // Sprites corresponding to the first prefab set

    public GameObject[] prefabSet2; // Second set of prefabs
    public Sprite[] spriteSet2; // Sprites corresponding to the second prefab set

    public GameObject[] prefabSet3;
    public Sprite[] spriteSet3;
    
    public GameObject[] prefabSet4;
    public Sprite[] spriteSet4;
    // Method to purchase items and apply the selected prefab set
    public void PurchaseAndApplySet1(int cost)
    {
        if (playerCoins.value >= cost)
        {
            playerCoins.value -= cost;
            ApplyColorScheme(prefabSet1, spriteSet1);
            Debug.Log("Item purchased and applied Set 1!");
        }
        else
        {
            Debug.Log("Insufficient coins!");
        }
    }

    public void PurchaseAndApplySet2(int cost)
    {
        if (playerCoins.value >= cost)
        {
            playerCoins.value -= cost;
            ApplyColorScheme(prefabSet2, spriteSet2);
            Debug.Log("Item purchased and applied Set 2!");
        }
        else
        {
            Debug.Log("Insufficient coins!");
        }
    }

    public void PurchaseAndApplySet3(int cost)
    {
        if (playerCoins.value >= cost)
        {
            playerCoins.value -= cost;
            ApplyColorScheme(prefabSet3, spriteSet3);
            Debug.Log("Item purchased and applied Set 3!");
        }
        else
        {
            Debug.Log("Insufficient coins!");
        }
    }
    
    public void PurchaseAndApplySet4(int cost)
    {
        if (playerCoins.value >= cost)
        {
            playerCoins.value -= cost;
            ApplyColorScheme(prefabSet4, spriteSet4);
            Debug.Log("Item purchased and applied Set 4!");
        }
        else
        {
            Debug.Log("Insufficient coins!");
        }
    }
    
    // Apply the selected prefab set's sprite set to the corresponding prefabs
    void ApplyColorScheme(GameObject[] prefabs, Sprite[] sprites)
    {
        for (int i = 0; i < prefabs.Length; i++)
        {
            SpriteRenderer prefabRenderer = prefabs[i].GetComponent<SpriteRenderer>();

            if (prefabRenderer != null && i < sprites.Length)
            {
                prefabRenderer.sprite = sprites[i];
            }
            else
            {
                Debug.Log("Prefab " + i + " does not have a SpriteRenderer component or sprite is missing!");
            }
        }
    }
}