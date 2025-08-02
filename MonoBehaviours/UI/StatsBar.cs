using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsBar : MonoBehaviour
{
    [HideInInspector]
    public Player character;
    public Stats stats;

    public Image hpMeterImage;
    public Image expMeterImage;
    public Text hpText;
    public Text levelText;
    float maxHitPoints;
    float maxExPoints;

    void Update()
    {
        if (character != null)
        {
            hpMeterImage.fillAmount = stats.hitPoints / character.maxHitPoints;
            hpText.text = "HP: " + (hpMeterImage.fillAmount * 100);
            expMeterImage.fillAmount = stats.exPoints / character.maxExPoints;
            levelText.text = "LV: " + stats.level;
        }
    }
}
