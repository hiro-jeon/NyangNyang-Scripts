using UnityEngine;
using System.Collections;

public class Player : Character
{
    public Stats stats;
    public StatsBar statsBarPrefab;

    StatsBar statsBar;

    [HideInInspector]
    public float maxExPoints;

    private void OnEnable()
    {
        ResetCharacter();
    }

	// 	- DamageChracter()
	// 		- KillCharacter()
    public override IEnumerator DamageCharacter(int amount, float interval)
    {
		AdjustHealth(-amount);

        while (true)
        {
            if (stats.hitPoints <= float.Epsilon)
            {
                KillCharacter();
                break ;
            }

            if (interval > float.Epsilon)
                yield return new WaitForSeconds(interval);
            else
                break ;
        }
    }

	public bool AdjustHealth(int amount)
	{
        if (0 < stats.hitPoints && stats.hitPoints < maxHitPoints)
        {
			stats.hitPoints += amount;
            return true;
        }
        return false;
	}

    public override void KillCharacter()
    {
        gameObject.SetActive(false);
    }

	//	- ResetChracter()
    public override void ResetCharacter()
    {
		statsBar = Instantiate(statsBarPrefab);
		statsBar.player = this;

        stats.hitPoints = startingHitPoints;
        stats.exPoints = 0;
        stats.level = 1;
		maxExPoints = stats.level * stats.level / 2;
    }

    public void GetExp(int amount)
    {
        stats.exPoints += amount;
        if (stats.exPoints >= maxExPoints)
        {
            stats.exPoints -= maxExPoints;
            stats.level++;
            maxExPoints = stats.level * stats.level / 2;
            // LevelUp();
        }
    }

}
