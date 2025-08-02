using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
	private Player player;

    public Inventory inventoryPrefab;

    Inventory inventory;
	
	private void OnEnable()
	{
		player = GetComponent<Player>();
		inventory = Instantiate(inventoryPrefab);
	}

	private void OnDestroy()
	{
		Destroy(inventory.gameObject);
	}

    // 아이템 습득
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
                        shouldDisappear = player.AdjustHealth(hitObject.quantity);
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

}
