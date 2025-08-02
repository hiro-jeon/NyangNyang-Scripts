using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeShop : MonoBehaviour
{
    public Upgrade testUpgrade;
    public GameObject upgradeSlotPrefab; // 에디터에서 추가
    public const int maxSlots = 3;

    public class UpgradeSlot
    {
        public GameObject gameObject;
        public Image image;
        public Text level;
        public Text name;
        public Text details;
        public Upgrade upgrade;
    }

    public List<UpgradeSlot> slotList = new List<UpgradeSlot>();

    void Start()
    {
        CreateSlot(testUpgrade);
        SlotEventLister();
    }

    void CreateSlot(Upgrade upgrade)
    {
        if (upgradeSlotPrefab != null)
        {
            if (slotList.Count < maxSlots)
            {
                GameObject newSlotObj = Instantiate(upgradeSlotPrefab);

                // newSlotObj.name = "UpgradeSlot";
                newSlotObj.transform.SetParent(gameObject.transform.GetChild(0).GetChild(1).transform);

                // 참조 만들기
                UpgradeSlot newSlot = new UpgradeSlot
                {
                    gameObject = newSlotObj,
                    image = newSlotObj.transform.GetChild(0).GetChild(2).GetComponent<Image>(),
                    level = newSlotObj.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>(),
                    name = newSlotObj.transform.GetChild(0).GetChild(1).GetChild(1).GetComponent<Text>(),
                    details = newSlotObj.transform.GetChild(0).GetChild(1).GetChild(2).GetComponent<Text>(),
                    upgrade = upgrade
                };
                
                slotList.Add(newSlot);
                SetSlot(newSlot, upgrade);
            }
        }
    }

    void SetSlot(UpgradeSlot slot, Upgrade upgrade)
    {
        slot.image.sprite = upgrade.sprite;
        slot.level.text = new string('★', upgrade.level);
        slot.name.text = upgrade.objectName;
        slot.details.text = upgrade.details;
        slot.gameObject.SetActive(true);
        slot.image.enabled = true;
    }

    void SlotEventLister()
    {
        foreach (UpgradeSlot slot in slotList)
        {
            Button button = slot.gameObject.GetComponent<Button>();
            
            button.onClick.AddListener(() => OnSlotClicked(slot));
        }
    }

    void OnSlotClicked(UpgradeSlot slot)
    {
        Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        player.UpgradeCharacter(slot.upgrade);
        // slotList.Remove(slot);
        // slot.gameObject.SetActive(false);
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
    /*
        업그레이드 종류
        - 무기 추가
        - 새로운 무기
        - 기존 무기 공격력 강화
    */
}
