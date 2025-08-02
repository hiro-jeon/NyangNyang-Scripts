using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public GameObject slotPrefab; // 에디터에서 추가
    public const int numSlots = 5;
    Image[] itemImages = new Image[numSlots];
    public Item[] items = new Item[numSlots];
    GameObject[] slots = new GameObject[numSlots];

    void Start()
    {
        CreateSlots();
    }

    public void CreateSlots()
    {
        if (slotPrefab != null)
        {
            for (int i = 0; i < numSlots; i++)
            {
                GameObject newSlot = Instantiate(slotPrefab); // **
                newSlot.name = "ItemSlot_" + i;
                newSlot.transform.SetParent(gameObject.transform.GetChild(0).transform); // ChildObject[0]: InventoryBackground
                slots[i] = newSlot;
                itemImages[i] = newSlot.transform.GetChild(1).GetComponent<Image>(); // ChildObject[1]: Image 참조 지정
            }
        }
    }

    public bool AddItem(Item itemToAdd)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                items[i] = Instantiate(itemToAdd);
                items[i].quantity = 1;

                itemImages[i].sprite = items[i].sprite;
                itemImages[i].enabled = true;
                return true;
            }
            else if (items[i] != null
                     && items[i].itemType == itemToAdd.itemType
                     && itemToAdd.stackable == true)
            {
                // 아이템 존재 & 같은 아이템 타입 && Stackable
                    // [ ] 아이템 타입이 뭐지
                items[i].quantity = items[i].quantity + 1;
                Slot slotScript = slots[i].gameObject.GetComponent<Slot>(); // Slot Script
                Text quantityText = slotScript.qtyText;

                quantityText.enabled = true;
                quantityText.text = items[i].quantity.ToString();
                return true;
            }
        }

        return false;
    }
}
