using UnityEngine;

public class PlayerUpgrade : MonoBehaviour
{
    public UpgradeShop upgradeShopPrefab;

    UpgradeShop upgradeShop;

	private void OnEnable()
	{
		upgradeShop = Instantiate(upgradeShopPrefab);
		upgradeShop.gameObject.SetActive(false);
	}

	private void OnDestroy()
	{
        Destroy(upgradeShop.gameObject);
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
