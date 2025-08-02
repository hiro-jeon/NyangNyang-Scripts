using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleWeapon : Weapon
{
    [Header("Target")]
    public LayerMask targetLayer;

    ContactFilter2D filter;
    List<Vector2> points;

	void Start()
	{
        filter = new ContactFilter2D();
        filter.SetLayerMask(targetLayer);
        filter.useLayerMask = true;
	}

    public override void Attack(Transform target)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, range, targetLayer);

        foreach (var hit in hits)
		{
			Enemy enemy = hit.transform.gameObject.GetComponent<Enemy>();
			if (enemy != null)
			{
				enemy.TakeDamage(damage);
			}
		}
    }

	void OnDrawGizmos()
	{
		if (points == null || points.Count == 0)
		{
			points = PointsBuilder.GetSectorPoints(
					transform.right,
					0,
					range,
					360,
					30
			);
		}

		Vector3 origin = transform.position;
		Gizmos.color = Color.red;

		for (int i = 0; i < points.Count - 1; i++)
		{
			Vector3 p1 = origin + (Vector3)points[i];
			Vector3 p2 = origin + (Vector3)points[i + 1];
			Gizmos.DrawLine(p1, p2);
		}

		Vector3 last = origin +  (Vector3)points[points.Count - 1];
		Vector3 first = origin + (Vector3)points[0];
		Gizmos.DrawLine(last, first);
	}
}
