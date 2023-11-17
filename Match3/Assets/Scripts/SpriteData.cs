using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class SpriteData : ScriptableObject
{
    public Sprite value;

    public void SetValue(Sprite sprite)
    {
        value = sprite;
    }
}