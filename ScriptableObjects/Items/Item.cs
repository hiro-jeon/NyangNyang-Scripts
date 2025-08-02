using UnityEngine;

[CreateAssetMenu(menuName = "Item")]

public class Item : ScriptableObject
{
    public enum ItemType
    {
        COIN,
        HEALTH,
        WEAPON
    }

    public string objectName;
    public Sprite sprite;
    public int price;
    public int quantity;
    public bool stackable;
    public ItemType itemType;
}
