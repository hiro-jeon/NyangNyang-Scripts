using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    public LayerMask enemyLayer;

    public override void Attack(Transform target)
    {
		if (target != null)
		{
        	float distance = Vector2.Distance(transform.position, target.position);

			if (distance <= range)
			{
				DealDamage(target);
			}
		}
    }

    void DealDamage(Transform transform)
    {
        Enemy enemy = transform.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
    }
	/*
    public override void Attack(Vector2 direction)
    {
        Vector2 origin = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(origin, direction.normalized, range, enemyLayer);

        if (hit.collider != null)
        {
            DealDamage(hit.collider.transform);
        }
    }
	*/

}
