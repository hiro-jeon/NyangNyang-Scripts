using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewAutoAttack : MonoBehaviour
{
	private CircleCollider2D detector;

	private List<Transform> enemies = new();

	private Coroutine attackRoutine;
	private Weapon weapon;

	void Start()
	{
		weapon = GetComponentInChildren<Weapon>();

		if (weapon == null)
		{
			Destroy(this);
			return ;
		}

		if (detector == null)
		{
			detector = gameObject.AddComponent<CircleCollider2D>();
			detector.radius = weapon.range;
			detector.isTrigger = true;
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Enemy"))
		{
			enemies.Add(other.transform);

			if (attackRoutine == null)
			{
				attackRoutine = StartCoroutine(Attack());
			}
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Enemy"))
		{
			enemies.Remove(other.transform);
			if (enemies.Count == 0 && attackRoutine != null)
			{
				StopCoroutine(attackRoutine);
				attackRoutine = null;
			}
		}
	}

    transform findnearest()
    {
        transform nearest = null;
        float mindist = mathf.infinity;

        for (int i = enemies.Count -1; i >= 0; i--)
        {
			if (enemies[i] == null)
			{
				enemies.RemoveAt(i);
				continue ;
			}

            float dist = Vector2.Distance(transform.position, enemies[i].transform.position);
            if (dist < minDist)
            {
                nearest = enemies[i];
                minDist = dist;
            }
        }
        return (nearest);
    }    

    IEnumerator Attack()
    {
		Transform target;

        while (enemies.Count > 0)
        {
            yield return new WaitForSeconds(1 / weapon.speed);
			target = FindNearest();
            weapon.Attack(target);
        }
		attackRoutine = null;
    }
}
