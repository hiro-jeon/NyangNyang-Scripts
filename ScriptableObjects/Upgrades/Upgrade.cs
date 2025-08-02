using UnityEngine;

[CreateAssetMenu(menuName = "Upgrade")]

public class Upgrade : ScriptableObject
{
    public enum UpgradeType
    {
        STATS,
        WEAPON,
    }

    public enum SlotType
    {
        NONE,
        PRIMARY,
        SECONDARY,
        DRONE,
        SPECIAL
    }
    
    public string objectName;
    public Sprite sprite;
    public string details;
    public int level;
    public UpgradeType upgradeType;
    public SlotType slotType;
    public GameObject gameObjectPrefab;
}
