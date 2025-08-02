using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsBar : MonoBehaviour
{
    [HideInInspector]
	public Player player;

    public Image hpMeterImage;
    public Image expMeterImage;
    public Text hpText;
    public Text levelText;
    float maxHitPoints;
    float maxExPoints;

    void Update()
    {
		Stats stats = player.stats;
		hpMeterImage.fillAmount = stats.hitPoints / player.maxHitPoints;
		expMeterImage.fillAmount = stats.exPoints / player.maxExPoints;
		hpText.text = "HP: " + (hpMeterImage.fillAmount * 100);
		levelText.text = "LV: " + stats.level;
    }
}
