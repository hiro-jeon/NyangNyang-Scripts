using UnityEngine;
using System.Collections;

public class Player : Character
{
    public Stats stats;

    public StatsBar statsBarPrefab;
    public Inventory inventoryPrefab;
    public UpgradeShop upgradeShopPrefab;

    [HideInInspector]
    public float maxExPoints;

    StatsBar statsBar;
    Inventory inventory;
    UpgradeShop upgradeShop;

    private void OnEnable()
    {
        ResetCharacter();
    }

    // 습득 가능(Consumable)
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("CanBePickedUp"))
        {
            Item hitObject = collision.gameObject.GetComponent<Consumable>().item;

            if (hitObject != null)
            {
                bool shouldDisappear = false;

                switch (hitObject.itemType)
                {
                    case Item.ItemType.COIN:
                        shouldDisappear = inventory.AddItem(hitObject);
                        break ;
                    case Item.ItemType.HEALTH:
                        shouldDisappear = AdjustHitPoints(hitObject.quantity);
                        break ;
                    default:
                        break ;
                }
                collision.gameObject.SetActive(false);
                if (shouldDisappear)
                {
                    collision.gameObject.SetActive(false);
                }
            }
        }
    }

    public bool AdjustHitPoints(int amount)
    {
        if (stats.hitPoints < maxHitPoints)
        {
            stats.hitPoints = stats.hitPoints + amount;
            print("Adjusted hitpoints by: " + amount + ". New value: " + stats.hitPoints);
            return true;
        }
        return false;
    }

    public override IEnumerator DamageCharacter(int damage, float interval)
    {
        stats.hitPoints = stats.hitPoints - damage;

        while (true)
        {
            if (stats.hitPoints <= float.Epsilon)
            {
                KillCharacter();
                break ;
            }

            if (interval > float.Epsilon)
            {
                yield return new WaitForSeconds(interval);
            }
            else
            {
                break ;
            }
        }
    }

    public void GetExp(int amount)
    {
        stats.exPoints += amount;
        if (stats.exPoints >= maxExPoints)
        {
            LevelUp();
        }
    }

    public void LevelUp()
    {
        stats.exPoints -= maxExPoints;
        Debug.Log("레벨업");
        if (upgradeShop != null)
        {
            upgradeShop.gameObject.SetActive(true);
            Time.timeScale = 0f;
        }
        
        stats.level++;
        maxExPoints = stats.level * stats.level / 2;
    }

    public override void KillCharacter()
    {
        gameObject.SetActive(false);
        // base.KillCharacter();

        Destroy(statsBar.gameObject);
        // Destroy(inventory.gameObject);
    }

    public override void ResetCharacter()
    {
        maxExPoints = stats.level * stats.level / 2;

        inventory = Instantiate(inventoryPrefab);
        statsBar = Instantiate(statsBarPrefab);
        upgradeShop = Instantiate(upgradeShopPrefab);
        upgradeShop.gameObject.SetActive(false);

        statsBar.character = this;

        stats.hitPoints = startingHitPoints;
        stats.exPoints = 0;
        stats.level = 1;

    }

    public void UpgradeCharacter(Upgrade upgrade)
    {
        Transform slot;

        switch (upgrade.slotType)
        {
            case Upgrade.SlotType.PRIMARY:
                slot = transform.GetChild(0);
                break ;
            case Upgrade.SlotType.SECONDARY:
                slot = transform.GetChild(1);
                break ;
            case Upgrade.SlotType.DRONE:
                slot = transform.GetChild(2);
                break ;
            case Upgrade.SlotType.SPECIAL:
                slot = transform.GetChild(3);
                break ;
            default:
                return ;
        }
        switch (upgrade.upgradeType)
        {
            case Upgrade.UpgradeType.STATS:
                Weapon weapon = slot.GetChild(0).GetComponent<Weapon>();
                weapon.speed += 0.25f; 
                break ;
            case Upgrade.UpgradeType.WEAPON:
                Destroy(slot.transform.GetChild(0).gameObject);
                GameObject weaponObj = Instantiate(upgrade.gameObjectPrefab);
                weaponObj.transform.SetParent(slot);
                break ;
        }
    }
}